using Com2usEduProject.DBSchema;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Services;
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
			response.Result = ErrorCode.ReceiveMailAlready;
			return response;
		}
		
		(errorCode, var item) = _masterDb.GetItem(mail.ItemCode);
		if (errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}
		
		(errorCode, _) = await _gameDb.InsertPlayerItemAsync(request.PlayerId, item, mail.ItemCount);
		if (errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}
		
		errorCode = await _gameDb.ReceiveMailItem(request.MailId);
		if (errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
		}

		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.ReceiveMail], new {PlayerId = request.PlayerId, Mail = mail}, "ReceiveMail Success");
		return response;
	}
	
}