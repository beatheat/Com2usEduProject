﻿using Com2usEduAPIServer.Databases;
using Com2usEduAPIServer.ReqRes;
using Com2usEduAPIServer.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoadPlayer : ControllerBase
{
	readonly IGameDb _gameDb;
	readonly ILogger<LoadPlayer> _logger;
	
	public LoadPlayer(ILogger<LoadPlayer> logger, IGameDb gameDb)
	{
		_logger = logger;
		_gameDb = gameDb;
	}

	[HttpPost]
	public async Task<LoadPlayerResponse> Post(LoadPlayerRequest request)
	{
		var response = new LoadPlayerResponse();
 
		var (errorCode, player) = await _gameDb.PlayerTable.SelectAsync(request.PlayerId);

		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Select Player Fail");
			response.Result = errorCode;
			return response;
		}

		response.Player = player;
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APILoadPlayer], 
			new {PlayerId = request.PlayerId}, "Load Player Success");

		return response;
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoadPlayerError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}