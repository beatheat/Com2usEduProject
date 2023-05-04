﻿using Com2usEduProject.DBSchema;
using Com2usEduProject.Tools;
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
	
	public async Task<(ErrorCode, int)> CreateAndInsertAsync(int accountId)
	{
		try
		{
			var playerId = await _queryFactory.Query("Player").InsertGetIdAsync<int>(new Player
			{
				AccountId = accountId,
			});
			_logger.ZLogDebug($"[CreateAndInsertAsync] AccountId: {accountId}");

			
			return (ErrorCode.None, playerId);
		}
		catch  (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerCreateAndInsertError], e,
				new {AccountId = accountId, ErrorCode = ErrorCode.PlayerInsertFailException}, "Insert PlayerData Failed");
			return (ErrorCode.PlayerInsertFailException, -1);
		}
	}

	public async Task<(ErrorCode, Player)> SelectByAccountIdAsync(int accountId)
	{
		try
		{
			var playerData = await _queryFactory.Query("Player").Where("AccountId", accountId).FirstAsync<Player>();
			_logger.ZLogDebug($"[SelectByAccountIdAsync] AccountId: {accountId}");
			
			return (ErrorCode.None, playerData);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerSelectByAccountIdError], e,
				new {AccountId = accountId, ErrorCode = ErrorCode.PlayerSelectFailException}, "Select PlayerData Failed");		
			return (ErrorCode.PlayerSelectFailException, new Player());
		}
	}

	public async Task<(ErrorCode, Player)> SelectAsync(int playerId)
	{
		try
		{
			var playerData = await _queryFactory.Query("Player").Where("PlayerId", playerId).FirstAsync<Player>();
			_logger.ZLogDebug($"[SelectAsync] PlayerId: {playerId}");
			
			return (ErrorCode.None, playerData);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerSelectError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerSelectFailException}, "Select PlayerData Failed");		
			return (ErrorCode.PlayerSelectFailException, new Player());
		}
	}

	public async Task<ErrorCode> DeleteAsync(int playerId)
	{
		try
		{
			var count = await _queryFactory.Query("Player").Where("PlayerId", playerId).DeleteAsync();

			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerDeleteError], 
					new {AccountId = playerId, ErrorCode = ErrorCode.PlayerDeleteFail}, "Delete Player Failed");
				return ErrorCode.PlayerDeleteFail;
			}
			
			_logger.ZLogDebug($"[DeleteAsync] PlayerId: {playerId}");

			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerDeleteError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerDeleteFailException}, "Delete Player Failed");		
			return ErrorCode.PlayerDeleteFailException;
		}	
	}

	public async Task<ErrorCode> UpdateAsync(Player player)
	{
		try
		{
			var count = await _queryFactory.Query("Player").Where("PlayerId", player.Id).UpdateAsync(player);
			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerUpdateError], 
					new {Player = player, ErrorCode = ErrorCode.PlayerUpdateFail}, "Update Player Failed");
				return ErrorCode.PlayerUpdateFail;
			}
			_logger.ZLogDebug($"[UpdateAsync] PlayerId: {player.Id}");

			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerUpdateError], e,
				new {Player = player, ErrorCode = ErrorCode.PlayerUpdateFailException}, "Update PlayerData Failed");		
			return ErrorCode.PlayerUpdateFailException;
		}	
	}

}