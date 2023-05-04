using Com2usEduProject.DBSchema;
using Com2usEduProject.Tools;
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
		
			_logger.ZLogDebug($"[InsertPlayerItem] PlayerId: {item.PlayerId} ItemCode {item.ItemCode} Count {item.Count}");

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
		try
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
			
			var playerItemId = await _queryFactory.Query("PlayerItem").InsertGetIdAsync<int>(playerItem);
		
			_logger.ZLogDebug($"[InsertPlayerItem] PlayerId: {playerId} ItemCode {item.Code} Count {count}");

			return (ErrorCode.None, playerItemId);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerItemInsertError], e,
				new {PlayerItem = item, ErrorCode = ErrorCode.PlayerItemInsertFailException}, "Insert PlayerItem Fail");
			return (ErrorCode.PlayerItemInsertFailException, -1);
		}
	}
	
	
	public async Task<(ErrorCode, IList<PlayerItem>)> SelectAsync(int playerId)
	{
		try
		{
			var playerItems = await _queryFactory.Query("PlayerItem").Where("PlayerId", playerId).GetAsync<PlayerItem>();

			_logger.ZLogDebug($"[LoadPlayerItems] PlayerId: {playerId}");
			
			return (ErrorCode.None, playerItems.ToList());
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerItemSelectError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerItemSelectFailException}, "Select PlayerItem Fail");
			return (ErrorCode.PlayerItemSelectFailException, new List<PlayerItem>());
		}
	}

	public async Task<ErrorCode> DeleteAsync(int playerItemId)
	{
		try
		{
			int count = await _queryFactory.Query("PlayerItem").Where("Id", playerItemId).DeleteAsync();
			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerItemDeleteError],
					new {PlayerItemId = playerItemId, ErrorCode = ErrorCode.PlayerItemDeleteFail}, "Delete PlayerItem Fail");
				return ErrorCode.PlayerItemDeleteFail;

			}
			_logger.ZLogDebug($"[DeletePlayerItem] PlayerItemId: {playerItemId}");
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

			_logger.ZLogDebug($"[DeletePlayerItem] PlayerItemId: {playerId}");
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