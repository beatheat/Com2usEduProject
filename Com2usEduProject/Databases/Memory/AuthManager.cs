using CloudStructures;
using CloudStructures.Structures;
using Com2usEduProject.Authorization;
using Com2usEduProject.Tools;
using SqlKata.Execution;
using ZLogger;

namespace Com2usEduProject.Databases;

public class AuthManager
{
    const string UID = "UID_";
    const string ULOCK = "ULOCK_";
    
    
    readonly RedisConnection _redisConnection;
    readonly ILogger<RedisDb> _logger;
    
    public class RedisKeyExpireTime
    {
        public const ushort KeyExpireSecond = 3;
        public const ushort RegisterKeyExpireSecond = 6000;
        public const ushort LoginKeyExpireMin = 60; 
        public const ushort TicketKeyExpireSecond = 6000; 
    }


    public AuthManager(RedisConnection redisConnection, ILogger<RedisDb> logger)
    {
        _redisConnection = redisConnection;
        _logger = logger;
    }
    
    public async Task<ErrorCode> RegisterUserAsync(int accountId, string authToken, int playerId)
    {
        var uid = UID + accountId;
        //TODO: 로그인 키 갱신 필요
        var loginTimeSpan = TimeSpan.FromMinutes(RedisKeyExpireTime.LoginKeyExpireMin);
        
        var user = new AuthUser
        {
            AccountId = accountId,
            AuthToken = authToken,
            PlayerId = playerId
        };
        
        try
        {
            var redis = new RedisString<AuthUser>(_redisConnection, uid, loginTimeSpan);
            if (await redis.SetAsync(user, loginTimeSpan) == false)
            {
                _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.RegisterUserError],
                    new {AuthUser= user, ErrorCode = ErrorCode.RedisSetDuplicateKey}, "Set Redis Key Already Exists");
                return ErrorCode.RedisSetDuplicateKey;
            }
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.RegisterUserError], e, 
                new {AuthUser = user, ErrorCode = ErrorCode.RedisFailException}, "Set Redis String Failed With Exception");
            return ErrorCode.RedisFailException;
        }
        
        return ErrorCode.None;    
    }

    public async Task<(ErrorCode, AuthUser)> GetUserAsync(int accountId)
    {
        var uid = UID + accountId;

        try
        {
            var redis = new RedisString<AuthUser>(_redisConnection, uid, null);
            var user = await redis.GetAsync();
            if (!user.HasValue)
            {
                _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GetUserError],
                    new {RedisKey = uid, ErrorCode = ErrorCode.RedisKeyNotFound}, "Get Redis Key Not Exist");
                return (ErrorCode.RedisKeyNotFound, new AuthUser());
            }

            return (ErrorCode.None, user.Value);
        }
        catch(Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GetUserError], e,
                new {RedisKey = uid, ErrorCode = ErrorCode.RedisFailException}, "Get Redis String Failed");           
            return (ErrorCode.RedisFailException, new AuthUser());
        }
    }

    
    public async Task<bool> SetUserRequestLockAsync(string lockName)
    {
        var lockId = ULOCK + lockName;
        var keyTimeSpan = TimeSpan.FromSeconds(RedisKeyExpireTime.KeyExpireSecond);
        
        try
        {
            var redis = new RedisString<AuthUser>(_redisConnection, lockId, keyTimeSpan);
            if (await redis.SetAsync(null, keyTimeSpan, StackExchange.Redis.When.NotExists) == false)
            {
                _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.SetUserRequestLockError],
                    new {RedisKey = lockId, ErrorCode = ErrorCode.RedisSetDuplicateKey}, "Set Redis Key Already Exists");  
                return false;
            }
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.SetUserRequestLockError], e,
                new {RedisKey = lockId, ErrorCode = ErrorCode.RedisFailException}, "Set Redis String Failed");  
            return false;
        }

        return true;
    }

    public async Task<bool> DelUserRequestLockAsync(string lockName)
    {
        if(string.IsNullOrEmpty(lockName))
        {
            return false;   
        }
        
        var lockId = ULOCK + lockName;

        try
        {
            var redis = new RedisString<AuthUser>(_redisConnection, lockId, null);
            var redisResult = await redis.DeleteAsync();
            return redisResult;
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.DelUserRequestLockError], e,
                new {RedisKey = lockId, ErrorCode = ErrorCode.RedisFailException}, "Delete Redis String Failed");      
            return false;
        }
    }
}