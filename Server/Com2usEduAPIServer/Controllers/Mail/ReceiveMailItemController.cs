using Com2usEduAPIServer.Databases;
using Com2usEduAPIServer.GameLogic;
using Com2usEduAPIServer.ReqRes;
using Com2usEduAPIServer.Tools;
using Com2usEduAPIServer.Databases.Schema;
using Com2usEduAPIServer.Databases.Schema.Extension;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceiveMailItemController : ControllerBase
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
			LogError(errorCode, request, "Select Mail Fail");
			response.Result = errorCode;
			return response;
		}

		// 메일 아이템을 이미 받았다면 요청 무시
		if (mail.IsItemReceived)
		{
			errorCode = ErrorCode.ReceiveMailItemAlready;
			LogError(errorCode, request, "Mail Item Already Received");
			response.Result = errorCode;
			return response;
		}

		// 요청자와 메일의 소유주가 다르다면 요청 무시
		if (mail.PlayerId != request.PlayerId)
		{
			errorCode = ErrorCode.ReceiveMailItemRequestFromNonOwnerPlayer;
			LogError(errorCode, request, "Receive MailItem Request From Non Owner Player");
			response.Result = errorCode;
			return response;
		}

		// 메일 아이템을 플레이어에게 타입에 맞게 배분
		var itemReceiver = new PlayerItemReceiver(_logger, _masterDb, _gameDb);
		errorCode = await itemReceiver.Receive(mail.PlayerId, mail.GetItemList());
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Insert Mail Item To Player Fail");
			await itemReceiver.Rollback(mail.PlayerId);
			response.Result = errorCode;
			return response;
		}
		
		// 메일 아이템을 받음으로 체크
		errorCode = await _gameDb.MailTable.UpdateItemReceivedToTrue(mail.Id);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Update Mail Item Receive Flag To True Fail");
			await itemReceiver.Rollback(mail.PlayerId);
			response.Result = errorCode;
			return response;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIReceiveMailItem], new {PlayerId = request.PlayerId, Mail = mail}, "ReceiveMail Success");
		return response;
	}

	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveInAppPurchaseItemError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}