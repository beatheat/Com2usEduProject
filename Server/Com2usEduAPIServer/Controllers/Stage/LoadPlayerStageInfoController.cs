﻿using Com2usEduAPIServer.Databases;
using Com2usEduAPIServer.ReqRes;
using Com2usEduAPIServer.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduAPIServer.Controllers;


[ApiController]
[Route("[controller]")]
public class LoadPlayerStageInfo : ControllerBase
{
	readonly IGameDb _gameDb;
	readonly ILogger<LoadPlayerStageInfo> _logger;
	
	const int MAX_STAGE_CODE = 4;

	public LoadPlayerStageInfo(ILogger<LoadPlayerStageInfo> logger, IGameDb gameDb)
	{
		_logger = logger;
		_gameDb = gameDb;
	}

	[HttpPost]
	public async Task<LoadPlayerStageInfoResponse> Post(LoadPlayerStageInfoRequest request)
	{
		var response = new LoadPlayerStageInfoResponse();

		var (errorCode, player) = await _gameDb.PlayerTable.SelectAsync(request.PlayerId, "HighestClearStageCode");
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Select Player Column 'HighestClearStageCode' Fail");
			response.Result = errorCode;
			return response;
		}

		response.ClearStageCodes = Enumerable.Range(1, player.HighestClearStageCode).ToList();
		response.AccessibleStageCodes = Enumerable.Range(1, player.HighestClearStageCode + 1).ToList();
		response.MaxStageCode = MAX_STAGE_CODE;
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APILoadCompletedStageList], 
			new {PlayerId = request.PlayerId}, "Load Completed Stage List Success");
		
		return response;
	}

	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoadCompletedStageListError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}