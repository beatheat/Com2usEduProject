using Com2usEduAPIServer.Databases;
using Com2usEduAPIServer.ReqRes;
using Com2usEduAPIServer.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduAPIServer.Controllers;


[ApiController]
[Route("[controller]")]
public class ExitChatLobby : ControllerBase
{
	readonly IMemoryDb _memoryDb;
	readonly ILogger<ExitChatLobby> _logger;


	public ExitChatLobby(ILogger<ExitChatLobby> logger, IMemoryDb memoryDb)
	{
		_logger = logger;
		_memoryDb = memoryDb;
	}

	[HttpPost]
	public async Task<ExitChatLobbyResponse> Post(ExitChatLobbyRequest request)
	{
		var response = new ExitChatLobbyResponse();

		var errorCode = await _memoryDb.ChatManager.ExitLobby(request.PlayerId);
		if (errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIExitChatLobby],
			new {PlayerId = request.PlayerId}, "Exit Chat Lobby Success");
		return response;
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIExitChatLobbyError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}