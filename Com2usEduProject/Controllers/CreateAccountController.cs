using Com2usEduProject.DBSchema;
using Com2usEduProject.ReqRes;
using Microsoft.AspNetCore.Mvc;
using Com2usEduProject.Services;
using Com2usEduProject.Tools;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateAccount : ControllerBase
{
	readonly ILogger<CreateAccount> _logger;
	readonly IAccountDb _accountDb;
	readonly IGameDb _gameDb;
	readonly IMasterDb _masterDb;
	
	public CreateAccount(ILogger<CreateAccount> logger, IAccountDb accountDb, IGameDb gameDb, IMasterDb masterDb)
	{
		_logger = logger;
		_accountDb = accountDb;
		_gameDb = gameDb;
		_masterDb = masterDb;
	}
	
	[HttpPost]
	public async Task<CreateAccountRes> Post(CreateAccountReq request)
	{
		var response = new CreateAccountRes();
        
		// 계정 생성
		var (errorCode,accountId) = await _accountDb.CreateAccountAsync(request.Id, request.Password);
		if (errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}

		// 플레이어 기본 데이터 생성
		(errorCode, var playerId) = await _gameDb.CreatePlayerDataAsync(accountId);
		if (errorCode != ErrorCode.None)
		{
			// TODO: 플레이어 데이터 생성 실패 시 계정정보 삭제해서 롤백하는 기능 고려
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.CreateAccount], new {AccountId = accountId}, "Player Data Creation Failed");
			response.Result = errorCode;
			return response;
		}
		
		// 플레이어 기본 아이템 생성
		errorCode = await InsertInitialPlayerItem(playerId);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.CreateAccount], new {AccountId = accountId}, "Player Initial Item Creation Failed");
			response.Result = errorCode;
			return response;
		}

		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.CreateAccount], new {AccountId = accountId}, "CreateAccount Success");
		return response;
	}

	private async Task<ErrorCode> InsertInitialPlayerItem(int playerId)
	{
		var initialPlayerItemList = _masterDb.GetInitialPlayerItem();
		foreach (var item in initialPlayerItemList)
		{
			var (_,itemData) = _masterDb.GetItem(item.ItemCode);
			PlayerItem playerItem = new PlayerItem
			{
				PlayerId = playerId,
				ItemCode = item.ItemCode,
				Count = item.ItemCount,
				Attack = itemData.Attack,
				Defence = itemData.Defence,
				Magic = itemData.Magic,
				EnhanceCount = 0
			};
			var (errorCode, _) = await _gameDb.InsertPlayerItemAsync(playerItem);
			
			if (errorCode != ErrorCode.None)
				return errorCode;
		}
		return ErrorCode.None;
	}
}