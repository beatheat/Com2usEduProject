using Com2usEduProject.Databases;
using Com2usEduProject.Databases.Schema;
using Com2usEduProject.GameLogic;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class CompleteStage
{
	readonly IGameDb _gameDb;
	readonly IMemoryDb _memoryDb;
	readonly IMasterDb _masterDb;
	readonly ILogger<CompleteStage> _logger;

	public CompleteStage(ILogger<CompleteStage> logger, IGameDb gameDb, IMemoryDb memoryDb, IMasterDb masterDb)
	{
		_logger = logger;
		_gameDb = gameDb;
		_memoryDb = memoryDb;
		_masterDb = masterDb;
	}

	[HttpPost]
	public async Task<CompleteStageResponse> Post(CompleteStageRequest request)
	{
		var response = new CompleteStageResponse();

		// 플레이어 스테이지 정보 로드
		var (errorCode, stageInfo) = await _memoryDb.StageManager.GetPlayerStageInfoAsync(request.PlayerId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Get Player Stage Info Fail");
			response.Result = errorCode;
			return response;
		}
		
		// 스테이지 클리어 여부를 확인
		if (response.IsStageCleared = CheckStageClear(stageInfo))
		{
			// 스테이지 클리어 시 보상 획득
			errorCode = await InsertStageRewardToPlayer(stageInfo);
			if (errorCode != ErrorCode.None)
			{
				LogError(errorCode, request, "Insert Stage Reward To Player Fail");
				response.Result = errorCode;
				return response;
			}
		}

		// 스테이지 완료 정보 삽입
		errorCode = await _gameDb.PlayerCompletedStageTable.InsertAsync(request.PlayerId, stageInfo.StageCode);
		if (errorCode is not ErrorCode.PlayerCompletedStageInsertDuplicate and not ErrorCode.None)
		{
			LogError(errorCode, request, "Insert Player Completed Stage Fail");
			response.Result = errorCode;
			return response;
		}
		
		// 스테이지 종료
		errorCode = await _memoryDb.StageManager.ExitStageAsync(request.PlayerId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Exit Stage Fail");
			response.Result = errorCode;
			return response;
		}
		

		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APICompleteStage], 
			new {PlayerId = request.PlayerId}, "Complete Stage Success");
		return response;
	}

	private async Task<ErrorCode> InsertStageRewardToPlayer(PlayerStageInfo stageInfo)
	{
		//경험치 획득
		var errorCode = await _gameDb.PlayerTable.UpdateAddColumnAsync(stageInfo.PlayerId, "Exp", CalculateStageExp(stageInfo.StageCode));
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, new{PlayerStageInfo = stageInfo}, "InsertStageRewardToPlayer - Receive Stage Reward Exp To Player Fail");
			return errorCode;
		}
		
		//아이템 획득
		var itemReceiver = new PlayerItemReceiver(_logger, _masterDb, _gameDb);
		errorCode = await itemReceiver.Receive(stageInfo.PlayerId, CreateRewardItemList(stageInfo));
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, new{PlayerStageInfo = stageInfo}, "InsertStageRewardToPlayer - Receive Stage Reward Item To Player Fail");
			await itemReceiver.Rollback(stageInfo.PlayerId);
			return errorCode;
		}

		return ErrorCode.None;
	}

	private List<ItemBundle> CreateRewardItemList(PlayerStageInfo stageInfo)
	{
		var itemBundles = new List<ItemBundle>();
		foreach (var farmedItem in stageInfo.FarmedStageItemCounts)
		{
			var itemCode = farmedItem.Key;
			var itemCount = farmedItem.Value;
			itemBundles.Add(new ItemBundle {ItemCode = itemCode, ItemCount = itemCount});
		}
		return itemBundles;
	}
	
	private int CalculateStageExp(int stageCode)
	{
		var (_, stageNpcs) = _masterDb.GetStageNpc(stageCode);

		int expSum = 0;
		foreach (var npc in stageNpcs)
		{
			expSum += npc.Count * npc.Exp;
		}
		return expSum;
	}
	
	private bool CheckStageClear(PlayerStageInfo stageInfo)
	{
		var (_, stageNpcs) = _masterDb.GetStageNpc(stageInfo.StageCode);
		foreach (var npc in stageNpcs)
		{
			if (stageInfo.FarmedStageNpcCounts[npc.Code] < npc.Count)
			{
				return false;
			}
		}
		return true;
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APICompleteStageError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}