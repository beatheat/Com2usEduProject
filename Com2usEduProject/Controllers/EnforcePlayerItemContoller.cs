using Com2usEduProject.Databases;
using Com2usEduProject.DBSchema;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class EnforcePlayerItem
{
	readonly IGameDb _gameDb;
	readonly IMasterDb _masterDb;
	readonly ILogger<EnforcePlayerItem> _logger;
	
	public EnforcePlayerItem(ILogger<EnforcePlayerItem> logger, IGameDb gameDb, IMasterDb masterDb)
	{
		_logger = logger;
		_gameDb = gameDb;
		_masterDb = masterDb;
	}

	[HttpPost]
	public async Task<EnforcePlayerItemResponse> Post(EnforcePlayerItemRequest request)
	{
		var response = new EnforcePlayerItemResponse();

		// 플레이어 로드
		var (errorCode, player) = await _gameDb.PlayerTable.SelectAsync(request.PlayerId);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIEnforcePlayerItemError], new {ErrorCode = errorCode}, 
				"Select Player Fail");
			response.Result = errorCode;
			return response;
		}
		
		// 플레이어 아이템 로드
		(errorCode,var playerItem) = await _gameDb.PlayerItemTable.SelectAsync(request.PlayerItemId);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIEnforcePlayerItemError], new {ErrorCode = errorCode}, 
				"Select PlayerItem Fail");
			
			response.Result = errorCode;
			return response;
		}

		// 플레이어가 다른 플레이어의 아이템을 강화할 경우
		if (player.Id != playerItem.PlayerId)
		{
			_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIEnforcePlayerItem], 
				new {ErrorCode = errorCode, Player = player, PlayerItem = playerItem}, 
				"Enforce PlayerItem Request From Non Owner Player");
			
			response.Result = ErrorCode.EnforcePlayerItemRequestFromNonOwnerPlayer;
			return response;
		}

		// 아이템 강화
		(errorCode, response.EnforceState) = await EnforceItem(playerItem);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIEnforcePlayerItemError], new {ErrorCode = errorCode}, 
				"Select PlayerItem Fail");
			
			response.Result = errorCode;
			return response;
		}

		response.EnforcedItem = playerItem;

		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIEnforcePlayerItem], 
			new {PlayerId = request.PlayerId, PlayerItemId = request.PlayerItemId, EnforceState = response.EnforceState}, "Enforce Player Item Success");

		return response;
	}

	private async Task<(ErrorCode errorCode, EnforceState EnforceDisable)> EnforceItem(PlayerItem playerItem)
	{
		ErrorCode errorCode = ErrorCode.None;
		Random random = new Random();

		// 아이템의 마스터 데이터를 로드
		(errorCode, var itemMasterData) = _masterDb.GetItem(playerItem.ItemCode);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIEnforcePlayerItemError], new {ErrorCode = errorCode}, 
				"Account Insert Fail");
			
			return (errorCode, EnforceState.Error);
		}

		// 방어구나 무기일 경우만 강화 가능
		if (itemMasterData.Attribute is not (ItemAttribute.ARMOR or ItemAttribute.WEAPON))
		{
			return (errorCode, EnforceState.Disable);
		}
		
		// 강화횟수가 남아 있어야 강화 가능
		if (playerItem.EnhanceCount >= itemMasterData.MaxEnhanceCount)
		{
			return (ErrorCode.None, EnforceState.Disable);
		}
		
		// 강화성공 
		if (random.Next(10) < 3)
		{
			if(itemMasterData.Attribute == ItemAttribute.WEAPON)
				playerItem.Attack = (int)(playerItem.Attack * 1.1);
			if (itemMasterData.Attribute == ItemAttribute.ARMOR)
				playerItem.Defence = (int) (playerItem.Defence * 1.1);

			playerItem.EnhanceCount++;

			errorCode = await _gameDb.PlayerItemTable.UpdateAsync(playerItem);
			if (errorCode != ErrorCode.None)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIEnforcePlayerItemError], new {ErrorCode = errorCode}, 
					"PlayerItem Update Fail");

				return (errorCode, EnforceState.Error);
			}
			
			return (ErrorCode.None, EnforceState.Success);
		}
		
		//강화실패
		errorCode = await _gameDb.PlayerItemTable.DeleteAsync(playerItem.Id);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIEnforcePlayerItemError], new {ErrorCode = errorCode}, 
				"PlayerItem Delete Fail");

			return (errorCode, EnforceState.Error);
		}
		return (ErrorCode.None, EnforceState.Fail);
	}
}