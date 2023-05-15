using Com2usEduProject.Databases;
using Com2usEduProject.Databases.Schema;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class FarmStageItem
{
	readonly IMemoryDb _memoryDb;
	readonly ILogger<FarmStageItem> _logger;

	public FarmStageItem(ILogger<FarmStageItem> logger, IMemoryDb memoryDb)
	{
		_logger = logger;
		_memoryDb = memoryDb;
	}


	[HttpPost]
	public async Task<FarmStageItemResponse> Post(FarmStageItemRequest request)
	{
		var response = new FarmStageItemResponse();

		// 플레이어 스테이지 정보 로드
		var (errorCode, playerStageInfo) = await _memoryDb.StageManager.GetPlayerStageInfoAsync(request.PlayerId);
		if(errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Get Player Stage Info Fail");
			response.Result = errorCode;
			return response;
		}

		// 파밍한 아이템이 스테이지에 속한 것인지 검증
		if (playerStageInfo.FarmedStageItemCounts.ContainsKey(request.ItemCode) == false)
		{
			errorCode = ErrorCode.FarmStageItemInvalidItem;
			LogError(errorCode, request, "Invalid Stage Item Request");
			response.Result = errorCode;
			return response;
		}

		// 파밍한 아이템 스테이지 정보에 추가
		playerStageInfo.FarmedStageItemCounts[request.ItemCode]++;
		errorCode = await _memoryDb.StageManager.UpdatePlayerStageInfoAsync(request.PlayerId, playerStageInfo);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Update Player Stage Info Fail");
			response.Result = errorCode;
			return response;
		}

		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIFarmStageItem], 
			new {PlayerId = request.PlayerId}, "Farm Stage Item Success");
		return response;
	}
	
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIFarmStageItemError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}