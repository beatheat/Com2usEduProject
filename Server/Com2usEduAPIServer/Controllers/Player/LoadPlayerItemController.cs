﻿using Com2usEduAPIServer.Databases;
using Com2usEduAPIServer.ReqRes;
using Com2usEduAPIServer.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoadPlayerItem : ControllerBase
{
	readonly IGameDb _gameDb;
	readonly ILogger<LoadPlayerItem> _logger;
	
	public LoadPlayerItem(ILogger<LoadPlayerItem> logger, IGameDb gameDb)
	{
		_logger = logger;
		_gameDb = gameDb;
	}

	[HttpPost]
	public async Task<LoadPlayerItemResponse> Post(LoadPlayerItemRequest request)
	{
		var response = new LoadPlayerItemResponse();
 
		var (errorCode, playerItems) = await _gameDb.PlayerItemTable.SelectListAsync(request.PlayerId);

		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Select PlayerItem Fail");
			response.Result = errorCode;
			return response;
		}

		response.PlayerItems = playerItems;
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APILoadPlayerItem], 
			new {PlayerId = request.PlayerId}, "Load Player Item Success");

		return response;
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoadPlayerItemError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}