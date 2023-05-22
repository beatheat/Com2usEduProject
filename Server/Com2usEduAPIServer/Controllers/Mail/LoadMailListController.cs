using Com2usEduAPIServer.Databases;
using Com2usEduAPIServer.ReqRes;
using Com2usEduAPIServer.Tools;
using Com2usEduAPIServer.Databases.Schema;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduAPIServer.Controllers;

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
		var (errorCode, totalPageCount) = await _gameDb.MailTable.SelectCountAsync(request.PlayerId);
		totalPageCount = (int) Math.Ceiling((double) totalPageCount / (double) PAGE_SIZE);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Select Mail Count Fail");
			response.Result = errorCode;
			return response;
		}
		
		// 원하는 페이지의 메일 로드
		(errorCode, var mailList) = await _gameDb.MailTable.SelectList(request.PlayerId, PAGE_SIZE, PAGE_SIZE * (request.PageNo-1));
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Select Mail List Fail");
			response.Result = errorCode;
			return response;
		}

		response.TotalPageCount = totalPageCount;
		response.MailList = mailList;
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APILoadMailList],
			new {PlayerId = request.PlayerId, PageNo = request.PageNo}, "LoadMailList Success");
		return response;
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoadMailListError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}
