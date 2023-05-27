using CloudStructures;
using CloudStructures.Structures;
using Com2usEduAPIServer.Databases.Schema;
using Com2usEduAPIServer.Tools;
using StackExchange.Redis;
using ZLogger;

namespace Com2usEduAPIServer.Databases;

public class StageManager
{
	const string SUID = "StageUID_";
	
	readonly RedisConnection _redisConnection;
	readonly ILogger<RedisDb> _logger;

	class RedisKeyExpireTime
	{
		public const ushort StageKeyExpireMin = 30; 
	}

	public StageManager(RedisConnection redisConnection, ILogger<RedisDb> logger)
	{
		_redisConnection = redisConnection;
		_logger = logger;
	}

	public async Task<ErrorCode> EnterStageAsync(int playerId, PlayerInGameStageInfo inGameStageInfo)
	{
		var keyTimeSpan = TimeSpan.FromMinutes(RedisKeyExpireTime.StageKeyExpireMin);
		try
		{
			var redis= new RedisString<PlayerInGameStageInfo>(_redisConnection, SUID + playerId, keyTimeSpan);
			if (await redis.SetAsync(inGameStageInfo, keyTimeSpan/*, When.NotExists*/) == false)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.EnterStageError],
					new {ErrorCode = ErrorCode.RedisFailException, PlayerId = playerId, StageInfo = inGameStageInfo},
					$"Redis Set Duplicate Key Error");           
				return ErrorCode.RedisSetDuplicateKey;
			}

			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.EnterStageError], e,
				new {ErrorCode = ErrorCode.RedisFailException, PlayerId = playerId, StageInfo = inGameStageInfo},
				$"Redis Set Fail");           
			return ErrorCode.RedisFailException;
		}
	}
	
	public async Task<ErrorCode> UpdatePlayerStageInfoAsync(int playerId, PlayerInGameStageInfo inGameStageInfo)
	{
		var keyTimeSpan = TimeSpan.FromMinutes(RedisKeyExpireTime.StageKeyExpireMin);
		try
		{
			var redis = new RedisString<PlayerInGameStageInfo>(_redisConnection, SUID + playerId, keyTimeSpan);
			if (await redis.SetAsync(inGameStageInfo, keyTimeSpan, When.Exists) == false)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.EnterStageError],
					new {ErrorCode = ErrorCode.RedisFailException, PlayerId = playerId, StageInfo = inGameStageInfo},
					$"Redis Set Key Not Found");           
				return ErrorCode.RedisKeyNotFound;
			}

			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.EnterStageError], e,
				new {ErrorCode = ErrorCode.RedisFailException, PlayerId = playerId, StageInfo = inGameStageInfo},
				$"Redis Set Fail");
			return ErrorCode.RedisFailException;
		}
	}

	public async Task<(ErrorCode, PlayerInGameStageInfo)> GetPlayerStageInfoAsync(int playerId)
	{
		var keyTimeSpan = TimeSpan.FromMinutes(RedisKeyExpireTime.StageKeyExpireMin);

		try
		{
			var redis = new RedisString<PlayerInGameStageInfo>(_redisConnection, SUID + playerId, null);
			var stageInfo = await redis.GetAsync();
			if (!stageInfo.HasValue)
			{
				return (ErrorCode.RedisKeyNotFound, new PlayerInGameStageInfo());
			}
			return (ErrorCode.None, stageInfo.Value);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.EnterStageError], e,
				new {ErrorCode = ErrorCode.RedisFailException, PlayerId = playerId},
				$"Redis Get Fail");           
			return (ErrorCode.RedisFailException, new PlayerInGameStageInfo());
		}
	}
	
	public async Task<(ErrorCode,PlayerInGameStageInfo)> ExitAndGetStageInfoAsync(int playerId)
	{
		var keyTimeSpan = TimeSpan.FromMinutes(RedisKeyExpireTime.StageKeyExpireMin);
		try
		{
			var redis = new RedisString<PlayerInGameStageInfo>(_redisConnection, SUID + playerId, null);
			var stageInfo = await redis.GetAsync();
			await redis.DeleteAsync();
			if (!stageInfo.HasValue)
			{
				return (ErrorCode.RedisKeyNotFound, new PlayerInGameStageInfo());
			}
			return (ErrorCode.None, stageInfo.Value);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.ExitStageError], e,
				new {ErrorCode = ErrorCode.RedisFailException}, $"Redis Get/Delete Fail");           
			return (ErrorCode.RedisFailException, new PlayerInGameStageInfo());
		}
	}

}