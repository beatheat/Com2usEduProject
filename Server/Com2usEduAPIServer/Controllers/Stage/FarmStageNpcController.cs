using Com2usEduAPIServer.Databases;
using Com2usEduAPIServer.Databases.Schema;
using Com2usEduAPIServer.ReqRes;
using Com2usEduAPIServer.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class FarmStageNpc : ControllerBase
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
		var (errorCode, stageInfo) = await _memoryDb.StageManager.GetPlayerStageInfoAsync(request.PlayerId);
		if(errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Get Player Stage Info Fail");
			response.Result = errorCode;
			return response;
		}
		
		// 파밍한 NPC가 스테이지에 속하는 지 검증
		if (ValidateStageNpc(request.NpcCode, stageInfo) == false)
		{
			errorCode = ErrorCode.FarmStageItemInvalidItem;
			LogError(errorCode, request, "Invalid Stage Npc Request");
			response.Result = errorCode;
			return response;
		}

		// 파밍한 NPC 스테이지 정보에 추가
		stageInfo.FarmedStageNpcCounts[request.NpcCode]++;
		errorCode = await _memoryDb.StageManager.UpdatePlayerStageInfoAsync(request.PlayerId, stageInfo);
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
	
	private bool ValidateStageNpc(int npcCode, PlayerStageInfo stageInfo)
	{
		if (stageInfo.FarmedStageNpcCounts.TryGetValue(npcCode, out var farmedNpcCount))
		{
			if (farmedNpcCount + 1 <= stageInfo.MaxAvailableStageNpcCounts[npcCode])
			{
				return true;
			}
		}
		return false;
	}

	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIFarmStageNPCError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}