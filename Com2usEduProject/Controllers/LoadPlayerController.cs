using Com2usEduProject.Databases;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class LoadPlayer
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
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoadPlayerError], 
				new {PlayerId = request.PlayerId, ErrorCode = errorCode}, 
				"Player Table Select Fail");
			
			response.Result = errorCode;
			return response;
		}

		response.Player = player;
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APILoadPlayer], 
			new {PlayerId = request.PlayerId}, "Load Player Success");

		return response;
	}
}