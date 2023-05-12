using Com2usEduProject.DBSchema;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Databases;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceiveMailItemController
{
	readonly IMasterDb _masterDb;
	readonly IGameDb _gameDb;
	readonly ILogger<ReceiveMailItemController> _logger;

	class RollbackData
	{
		public enum ProcessType{MONEY,UPDATE,INSERT};
		public ProcessType Type { get; set; }
		public int PlayerItemId { get; set; }
		public int Count { get; set; }
		public PlayerItem PlayerItem { get; set; }
	}
	
	public ReceiveMailItemController(ILogger<ReceiveMailItemController> logger,IGameDb gameDb, IMasterDb masterDb)
	{
		_logger = logger;
		_gameDb = gameDb;
		_masterDb = masterDb;
	}

	[HttpPost]
	public async Task<ReceiveMailItemResponse> Post(ReceiveMailItemRequest request)
	{
		var response = new ReceiveMailItemResponse();

		// 메일 로드
		var (errorCode, mail) = await _gameDb.MailTable.SelectAsync(request.MailId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Select Mail Fail");
			response.Result = errorCode;
			return response;
		}

		// 메일 아이템을 이미 받았다면 요청 무시
		if (mail.IsItemReceived)
		{
			errorCode = ErrorCode.ReceiveMailItemAlready;
			LogError(errorCode, request, "Mail Item Already Received");
			response.Result = errorCode;
			return response;
		}

		// 요청자와 메일의 소유주가 다르다면 요청 무시
		if (mail.PlayerId != request.PlayerId)
		{
			errorCode = ErrorCode.ReceiveMailItemRequestFromNonOwnerPlayer;
			LogError(errorCode, request, "Receive MailItem Request From Non Owner Player");
			response.Result = errorCode;
			return response;
		}


		// 메일 아이템을 플레이어 아이템에 삽입
		(errorCode,var rollbackData) = await InsertMailItemToPlayer(mail);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Insert Mail Item To Player Item Fail");
			await Rollback(mail.PlayerId,rollbackData);
			response.Result = errorCode;
			return response;
		}

		// 메일 아이템을 받음으로 체크
		errorCode = await _gameDb.MailTable.UpdateItemReceivedToTrue(mail.Id);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Insert Mail Item To Player Fail");
			await Rollback(mail.PlayerId,rollbackData);
			response.Result = errorCode;
			return response;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIReceiveMailItem], new {PlayerId = request.PlayerId, Mail = mail}, "ReceiveMail Success");
		return response;
	}

	private async Task<(ErrorCode,IList<RollbackData>)> InsertMailItemToPlayer(Mail mail)
	{
		//메일 아이템 정보 로드
		var errorCode = ErrorCode.None;
		List<RollbackData> rollbackData = new List<RollbackData>();

		foreach (var mailItem in mail.GetItemList())
		{
			if (mailItem.ItemCode == -1)
				break;
			// 아이템 정보 로드
			(errorCode, var item) = _masterDb.GetItem(mailItem.ItemCode);
			if (errorCode != ErrorCode.None)
			{
				LogError(errorCode, new {MailItem = mailItem}, "InsertMailItemToPlayer - Unknown Item Code");
				return (errorCode,rollbackData);
			}

			// 아이템이 돈이라면 플레이어 정보 수정
			if (item.Attribute == ItemAttribute.MONEY)
			{
				errorCode = await _gameDb.PlayerTable.UpdateAddMoneyAsync(mail.PlayerId, mailItem.ItemCount);
				if (errorCode != ErrorCode.None)
				{
					LogError(errorCode, new {MailItem = mailItem}, "InsertMailItemToPlayer - Update Player Add Money Fail");
					return (errorCode,rollbackData);
				}
				rollbackData.Add(new RollbackData{Count = mailItem.ItemCount,Type = RollbackData.ProcessType.MONEY});
				continue;
			}
			//아이템이 소비아이템이고 같은 종류의 아이템이 존재 한다면 중첩해서 저장함
			else if(item.Consumable)
			{
				(errorCode, var playerItem) = await _gameDb.PlayerItemTable.SelectByItemCodeAsync(item.Code);
				if (errorCode == ErrorCode.None)
				{
					playerItem.Count += mailItem.ItemCount;
					errorCode = await _gameDb.PlayerItemTable.UpdateAsync(playerItem);
					rollbackData.Add(new RollbackData{PlayerItem = playerItem, Count = mailItem.ItemCount, Type = RollbackData.ProcessType.UPDATE});
					continue;
				}
				else if (errorCode != ErrorCode.PlayerItemSelectNotExist)
				{
					LogError(errorCode, new {MailItem = mailItem}, "InsertMailItemToPlayer - Select PlayerItem By ItemCode Fail");
					return (errorCode,rollbackData);
				}
			}
			//아이템이 장비아이템이거나 처음 얻게 된 소비아이템이라면 플레이어 아이템에 새롭게 추가함
			(errorCode, var  playerItemId) = await _gameDb.PlayerItemTable.InsertAsync(mail.PlayerId, item, mailItem.ItemCount);
			if (errorCode != ErrorCode.None)
			{
				LogError(errorCode, new {MailItem = mailItem}, "InsertMailItemToPlayer - Insert PlayerItem Fail");
				return (errorCode, rollbackData);
			}
			rollbackData.Add(new RollbackData{PlayerItemId = playerItemId, Count = mailItem.ItemCount, Type = RollbackData.ProcessType.UPDATE});
		}
		
		return (errorCode, rollbackData);
	}

	private async Task Rollback(int playerId, IList<RollbackData> rollbackData)
	{
		var errorCode = ErrorCode.None;
		foreach (var data in rollbackData)
		{
			switch (data.Type)
			{
				case RollbackData.ProcessType.MONEY:
					errorCode = await _gameDb.PlayerTable.UpdateAddMoneyAsync(playerId, -data.Count);
					break;
				case RollbackData.ProcessType.INSERT:
					errorCode = await _gameDb.PlayerItemTable.DeleteAsync(data.PlayerItemId);
					break;
				case RollbackData.ProcessType.UPDATE:
					data.PlayerItem.Count -= data.Count;
					errorCode = await _gameDb.PlayerItemTable.UpdateAsync(data.PlayerItem);
					break;
				default: 
					LogError(errorCode, new {RollbackData = data}, "Rollback - Unknown Rollback Process Type");
					return;
			}
			if (errorCode != ErrorCode.None)
			{
				LogError(errorCode, new {RollbackData = data}, "Rollback - Rollback Fail");
			}

		}
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveInAppPurchaseItemError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}