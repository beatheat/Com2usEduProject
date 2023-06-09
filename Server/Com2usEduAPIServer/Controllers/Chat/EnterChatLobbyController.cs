﻿using Com2usEduAPIServer.Databases;
using Com2usEduAPIServer.ReqRes;
using Com2usEduAPIServer.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class EnterChatLobby : ControllerBase
{
	readonly IMemoryDb _memoryDb;
	readonly ILogger<EnterChatLobby> _logger;
	
	public EnterChatLobby(ILogger<EnterChatLobby> logger, IMemoryDb memoryDb)
	{
		_logger = logger;
		_memoryDb = memoryDb;
	}

	[HttpPost]
	public async Task<EnterChatLobbyResponse> Post(EnterChatLobbyRequest request)
	{
		var response = new EnterChatLobbyResponse();

		var errorCode = ErrorCode.None;

		int lobbyNumber = request.LobbyNumber;
		if (lobbyNumber == -1)
		{
			(errorCode,lobbyNumber) = await _memoryDb.ChatManager.GetRecommendLobbyNumber();
			if (errorCode != ErrorCode.None)
			{
				LogError(errorCode, request, "Every Lobby is Full");
				response.Result = errorCode;
				return response;
			}
		}

		errorCode = await _memoryDb.ChatManager.ExitLobbyIfExist(request.PlayerId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode,request,"ExitLobby If Exist Fail");
			response.Result = errorCode;
			return response;
		}
		
		errorCode = await _memoryDb.ChatManager.EnterLobby(request.PlayerId, lobbyNumber);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode,request,"Enter Lobby Fail");
			response.Result = errorCode;
			return response;
		}
		
		(errorCode, var chatHistory) = await _memoryDb.ChatManager.LoadChatHistoryAsync(lobbyNumber);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode,request,"Load Chat History Fail");
			response.Result = errorCode;
			return response;
		}
		
		response.ChatHistory = chatHistory;
		response.LobbyNumber = lobbyNumber;
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIEnterChatLobby],
			new {PlayerId = request.PlayerId , LobbyNumber = request.LobbyNumber}, "Enter Chat Lobby Success");
		return response;
	}

	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIEnterChatLobbyError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}