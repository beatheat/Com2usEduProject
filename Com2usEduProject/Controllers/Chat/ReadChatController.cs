using Com2usEduProject.Databases;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ReadChat
{
	readonly IMemoryDb _memoryDb;
	readonly ILogger<ReadChat> _logger;
	
	public ReadChat(ILogger<ReadChat> logger, IMemoryDb memoryDb)
	{
		_logger = logger;
		_memoryDb = memoryDb;
	}

	[HttpPost]
	public async Task<ReadChatResponse> Post(ReadChatRequest request)
	{
		var response = new ReadChatResponse();
	
		var errorCode = await _memoryDb.ChatManager.ValidateChatUserAsync(request.LobbyNumber, request.PlayerId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Invalidate Chat User");
			response.Result = errorCode;
			return response;
		}
		
		(errorCode, var chatList) = await _memoryDb.ChatManager.LoadChatHistoryFromIndexAsync(request.LobbyNumber, request.LastChatIndex+1);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode,request,"Load Chat From Index Fail");
			response.Result = errorCode;
			return response;
		}
		
		response.Chats = chatList;
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIReadChat],
			new {PlayerId = request.PlayerId}, "Read Chat Success");
		return response;
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReadChatError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}