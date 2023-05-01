﻿using System.Data;
using Com2usEduProject.DBSchema;
using Com2usEduProject.ModelDB;
using Com2usEduProject.Tools;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;
using ZLogger;
using Exception = System.Exception;

namespace Com2usEduProject.Services;

public class GameDb : IGameDb
{
	readonly IOptions<DbConnectionConfig> _dbConfig;
	readonly ILogger<AccountDb> _logger;
	readonly QueryFactory _queryFactory;

	IDbConnection _dbConnection;

	public GameDb(ILogger<AccountDb> logger, IOptions<DbConnectionConfig> dbConfig)
	{
		_dbConfig = dbConfig;
		_logger = logger;

		Open();
		
		_queryFactory = new QueryFactory(_dbConnection, new MySqlCompiler());
	}
	
	public void Dispose()
	{
		Close();
	}

	private void Open()
	{
		try
		{
			_dbConnection = new MySqlConnection(_dbConfig.Value.GameDb);
			_dbConnection.Open();
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GameDbConnection], e,
				new {ConnectionString = _dbConfig.Value.GameDb}, "GameDb Connection Failed");
		}
	}
	
	public void Close()
	{
		_dbConnection.Close();
	}

	public async Task<(ErrorCode, int)> CreatePlayerDataAsync(int accountId)
	{
		try
		{
			var playerId = await _queryFactory.Query("Player").InsertGetIdAsync<int>(new
			{
				AccountId = accountId, 
				ContinuousAttendanceDays = 0, 
				LastAttendanceDate = DateTime.Today,
			});
			
			_logger.ZLogDebug($"[CreatePlayerData] PlayerId: {playerId} AccountId {accountId}");

			return (ErrorCode.None, playerId);
		}
		catch  (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.CreatePlayerData], e,
				new {AccountId = accountId, ErrorCode = ErrorCode.PlayerDataInsertFailException}, "Insert PlayerData Failed");
			return (ErrorCode.PlayerDataInsertFailException, -1);
		}
	}

	public async Task<(ErrorCode, Player)> LoadPlayerDataAsync(int accountId)
	{
		try
		{
			var playerData = await _queryFactory.Query("Player").Where("AccountId", accountId).FirstAsync<Player>();
			_logger.ZLogDebug($"[LoadPlayerData] Id: {accountId}");
			
			return (ErrorCode.None, playerData);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadPlayerData], e,
				new {AccountId = accountId, ErrorCode = ErrorCode.PlayerDataSelectFailException}, "Select PlayerData Failed");		
			return (ErrorCode.CreateAccountFailException, new Player());
		}
	}

	
	public async Task<(ErrorCode, int)> InsertPlayerItemAsync(PlayerItem item)
	{
		try
		{
			var playerItemUniqueNo = await _queryFactory.Query("PlayerItem").InsertGetIdAsync<int>(new
			{
				PlayerId = item.PlayerId,
				ItemCode = item.ItemCode,
				Attack = item.Attack,
				Defence = item.Defence,
				Magic = item.Magic,
				EnhanceCount = item.EnhanceCount,
				Count = item.Count
			});
		
			_logger.ZLogDebug($"[InsertPlayerItem] PlayerId: {item.PlayerId} ItemCode {item.ItemCode} Count {item.Count}");

			return (ErrorCode.None, playerItemUniqueNo);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.InsertPlayerItem], e,
				new {PlayerItem = item, ErrorCode = ErrorCode.PlayerDataInsertFailException}, "Insert PlayerItem Failed");
			return (ErrorCode.PlayerItemInsertFailException, -1);
		}

	}
	
	
	public async Task<(ErrorCode, IList<PlayerItem>)> LoadPlayerItemsAsync(int playerId)
	{
		try
		{
			var playerItems = await _queryFactory.Query("PlayerItem").Where("PlayerId", playerId).GetAsync<PlayerItem>();

			_logger.ZLogDebug($"[LoadPlayerItems] PlayerId: {playerId}");
			
			return (ErrorCode.None, playerItems.ToList());
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadPlayerItems], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerItemSelectFailException}, "Select PlayerItem Failed");
			return (ErrorCode.PlayerItemSelectFailException, new List<PlayerItem>());
		}
	}

}