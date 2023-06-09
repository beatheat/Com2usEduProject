﻿using Com2usEduAPIServer.Databases;
using Com2usEduAPIServer.ReqRes;
using Com2usEduAPIServer.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class WriteChat : ControllerBase
{
	readonly IMemoryDb _memoryDb;
	readonly ILogger<WriteChat> _logger;
	
	public WriteChat(ILogger<WriteChat> logger, IMemoryDb memoryDb)
	{
		_logger = logger;
		_memoryDb = memoryDb;
	}
	
	[HttpPost]
	public async Task<WriteChatResponse> Post(WriteChatRequest request)
	{
		var response = new WriteChatResponse();

		var errorCode = await _memoryDb.ChatManager.ValidateChatUserAsync(request.PlayerId, request.LobbyNumber);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Invalid Chat User");
			response.Result = errorCode;
			return response;
		}
		
		errorCode = await _memoryDb.ChatManager.WriteChatAsync(request.PlayerId, request.LobbyNumber, request.PlayerNickName, request.Chat);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode,request,"Write Chat Fail");
			response.Result = errorCode;
			return response;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIWriteChat],
			new {PlayerId = request.PlayerId}, "Write Chat Success");
		return response;
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIWriteChatError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
	
}