using Com2usEduProject.Databases;
using Com2usEduProject.Databases.Schema;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class FarmStageNpc
{
	readonly IMemoryDb _memoryDb;
	readonly ILogger<FarmStageNpc> _logger;

	
	public FarmStageNpc(ILogger<FarmStageNpc> logger, IMemoryDb memoryDb)
	{
		_logger = logger;
		_memoryDb = memoryDb;
	}

	[HttpPost]
	public async Task<FarmStageNpcResponse> Post(FarmStageNpcRequest request)
	{
		var response = new FarmStageNpcResponse();

		// 플레이어 스테이지 정보 로드
		var (errorCode, playerStageInfo) = await _memoryDb.StageManager.GetPlayerStageInfoAsync(request.PlayerId);
		if(errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Get Player Stage Info Fail");
			response.Result = errorCode;
			return response;
		}
		
		// 파밍한 NPC가 스테이지에 속하는 지 검증
		if (playerStageInfo.FarmedStageNpcCounts.ContainsKey(request.NpcCode) == false)
		{
			errorCode = ErrorCode.FarmStageItemInvalidItem;
			LogError(errorCode, request, "Invalid Stage Npc Request");
			response.Result = errorCode;
			return response;
		}

		// 파밍한 NPC 스테이지 정보에 추가
		playerStageInfo.FarmedStageNpcCounts[request.NpcCode]++;
		errorCode = await _memoryDb.StageManager.UpdatePlayerStageInfoAsync(request.PlayerId, playerStageInfo);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Update Player Stage Info Fail");
			response.Result = errorCode;
			return response;
		}

		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIFarmStageNPC], 
			new {PlayerId = request.PlayerId}, "Farm Stage Npc Success");
		return response;	
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIFarmStageNPCError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}