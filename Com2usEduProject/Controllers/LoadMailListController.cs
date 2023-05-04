using Com2usEduProject.DBSchema;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Databases;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class LoadMailList : ControllerBase
{

	readonly IGameDb _gameDb;
	readonly ILogger<LoadMailList> _logger;

	const int PAGE_SIZE = 20;

	public LoadMailList(ILogger<LoadMailList> logger, IGameDb gameDb)
	{
		_logger = logger;
		_gameDb = gameDb;
	}

	[HttpPost]
	public async Task<LoadMailListResponse> Post(LoadMailListRequest request)
	{
		var response = new LoadMailListResponse();

		// 메일함의 총 메일 갯수를 로드해, 총 페이지 수를 구함
		(var errorCode, response.TotalPageCount) = await _gameDb.MailTable.SelectCountAsync(request.PlayerId);
		response.TotalPageCount /= PAGE_SIZE;
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoadMailListError], new {ErrorCode = errorCode, PlayerId = request.PlayerId},
				"Load Mail Count Failed");

			response.Result = errorCode;
			return response;
		}
		
		// 원하는 페이지의 메일 로드
		(errorCode, response.MailList) = await _gameDb.MailTable.SelectList(request.PlayerId, PAGE_SIZE, PAGE_SIZE * request.PageNo);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoadMailListError], new {ErrorCode = errorCode, PlayerId = request.PlayerId},
				"Load Mail List Failed");
			
			response.Result = errorCode;
			return response;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APILoadMail], new {PlayerId = request.PlayerId, PageNo = request.PageNo}, "LoadMailList Success");
		return response;
	}
}
