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
	public async Task<ReceiveMailItemResponse> Post(ReceiveMailItemRequest request)
	{
		var response = new ReceiveMailItemResponse();

		// 메일 로드
		var (errorCode, mail) = await _gameDb.MailTable.SelectAsync(request.MailId);
		if (errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}

		// 메일 아이템을 이미 받았다면 요청 무시
		if (mail.IsItemReceived)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveMailItemError], 
				new {ErrorCode = ErrorCode.ReceiveMailItemAlready, PlayerId = request.PlayerId, MailId = request.MailId}, 
				"Mail Item Already Received");
			
			response.Result = ErrorCode.ReceiveMailItemAlready;
			return response;
		}

		// 요청자와 메일의 소유주가 다르다면 요청 무시
		if (mail.PlayerId != request.PlayerId)
		{
			_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIReceiveMailItemError], 
				new {ErrorCode = ErrorCode.ReceiveMailItemRequestFromNonOwnerPlayer, PlayerId = request.PlayerId, Mail = mail}, 
				"Receive MailItem Request From Non Owner Player");
			
			response.Result = ErrorCode.ReceiveMailItemRequestFromNonOwnerPlayer;
			return response;
		}

		// 메일 아이템 리스트 로드
		(errorCode, var mailItemList) = await _gameDb.MailItemTable.SelectListAsync(request.MailId);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveMailItemError], 
				new {PlayerId = request.PlayerId, MailId = request.MailId, ErrorCode = errorCode}, 
				"Select Mail Item List Fail");
			response.Result = errorCode;
			return response;
		}

		// 메일 아이템을 플레이어 아이템에 삽입
		var insertedPlayerItemIds = new List<int>();
		foreach (var mailItem in mailItemList)
		{
			(errorCode, var playerItemId) = await InsertMailItemToPlayer(mail.PlayerId, mailItem);
			if (errorCode != ErrorCode.None)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveMailItemError], 
					new
					{
						PlayerId = request.PlayerId, MailItem = mailItem, ErrorCode = errorCode,
						ErrorInsertedPlayerItemIds = insertedPlayerItemIds,
					}, 
					"Insert Mail Item To Player Fail");
				
				await Rollback(insertedPlayerItemIds);
				response.Result = errorCode;
				
				return response;
			}
			insertedPlayerItemIds.Add(playerItemId);
		}

		// 메일 아이템을 받음으로 체크
		errorCode = await _gameDb.MailTable.UpdateItemReceivedToTrue(mail.Id);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveMailItemError], 
				new {PlayerId = request.PlayerId, Mail = mail, ErrorCode = errorCode}, 
				"Insert Mail Item To Player Fail");
				
			await Rollback(insertedPlayerItemIds);
			response.Result = errorCode;
			return response;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIReceiveMailItem], new {PlayerId = request.PlayerId, Mail = mail}, "ReceiveMail Success");
		return response;
	}

	private async Task<(ErrorCode, int)> InsertMailItemToPlayer(int playerId, MailItem mailItem)
	{
		//메일 아이템 정보 로드
		var errorCode = ErrorCode.None;
		(errorCode, var item) = _masterDb.GetItem(mailItem.ItemCode);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveMailItemError], 
				new {MailItem = mailItem, ErrorCode = errorCode}, 
				"InsertMailItemToPlayer - Unknown Item Code");
			return (errorCode,-1);
		}

		//메일 아이템을 플레이어 아이템 테이블에 삽입
		(errorCode, var playerItemId) = await _gameDb.PlayerItemTable.InsertAsync(playerId, item, mailItem.ItemCount);

		return (errorCode, playerItemId);
	}

	private async Task Rollback(IList<int> errorInsertedItemIds)
	{
		foreach (var errorItemId in errorInsertedItemIds)
		{
			var errorCode = await _gameDb.PlayerItemTable.DeleteAsync(errorItemId);
			if (errorCode != ErrorCode.None)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveMailItemError], 
					new {PlayerItemId = errorItemId, ErrorCode = errorCode}, 
					"Rollback - Delete Player Item Failed");
				
			}
		}
	}
}