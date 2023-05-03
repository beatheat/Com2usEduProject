using Com2usEduProject.DBSchema;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Databases;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceiveMailItemController
{
	readonly IMasterDb _masterDb;
	readonly IGameDb _gameDb;
	readonly ILogger<ReceiveMailItemController> _logger;

	public ReceiveMailItemController(ILogger<ReceiveMailItemController> logger,IGameDb gameDb, IMasterDb masterDb)
	{
		_logger = logger;
		_gameDb = gameDb;
		_masterDb = masterDb;
	}

	[HttpPost]
	public async Task<ReceiveMailResponse> Post(ReceiveMailRequest request)
	{
		var response = new ReceiveMailResponse();

		var (errorCode, mail) = await _gameDb.LoadMailAsync(request.MailId);
		if (errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}

		if (mail.IsItemReceived)
		{
			response.Result = ErrorCode.ReceiveMailItemAlready;
			return response;
		}

		(errorCode, var errorInsertedItemIds) = await ReceiveItemFromMail(mail);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.ReceiveMail], new {Mail = mail}, "Receive Item From Mail Failed, Cancel Mail Item Receive");
			
			response.Result = errorCode;
			errorCode = await CancelMailItemReceive(errorInsertedItemIds);
			
			if (errorCode != ErrorCode.None)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.ReceiveMail], new {Mail = mail, ErrorInsertedItems = errorInsertedItemIds}, 
					"Cancel Mail Item Receive Failed");
				response.Result = errorCode;
			}
			
			return response;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.ReceiveMail], new {PlayerId = request.PlayerId, Mail = mail}, "ReceiveMail Success");
		return response;
	}

	private async Task<(ErrorCode, IList<int>)> ReceiveItemFromMail(Mail mail)
	{
		List<int> insertedItems = new List<int>();
		ErrorCode errorCode;
		for (int i = 0; i < mail.ItemCode.Length; i++)
		{
			if (mail.ItemCode[i] == Mail.ITEM_NOT_EXIST)
				continue;
			
			//메일 아이템 정보 로드
			(errorCode, var item) = _masterDb.GetItem(mail.ItemCode[i]);
			if (errorCode != ErrorCode.None)
			{
				return (errorCode, insertedItems);
			}

			//메일 아이템 플레이어 아이템 테이블에 삽입
			(errorCode, var playerItemId) = await _gameDb.InsertPlayerItemAsync(mail.PlayerId, item, mail.ItemCount[i]);
			insertedItems.Add(playerItemId);
			
			if (errorCode != ErrorCode.None)
			{
				return (errorCode, insertedItems);
			}
		}
		
		// 메일 아이템 수령 체크
		errorCode = await _gameDb.UpdateMailItemReceivedToTrueAsync(mail.Id);
		if (errorCode != ErrorCode.None)
		{
			return (errorCode, insertedItems);
		}
		return (ErrorCode.None, new List<int>());
	}

	private async Task<ErrorCode> CancelMailItemReceive(IList<int> errorInsertedItemIds)
	{
		foreach (var errorItemId in errorInsertedItemIds)
		{
			var errorCode = await _gameDb.DeletePlayerItemAsync(errorItemId);
			if (errorCode != ErrorCode.None)
			{
				return errorCode;
			}
		}

		return ErrorCode.None;
	}
}