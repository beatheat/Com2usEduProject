using System.Linq.Expressions;
using Com2usEduProject.Databases.Schema;
using Com2usEduProject.Tools;
using SqlKata;
using SqlKata.Execution;
using ZLogger;

namespace Com2usEduProject.Databases;

public class PlayerTable
{
	readonly QueryFactory _queryFactory;
	readonly ILogger<GameDb> _logger;
	
	public PlayerTable(QueryFactory queryFactory, ILogger<GameDb> logger)
	{
		_queryFactory = queryFactory;
		_logger = logger;
	}
	
	public async Task<(ErrorCode, int)> InsertAsync(int accountId, string nickname)
	{
		try
		{
			var playerId = await _queryFactory.Query("Player").InsertGetIdAsync<int>(new
			{
				AccountId = accountId,
				Nickname = nickname,
			});
			return (ErrorCode.None, playerId);
		}
		catch  (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerCreateAndInsertError], e,
				new {AccountId = accountId, ErrorCode = ErrorCode.PlayerInsertFailException}, "Insert Player Fail");
			return (ErrorCode.PlayerInsertFailException, -1);
		}
	}

	public async Task<(ErrorCode, int)> SelectIdByAccountIdAsync(int accountId)
	{
		try
		{
			var playerId = await _queryFactory.Query("Player").Select("Id").Where("AccountId", accountId).FirstAsync<int>();
			return (ErrorCode.None, playerId);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerSelectByAccountIdError], e,
				new {AccountId = accountId, ErrorCode = ErrorCode.PlayerSelectFailException}, "Select Player Fail");		
			return (ErrorCode.PlayerSelectFailException, -1);
		}
	}

	public async Task<ErrorCode> UpdateAddColumnAsync(int playerId, string column, int amount)
	{
		try
		{
			var playerData = await _queryFactory.StatementAsync($"UPDATE Player SET {column} = {column} + {amount} WHERE Id = {playerId}");
			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerUpdateError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerUpdateFailException}, "Update Player Fail");		
			return ErrorCode.PlayerUpdateFailException;
		}
	}

	public async Task<(ErrorCode, Player)> SelectAsync(int playerId)
	{
		try
		{
			var playerData = await _queryFactory.Query("Player").Where("Id", playerId).FirstAsync<Player>();
			return (ErrorCode.None, playerData);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerSelectError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerSelectFailException}, "Select Player Fail");		
			return (ErrorCode.PlayerSelectFailException, new Player());
		}
	}

	public async Task<(ErrorCode, Player)> SelectAsync(int playerId, params string[] columns)
	{
		try
		{
			var playerData = await _queryFactory.Query("Player").Select(columns).Where("Id", playerId).FirstAsync<Player>();
			return (ErrorCode.None, playerData);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerSelectError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerSelectFailException}, "Select Player Fail");		
			return (ErrorCode.PlayerSelectFailException, new Player());
		}
	}

	public async Task<ErrorCode> DeleteAsync(int playerId)
	{
		try
		{
			var count = await _queryFactory.Query("Player").Where("Id", playerId).DeleteAsync();

			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerDeleteError], 
					new {AccountId = playerId, ErrorCode = ErrorCode.PlayerDeleteFail}, "Delete Player Fail");
				return ErrorCode.PlayerDeleteFail;
			}
			
			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerDeleteError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerDeleteFailException}, "Delete Player Fail");		
			return ErrorCode.PlayerDeleteFailException;
		}	
	}

	public async Task<ErrorCode> UpdateAsync(Player player)
	{
		try
		{
			var count = await _queryFactory.Query("Player").Where("Id", player.Id).UpdateAsync(player);

			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerUpdateError], 
					new {Player = player, ErrorCode = ErrorCode.PlayerUpdateFail}, "Update Player Fail");
				return ErrorCode.PlayerUpdateFail;
			}
			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerUpdateError], e,
				new {Player = player, ErrorCode = ErrorCode.PlayerUpdateFailException}, "Update Player Fail");		
			return ErrorCode.PlayerUpdateFailException;
		}	
	}
	
	public async Task<ErrorCode> UpdateAsync<T>(int playerId, string column, T value)
	{
		var updateObject = new Dictionary<string, object> {[column] = value};
		try
		{
			var count = await _queryFactory.Query("Player").Where("Id", playerId).UpdateAsync(updateObject);

			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerUpdateError], 
					new {PlayerId = playerId, Column = column, UpdateValue = value,
						ErrorCode = ErrorCode.PlayerUpdateFail}, "Update Player Fail");
				return ErrorCode.PlayerUpdateFail;
			}
			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerUpdateError], e,
				new {PlayerId = playerId, Column = column, UpdateValue = value,
					ErrorCode = ErrorCode.PlayerUpdateFailException}, "Update Player Fail");		
			return ErrorCode.PlayerUpdateFailException;
		}	
	}

}