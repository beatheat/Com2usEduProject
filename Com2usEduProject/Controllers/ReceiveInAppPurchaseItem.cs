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
	
	[HttpPost]
	public async Task<ReceiveInAppPurchaseItemResponse> Post(ReceiveInAppPurchaseItemRequest request)
	{
		var response = new ReceiveInAppPurchaseItemResponse();

		// 중복되는 영수증인지 검증
		var (errorCode, _) = await _gameDb.BillTable.SelectAsync(request.BillToken);
		if (errorCode == ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveInAppPurchaseItemError], 
				new {PlayerId = request.PlayerId,Token = request.BillToken, ErrorCode = ErrorCode.DuplicatedReceiveInAppPurchaseItemRequest}, 
				"Duplicated Receive InAppPurchase Item Request");
			
			response.Result = ErrorCode.DuplicatedReceiveInAppPurchaseItemRequest;
			return response;
		}
		
		// 이미 사용한 영수증 등록
		(errorCode, var billId) = await _gameDb.BillTable.InsertAsync(new Bill {PlayerId = request.PlayerId, Token = request.BillToken});
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveInAppPurchaseItemError], 
				new {PlayerId = request.PlayerId,Token = request.BillToken, ShopCode = request.ShopCode, ErrorCode = errorCode}, 
				"Insert Bill Fail");
			
			response.Result = errorCode;
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
		(errorCode, var mailId) = await InsertShopItemMail(request.PlayerId,request.ShopCode, shopItems);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveInAppPurchaseItemError], 
				new
				{
					PlayerId = request.PlayerId, ShopCode = request.ShopCode, ErrorCode = errorCode, 
				}, 
				"Insert Shop Item Mail Fail");
				
			await Rollback(mailId);
			response.Result = errorCode;
			return response;
		}

		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceReward], 
			new {PlayerId = request.PlayerId, ShopCode = request.ShopCode},
			"Receive InAppPurchase Item Success");
		
		return response;
	}
	
	
	private async Task<(ErrorCode, int)> InsertShopItemMail(int playerId, int shopCode, IList<ShopItem> shopItems)
	{
		var errorCode = ErrorCode.None;
		var mail = new Mail
		{
			PlayerId = playerId, 
			Content = $"상품({shopCode}) 가 전달되었습니다!", 
			Name = $"상품({shopCode})",
			ExpireDate = DateTime.Now + TimeSpan.FromDays(3650),
			TransmissionDate = DateTime.Now,
			IsItemReceived = false,
		};

		(errorCode,var mailId) = await _gameDb.MailTable.InsertAsync(mail);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveInAppPurchaseItemError], 
				new {ErrorCode = errorCode, Mail  = mail}, "InsertShopItemMail - Insert Mail Failed");
			return (errorCode, -1);
		}

		foreach (var item in shopItems)
		{
			var mailItem = new MailItem
			{
				ItemCode = item.ItemCode,
				ItemCount = item.ItemCount,
				MailId = mailId
			};

			(errorCode, _) = await _gameDb.MailItemTable.InsertAsync(mailItem);
			if (errorCode != ErrorCode.None)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveInAppPurchaseItemError], 
					new {ErrorCode = errorCode, Mail  = mail}, "InsertShopItemMail - Insert MailItem Failed");
			}
		}
		
		return (errorCode, mailId);
	}

	
	public async Task Rollback(int mailId)
	{
		var errorCode = await _gameDb.MailTable.DeleteAsync(mailId);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceRewardError], 
				new {ErrorCode = errorCode, mailId = mailId}, "Rollback - Delete Mail Failed");
		}
		
		errorCode = await _gameDb.MailItemTable.DeleteAllAsync(mailId);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceRewardError], 
				new {ErrorCode = errorCode, mailId = mailId}, "Rollback - Delete MailItem Failed");
		}
	}
	
}