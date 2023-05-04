using Com2usEduProject.Databases;
using Com2usEduProject.DBSchema;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;


[ApiController]
[Route("[controller]")]
public class ReceiveInAppPurchaseItem
{
	readonly IMasterDb _masterDb;
	readonly IGameDb _gameDb;
	readonly ILogger<ReceiveInAppPurchaseItem> _logger;

	public ReceiveInAppPurchaseItem(ILogger<ReceiveInAppPurchaseItem> logger,IGameDb gameDb, IMasterDb masterDb)
	{
		_logger = logger;
		_gameDb = gameDb;
		_masterDb = masterDb;
	}

	public int Test(int a, out int b)
	{
		b = 3;
		return 1;
	}
	
	[HttpPost]
	public async Task<ReceiveInAppPurchaseItemResponse> Post(ReceiveInAppPurchaseItemRequest request)
	{
		var response = new ReceiveInAppPurchaseItemResponse();

		// 중복되는 영수증인지 검증
		var (errorCode, _) = await _gameDb.BillTalble.SelectAsync(request.BillToken);
		if (errorCode == ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveInAppPurchaseItemError], 
				new {PlayerId = request.PlayerId,Token = request.BillToken, ErrorCode = ErrorCode.DuplicatedReceiveInAppPurchaseItemRequest}, 
				"Duplicated Receive InAppPurchase Item Request");
			
			response.Result = ErrorCode.DuplicatedReceiveInAppPurchaseItemRequest;
			return response;
		}

		// 상품 코드로부터 상품아이템 로드
		(errorCode, var shopItems) = _masterDb.GetShopItem(request.ShopCode);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveInAppPurchaseItemError], 
				new {PlayerId = request.PlayerId, ShopCode = request.ShopCode, ErrorCode = errorCode}, 
				"Unknown Shop Item Code");
			
			response.Result = errorCode;
			return response;
		}

		// 상품코드를 통해 아이템을  플레이어 아이템 테이블에 삽입
		var insertedPlayerItemIds = new List<int>();
		foreach (var shopItem in shopItems)
		{
			(errorCode, var playerItemId) = await InsertShopItem(request.PlayerId, shopItem);
			
			if (errorCode != ErrorCode.None)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveInAppPurchaseItemError], 
					new
					{
						PlayerId = request.PlayerId, ShopCode = request.ShopCode, ErrorCode = errorCode, 
						ErrorInsertedPlayerItemIds = insertedPlayerItemIds,
					}, 
					"Insert Player Item Fail");
				
				await Rollback(insertedPlayerItemIds);
				response.Result = errorCode;
				return response;
			}
			
			insertedPlayerItemIds.Add(playerItemId);
		}
		
		// 이미 사용한 영수증 등록
		errorCode = await _gameDb.BillTalble.InsertAsync(new Bill {PlayerId = request.PlayerId, Token = request.BillToken});
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveInAppPurchaseItemError], 
				new {PlayerId = request.PlayerId,Token = request.BillToken, ShopCode = request.ShopCode, ErrorCode = errorCode}, 
				"Insert Bill Fail");
			
			response.Result = errorCode;
			return response;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceReward], 
			new {PlayerId = request.PlayerId, ShopCode = request.ShopCode},
			"Receive InAppPurchase Item Success");
		
		return response;
	}
	
	
	private async Task<(ErrorCode, int)> InsertShopItem(int playerId, ShopItem shopItem)
	{

		var errorCode = ErrorCode.None;

		(errorCode, var item) = _masterDb.GetItem(shopItem.ItemCode);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveInAppPurchaseItemError], 
				new
				{
					PlayerId = playerId, ItemCode = shopItem.ItemCode, ErrorCode = errorCode
				}, 
				"Unknown Item Code");

			return (errorCode, -1);
		}
			
		(errorCode, var playerItemId) = await _gameDb.PlayerItemTable.InsertAsync(playerId, item, shopItem.ItemCount);
		return (errorCode, playerItemId);
	}

	
	public async Task Rollback(IList<int> errorInsertedItemIds)
	{
		foreach (var errorItemId in errorInsertedItemIds)
		{
			var errorCode = await _gameDb.PlayerItemTable.DeleteAsync(errorItemId);
			if (errorCode != ErrorCode.None)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveInAppPurchaseItemError], 
					new {PlayerItemId = errorItemId, ErrorCode = errorCode}, 
					"Rollback - Delete Player Item Failed");
			}
		}
	}
}