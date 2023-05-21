using Com2usEduProject.Databases;
using Com2usEduProject.Databases.Schema;
using Com2usEduProject.Tools;
using ZLogger;

namespace Com2usEduProject.GameLogic;

public class PlayerItemReceiver
{
	class RollbackData
	{
		public enum ProcessType{MONEY,UPDATE,INSERT};
		public ProcessType Type { get; set; }
		public int PlayerItemId { get; set; }
		public int Count { get; set; }
		public PlayerItem PlayerItem { get; set; }
	}

	readonly IMasterDb _masterDb;
	readonly IGameDb _gameDb;
	readonly ILogger _logger;

	readonly List<RollbackData> _rollbackData;
	
	public PlayerItemReceiver(ILogger logger, IMasterDb masterDb, IGameDb gameDb)
	{
		_logger = logger;
		_masterDb = masterDb;
		_gameDb = gameDb;

		_rollbackData = new List<RollbackData>();
	}

	public async Task<ErrorCode> Receive(int playerId, IEnumerable<ItemBundle> items)
	{
		return await Receive(playerId, items.ToArray());
	}
	
	public async Task<ErrorCode> Receive(int playerId, params ItemBundle[] items)
	{
		var errorCode = ErrorCode.None;
		_rollbackData.Clear();
		
		foreach (var itemBundle in items)
		{
			if (itemBundle.ItemCode == -1)
				break;
			// 아이템 정보 로드
			(errorCode, var itemInfo) = _masterDb.GetItem(itemBundle.ItemCode);
			if (errorCode != ErrorCode.None)
			{
				LogError(errorCode, new {MailItem = itemInfo}, "Receive - Unknown Item Code");
				return errorCode;
			}

			// 아이템이 돈이라면 플레이어 정보 수정
			if (itemInfo.Attribute == ItemAttribute.MONEY)
			{
				errorCode = await _gameDb.PlayerTable.UpdateAddColumnAsync(playerId, "Money", itemBundle.ItemCount);
				if (errorCode != ErrorCode.None)
				{
					LogError(errorCode, new {MailItem = itemInfo}, "Receive - Update Player Add Money Fail");
					return errorCode;
				}
				_rollbackData.Add(new RollbackData{Count = itemBundle.ItemCount,Type = RollbackData.ProcessType.MONEY});
				continue;
			}
			//아이템이 소비아이템이고 같은 종류의 아이템이 존재 한다면 중첩해서 저장함
			else if(itemInfo.Consumable)
			{
				(errorCode, var playerItem) = await _gameDb.PlayerItemTable.SelectByItemCodeAsync(itemInfo.Code);
				if (errorCode == ErrorCode.None)
				{
					playerItem.Count += itemBundle.ItemCount;
					errorCode = await _gameDb.PlayerItemTable.UpdateAsync(playerItem);
					_rollbackData.Add(new RollbackData{PlayerItem = playerItem, Count = itemBundle.ItemCount, Type = RollbackData.ProcessType.UPDATE});
					continue;
				}
				else if (errorCode != ErrorCode.PlayerItemSelectNotExist)
				{
					LogError(errorCode, new {MailItem = itemInfo}, "Receive - Select PlayerItem By ItemCode Fail");
					return errorCode;
				}
			}

			for (int i = 0; i < itemBundle.ItemCount; i++)
			{
				//아이템이 장비아이템이거나 처음 얻게 된 소비아이템이라면 플레이어 아이템에 새롭게 추가함
				(errorCode, var playerItemId) = await _gameDb.PlayerItemTable.InsertAsync(playerId, itemInfo, 1);
				if (errorCode != ErrorCode.None)
				{
					LogError(errorCode, new {MailItem = itemInfo}, "Receive - Insert PlayerItem Fail");
					return errorCode;
				}
				_rollbackData.Add(new RollbackData{PlayerItemId = playerItemId, Count = 1, Type = RollbackData.ProcessType.UPDATE});
			}
		}

		return ErrorCode.None;
	}
	
	public async Task Rollback(int playerId)
	{
		var errorCode = ErrorCode.None;
		foreach (var data in _rollbackData)
		{
			switch (data.Type)
			{
				case RollbackData.ProcessType.MONEY:
					errorCode = await _gameDb.PlayerTable.UpdateAddColumnAsync(playerId, "Money", -data.Count);
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
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerItemReceiverError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}