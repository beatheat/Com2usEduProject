using Com2usEduProject.Databases;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class LoadMail
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
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoadMailError],
				new {ErrorCode = errorCode, PlayerId = request.PlayerId, MailId = request.MailId},
				"Select Mail Failed");
			response.Result = errorCode;
			return response;
		}

		// 메일 소유주와 요청자가 다르다면 요청 무시
		if (mail.PlayerId != request.PlayerId)
		{
			_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APILoadMail],
				new {ErrorCode = errorCode, PlayerId = request.PlayerId, Mail = mail},
				"Mail Request From Non Owner Player");
			response.Result = ErrorCode.LoadMailRequestFromNonOwnerPlayer;
			return response;
		}
		
		// 로드한 메일을 응답에
		// 2포함
		response.Mail = mail;

		// 메일아이템 로드
		(errorCode, response.MailItems) = await _gameDb.MailItemTable.SelectListAsync(mail.Id);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoadMailError],
				new {ErrorCode = errorCode, PlayerId = request.PlayerId, Mail = mail},
				"Select Mail Item List Fail");
			response.Result = errorCode;
			return response;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APILoadMail], 
			new {PlayerId = request.PlayerId, Mail = request.MailId}, "LoadMail Success");
		return response;
	}
}