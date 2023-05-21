using CloudStructures;
using CloudStructures.Structures;
using Com2usEduProject.Databases.Schema;
using Com2usEduProject.Tools;
using StackExchange.Redis;
using ZLogger;

namespace Com2usEduProject.Databases;

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

	public async Task<ErrorCode> EnterStageAsync(int playerId, PlayerStageInfo stageInfo)
	{
		var keyTimeSpan = TimeSpan.FromMinutes(RedisKeyExpireTime.StageKeyExpireMin);
		try
		{
			var redis= new RedisString<PlayerStageInfo>(_redisConnection, SUID + playerId, keyTimeSpan);
			if (await redis.SetAsync(stageInfo, keyTimeSpan/*, When.NotExists*/) == false)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.EnterStageError],
					new {ErrorCode = ErrorCode.RedisFailException, PlayerId = playerId, StageInfo = stageInfo},
					$"Redis Set Duplicate Key Error");           
				return ErrorCode.RedisSetDuplicateKey;
			}

			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.EnterStageError], e,
				new {ErrorCode = ErrorCode.RedisFailException, PlayerId = playerId, StageInfo = stageInfo},
				$"Redis Set Fail");           
			return ErrorCode.RedisFailException;
		}
	}
	
	public async Task<ErrorCode> UpdatePlayerStageInfoAsync(int playerId, PlayerStageInfo stageInfo)
	{
		var keyTimeSpan = TimeSpan.FromMinutes(RedisKeyExpireTime.StageKeyExpireMin);
		try
		{
			var redis = new RedisString<PlayerStageInfo>(_redisConnection, SUID + playerId, keyTimeSpan);
			if (await redis.SetAsync(stageInfo, keyTimeSpan, When.Exists) == false)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.EnterStageError],
					new {ErrorCode = ErrorCode.RedisFailException, PlayerId = playerId, StageInfo = stageInfo},
					$"Redis Set Key Not Found");           
				return ErrorCode.RedisKeyNotFound;
			}

			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.EnterStageError], e,
				new {ErrorCode = ErrorCode.RedisFailException, PlayerId = playerId, StageInfo = stageInfo},
				$"Redis Set Fail");
			return ErrorCode.RedisFailException;
		}
	}

	public async Task<(ErrorCode, PlayerStageInfo)> GetPlayerStageInfoAsync(int playerId)
	{
		var keyTimeSpan = TimeSpan.FromMinutes(RedisKeyExpireTime.StageKeyExpireMin);

		try
		{
			var redis = new RedisString<PlayerStageInfo>(_redisConnection, SUID + playerId, null);
			var stageInfo = await redis.GetAsync();
			if (!stageInfo.HasValue)
			{
				return (ErrorCode.RedisKeyNotFound, new PlayerStageInfo());
			}
			return (ErrorCode.None, stageInfo.Value);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.EnterStageError], e,
				new {ErrorCode = ErrorCode.RedisFailException, PlayerId = playerId},
				$"Redis Get Fail");           
			return (ErrorCode.RedisFailException, new PlayerStageInfo());
		}
	}
	
	public async Task<(ErrorCode,PlayerStageInfo)> ExitAndGetStageInfoAsync(int playerId)
	{
		var keyTimeSpan = TimeSpan.FromMinutes(RedisKeyExpireTime.StageKeyExpireMin);
		try
		{
			var redis = new RedisString<PlayerStageInfo>(_redisConnection, SUID + playerId, null);
			var stageInfo = await redis.GetAsync();
			await redis.DeleteAsync();
			if (!stageInfo.HasValue)
			{
				return (ErrorCode.RedisKeyNotFound, new PlayerStageInfo());
			}
			return (ErrorCode.None, stageInfo.Value);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.ExitStageError], e,
				new {ErrorCode = ErrorCode.RedisFailException}, $"Redis Get/Delete Fail");           
			return (ErrorCode.RedisFailException, new PlayerStageInfo());
		}
	}

}