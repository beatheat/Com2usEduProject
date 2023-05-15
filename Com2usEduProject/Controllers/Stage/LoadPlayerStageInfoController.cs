using Com2usEduProject.Databases;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;


[ApiController]
[Route("[controller]")]
public class LoadPlayerStageInfo
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
		var (errorCode, completedStages) = await _gameDb.PlayerCompletedStageTable.SelectListAsync(request.PlayerId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Select Player Completed Stage Fail");
			response.Result = errorCode;
			return response;
		}

		response.AccessibleStages = GetAccessibleStageList(completedStages);
		response.CompletedStages = completedStages;
		response.MaxStageCode = MAX_STAGE_CODE;
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APILoadCompletedStageList], 
			new {PlayerId = request.PlayerId}, "Load Completed Stage List Success");
		
		return response;
	}

	private IList<int> GetAccessibleStageList(IList<int> completedStages)
	{
		List<int> accessibleStages = new List<int>();
		int latestClearStage = 0;
		if (completedStages.Count > 0)
			latestClearStage = completedStages.Max();
		for (int i = 1; i <= latestClearStage + 1; i++)
		{
			accessibleStages.Add(i);
		}
		return accessibleStages;
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoadCompletedStageListError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}