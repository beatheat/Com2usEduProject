using Com2usEduProject.Databases;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

public class ShowPlayerItem
{
	readonly IGameDb _gameDb;
	readonly ILogger<ShowPlayerItem> _logger;
	
	public ShowPlayerItem(ILogger<ShowPlayerItem> logger, IGameDb gameDb)
	{
		_logger = logger;
		_gameDb = gameDb;
	}

	[HttpPost]
	public async Task<ShowPlayerItemResponse> Post(ShowPlayerItemRequest request)
	{
		var response = new ShowPlayerItemResponse();
 
		var (errorCode, playerItems) = await _gameDb.PlayerItemTable.SelectListAsync(request.PlayerId);

		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIShowPlayerItemError], 
				new {PlayerId = request.PlayerId, ErrorCode = errorCode}, 
				"PlayerItem Table Select Fail");
			
			response.Result = errorCode;
			return response;
		}

		response.PlayerItems = playerItems;
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIShowPlayerItem], 
			new {PlayerId = request.PlayerId}, "Show Player Item Success");

		return response;
	}
}