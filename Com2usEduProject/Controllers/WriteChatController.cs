using Com2usEduProject.Databases;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class WriteChat
{
	readonly IMemoryDb _memoryDb;
	readonly ILogger<LoadMail> _logger;
	
	public WriteChat(ILogger<LoadMail> logger, IMemoryDb memoryDb)
	{
		_logger = logger;
		_memoryDb = memoryDb;
	}
	[HttpPost]
	public async Task<WriteChatResponse> Post(WriteChatRequest request)
	{
		var response = new WriteChatResponse();

		var errorCode = await _memoryDb.ChatManager.WriteChat(request.LobbyNumber, request.PlayerId, request.PlayerNickName, request.Chat);
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