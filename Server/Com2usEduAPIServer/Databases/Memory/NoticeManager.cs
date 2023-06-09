﻿using CloudStructures;
using CloudStructures.Structures;
using Com2usEduAPIServer.Tools;
using ZLogger;

namespace Com2usEduAPIServer.Databases;

public class NoticeManager
{
	const string NOTICE = "Notice";
	
	readonly RedisConnection _redisConnection;
	readonly ILogger<RedisDb> _logger;

	public NoticeManager(RedisConnection redisConnection, ILogger<RedisDb> logger)
	{
		_redisConnection = redisConnection;
		_logger = logger;
	}

	public async Task<(bool,string)> GetNoticeAsync()
	{
		try
		{
			var redis = new RedisString<string>(_redisConnection, NOTICE, null);
			var notice = await redis.GetAsync();
            
			return (notice.HasValue, notice.Value);
		}
		catch(Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GetNoticeError], e,
				new {ErrorCode = ErrorCode.RedisFailException}, "Get Redis String Failed");           
			return (false, "");
		}
	}
}