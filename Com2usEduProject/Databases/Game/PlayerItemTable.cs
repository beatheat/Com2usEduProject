using Com2usEduProject.Databases.Schema;
using Com2usEduProject.Tools;
using MySqlConnector;
using SqlKata.Execution;
using ZLogger;

namespace Com2usEduProject.Databases;

public class PlayerItemTable
{
	readonly QueryFactory _queryFactory;
	readonly ILogger<GameDb> _logger;
	
	public PlayerItemTable(QueryFactory queryFactory, ILogger<GameDb> logger)
	{
		_queryFactory = queryFactory;
		_logger = logger;
	}
	
	
	public async Task<(ErrorCode, int)> InsertAsync(PlayerItem item)
	{
		try
		{
			var playerItemId = await _queryFactory.Query("PlayerItem").InsertGetIdAsync<int>(item);
			return (ErrorCode.None, playerItemId);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerItemInsertError], e,
				new {PlayerItem = item, ErrorCode = ErrorCode.PlayerItemDeleteFailException}, "Insert PlayerItem Fail");
			return (ErrorCode.PlayerItemInsertFailException, -1);
		}
	}
	
	public async Task<(ErrorCode, int)> InsertAsync(int playerId, Item item, int count)
	{
		PlayerItem playerItem = new PlayerItem
		{
			PlayerId = playerId,
			ItemCode = item.Code,
			Count = count,
			Attack = item.Attack,
			Defence = item.Defence,
			Magic = item.Magic,
			EnhanceCount = 0
		};

		try
		{
			var playerItemId = await _queryFactory.Query("PlayerItem").InsertGetIdAsync<int>(playerItem);
			return (ErrorCode.None, playerItemId);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerItemInsertError], e,
				new {PlayerItem = item, ErrorCode = ErrorCode.PlayerItemInsertFailException}, "Insert PlayerItem Fail");
			return (ErrorCode.PlayerItemInsertFailException, -1);
		}
	}

	public async Task<(ErrorCode, PlayerItem)> SelectAsync(int playerItemId)
	{
		try
		{
			var playerItem = await _queryFactory.Query("PlayerItem").Where("Id", playerItemId).FirstAsync<PlayerItem>();
			return (ErrorCode.None, playerItem);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerItemSelectError], e,
				new {PlayerItemId = playerItemId, ErrorCode = ErrorCode.PlayerItemSelectFailException}, "Select PlayerItem Fail");
			return (ErrorCode.PlayerItemSelectFailException, new PlayerItem());
		}
	}
	
	public async Task<(ErrorCode, IList<PlayerItem>)> SelectListAsync(int playerId)
	{
		try
		{
			var playerItems = await _queryFactory.Query("PlayerItem").Where("PlayerId", playerId).GetAsync<PlayerItem>();
			return (ErrorCode.None, playerItems.ToList());
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerItemSelectError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerItemSelectFailException}, "Select PlayerItem Fail");
			return (ErrorCode.PlayerItemSelectFailException, new List<PlayerItem>());
		}
	}
	
	public async Task<(ErrorCode, PlayerItem)> SelectByItemCodeAsync(int playerId, int itemCode)
	{
		try
		{
			var query = _queryFactory.Query("PlayerItem").Where("PlayerId", playerId).Where("ItemCode", itemCode);
			
			if (await query.ExistsAsync() == false)
			{
				return (ErrorCode.PlayerItemSelectNotExist, new PlayerItem());
			}
			
			var playerItem = await query.FirstAsync<PlayerItem>();
			
			return (ErrorCode.None, playerItem);		
		}

		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerItemSelectError], e,
				new {ItemCode = itemCode, ErrorCode = ErrorCode.PlayerItemSelectFailException}, "Select PlayerItem Fail");
			return (ErrorCode.PlayerItemSelectFailException, new PlayerItem());
		}
	}
	
	public async Task<ErrorCode> UpdateAsync(PlayerItem playerItem)
	{
		try
		{
			var count = await _queryFactory.Query("PlayerItem").Where("Id", playerItem.Id).UpdateAsync(playerItem);

			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerUpdateError], 
					new {PlayerItem = playerItem, ErrorCode = ErrorCode.PlayerItemUpdateFail}, "Update PlayerItem Fail");
			}
			
			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerItemUpdateError], e,
				new {PlayerItem = playerItem, ErrorCode = ErrorCode.PlayerItemUpdateFailException}, "Update PlayerItem Fail");
			return ErrorCode.PlayerItemUpdateFailException;
		}
	}
	
	public async Task<ErrorCode> DeleteAsync(int playerItemId)
	{
		try
		{
			var count = await _queryFactory.Query("PlayerItem").Where("Id", playerItemId).DeleteAsync();
			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerItemDeleteError],
					new {PlayerItemId = playerItemId, ErrorCode = ErrorCode.PlayerItemDeleteFail}, "Delete PlayerItem Fail");
				return ErrorCode.PlayerItemDeleteFail;

			}
			
			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerItemDeleteError], e,
				new {PlayerItemId = playerItemId, ErrorCode = ErrorCode.PlayerItemDeleteFailException}, "Delete PlayerItem Fail");
			return ErrorCode.PlayerItemDeleteFailException;
		}
	}

	public async Task<ErrorCode> DeleteAllAsync(int playerId)
	{
		try
		{
			await _queryFactory.Query("PlayerItem").Where("PlayerId", playerId).DeleteAsync();
			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerItemDeleteError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerItemDeleteFailException}, "Delete PlayerItem Fail");
			return ErrorCode.PlayerItemDeleteFailException;
		}
	}
	
}