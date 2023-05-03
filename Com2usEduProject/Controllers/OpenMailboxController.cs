using Com2usEduProject.DBSchema;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Databases;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class OpenMailbox : ControllerBase
{

	readonly IGameDb _gameDb;
	readonly ILogger<OpenMailbox> _logger;

	public OpenMailbox(ILogger<OpenMailbox> logger, IGameDb gameDb)
	{
		_logger = logger;
		_gameDb = gameDb;
	}

	[HttpPost]
	public async Task<OpenMailboxResponse> Post(OpenMailboxRequest request)
	{
		var response = new OpenMailboxResponse();

		// 메일함의 총 페이지 수를 로드
		(var errorCode, response.MailBoxPageCount) = await _gameDb.LoadMailboxPageCountAsync(request.PlayerId);
		if (errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}
		
		// 원하는 페이지의 메일 로드
		(errorCode, response.MailboxPage) = await _gameDb.LoadMailboxPageAsync(request.PlayerId, request.PageNo);
		if (errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.OpenMailbox], new {PlayerId = request.PlayerId, PageNo = request.PageNo}, "OpenMailBox Success");
		return response;
	}
}
