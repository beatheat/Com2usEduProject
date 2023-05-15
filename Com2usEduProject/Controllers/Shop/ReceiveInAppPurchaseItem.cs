using Com2usEduProject.Databases;
using Com2usEduProject.Databases.Schema;
using Com2usEduProject.Databases.Schema.Extension;
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
		
		// 사용한 영수증 등록 - 영수증을 Primary Key로 이용하고 있어 중복 영수증 등록 검증함
		var (errorCode, _) = await _gameDb.BillTable.InsertAsync(new Bill {PlayerId = request.PlayerId, Token = request.BillToken});
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Insert Bill Fail");
			response.Result = errorCode;
			return response;
		}
		
		// 상점에서 구입한 상품을 플레이어 우편함에 추가
		(errorCode, var mailId) = await InsertShopItemToMail(request.PlayerId, request.ShopCode);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Insert ShopItem To Mail Fail");
			response.Result = errorCode;
			return response;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceReward], 
			new {PlayerId = request.PlayerId, ShopCode = request.ShopCode},
			"Receive InAppPurchase Item Success");
		
		return response;
	}
	
	
	private async Task<(ErrorCode, int)> InsertShopItemToMail(int playerId, int shopCode)
	{
		var errorCode = ErrorCode.None;

		// 상품 코드로부터 상품아이템 로드
		(errorCode, var shopItems) = _masterDb.GetShopItem(shopCode);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, new {ShopCode = shopCode}, "InsertShopItemToMail - Get Shop Item Fail");
			return (errorCode, -1);
		}

		//아이템이 포함된 메일 발송
		var mail = new Mail
		{
			PlayerId = playerId, 
			Content = $"상품({shopCode}) 가 전달되었습니다!", 
			Name = $"상품({shopCode})",
			ExpireDate = DateTime.Now + TimeSpan.FromDays(3650),
			TransmissionDate = DateTime.Now,
			IsItemReceived = false,
		};

		foreach (var item in shopItems)
		{
			mail.AddItem(item.ItemCode,item.ItemCount);
		}
		
		(errorCode,var mailId) = await _gameDb.MailTable.InsertAsync(mail);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode,new{Mail = mail}, "InsertShopItemToMail - Insert Mail Failed");
			return (errorCode, -1);
		}

		return (errorCode, mailId);
	}

	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveInAppPurchaseItemError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
	
}