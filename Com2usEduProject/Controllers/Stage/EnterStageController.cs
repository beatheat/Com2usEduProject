using Com2usEduProject.Databases;
using Com2usEduProject.Databases.Schema;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;



[ApiController]
[Route("[controller]")]
public class EnterStage
{
	readonly IMemoryDb _memoryDb;
	readonly IMasterDb _masterDb;
	readonly ILogger<EnterStage> _logger;

	const int MAX_STAGE_CODE = 4;
	
	public EnterStage(ILogger<EnterStage> logger, IMemoryDb memoryDb, IMasterDb masterDb)
	{
		_logger = logger;
		_memoryDb = memoryDb;
		_masterDb = masterDb;
	}

	[HttpPost]
	public async Task<EnterStageResponse> Post(EnterStageRequest request)
	{
		var response = new EnterStageResponse();
		ErrorCode errorCode = ErrorCode.None;

		// 스테이지 코드 유효성 검증
		if (request.StageCode is < 1 or > MAX_STAGE_CODE)
		{
			errorCode = ErrorCode.UnknownStageCode;
			LogError(errorCode, request, "Unknown Stage Code");
			response.Result = errorCode;
			return response;
		}
		
		// 플레이어의 스테이지 정보 생성
		var stageInfo = CreatePlayerStageInfo(request.PlayerId, request.StageCode, out var stageItems, out var stageNpcs);
		
		// 스테이지 진입 - 이미 스테이지에 입장중이면 요청 무시
		errorCode = await _memoryDb.StageManager.EnterStageAsync(request.PlayerId, stageInfo);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode,request, "Enter Stage Fail");
			response.Result = errorCode;
			return response;
		}
		
		response.StageItems = stageItems;
		response.StageNpcs = stageNpcs;
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIEnterStagee], 
			new {PlayerId = request.PlayerId}, "EnterStage Success");
		return response;
	}

	private PlayerStageInfo CreatePlayerStageInfo(int playerId, int stageCode, out List<StageItem> stageItems, out List<StageNpc> stageNpcs)
	{
		(_, stageItems) = _masterDb.GetStageItem(stageCode);
		(_, stageNpcs) = _masterDb.GetStageNpc(stageCode);
		
		var stageInfo = new PlayerStageInfo {PlayerId = playerId, StageCode = stageCode};

		foreach (var item in stageItems)
		{
			stageInfo.FarmedStageItemCounts.Add(item.ItemCode, 0);
		}

		foreach (var npc in stageNpcs)
		{
			stageInfo.FarmedStageNpcCounts.Add(npc.Code, 0);
		}

		return stageInfo;
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIEnterStageError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}