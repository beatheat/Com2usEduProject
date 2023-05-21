using Com2usEduProject.Databases;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class LoadMail : ControllerBase
{
	readonly IGameDb _gameDb;
	readonly ILogger<LoadMail> _logger;
	
	public LoadMail(ILogger<LoadMail> logger, IGameDb gameDb)
	{
		_logger = logger;
		_gameDb = gameDb;
	}

	[HttpPost]
	public async Task<LoadMailResponse> Post(LoadMailRequest request)
	{
		var response = new LoadMailResponse();
		
		// 메일 로드
		var (errorCode, mail) = await _gameDb.MailTable.SelectAsync(request.MailId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Select Mail Fail");
			response.Result = errorCode;
			return response;
		}

		// 메일 소유주와 요청자가 다르다면 요청 무시
		if (mail.PlayerId != request.PlayerId)
		{
			errorCode = ErrorCode.LoadMailRequestFromNonOwnerPlayer;
			LogError(errorCode, request, "Mail Request From Non Owner Player");
			response.Result = errorCode;
			return response;
		}
		
		response.Mail = mail;

		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APILoadMail], 
			new {PlayerId = request.PlayerId, Mail = request.MailId}, "LoadMail Success");
		return response;
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoadMailError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}