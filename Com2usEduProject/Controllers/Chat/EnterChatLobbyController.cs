using Com2usEduProject.Databases;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class EnterChatLobby
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

		var (errorCode, chatList) = await _memoryDb.ChatManager.LoadChatHistoryAsync(request.LobbyNumber);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode,request,"Load Chat History Fail");
			response.Result = errorCode;
			return response;
		}
		
		response.ChatHistory = chatList;
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIEnterChatLobby],
			new {PlayerId = request.PlayerId}, "Enter Chat Lobby Success");
		return response;
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIEnterChatLobbyError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}