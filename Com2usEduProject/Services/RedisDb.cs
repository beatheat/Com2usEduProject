using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using CloudStructures;
using CloudStructures.Structures;
using Com2usEduProject.DBSchema;
using Com2usEduProject.ModelDB;
using Com2usEduProject.Tools;
using Humanizer;
using ZLogger;


namespace Com2usEduProject.Services;

public class RedisDb : IMemoryDb
{

    const string UID = "UID_";
    const string ULOCK = "ULOCK_";
    RedisConnection _redisConn;

    static readonly ILogger<RedisDb> s_logger = LogManager.GetLogger<RedisDb>();

    public void Init(string address)
    {
        var config = new RedisConfig("default", address);
        _redisConn = new RedisConnection(config);
        s_logger.ZLogDebug($"userDbAddress:{address}");
    }
    
    public async Task<ErrorCode> RegisterUserAsync(string id, string authToken, int accountId)
    {
        var uid = UID + id;
        var loginTimeSpan = TimeSpan.FromMinutes(RediskeyExpireTime.LoginKeyExpireMin);
        
        var user = new AuthUser
        {
            Id = id,
            AuthToken = authToken,
            AccountId = accountId, 
        };
        
        try
        {
            var redis = new RedisString<AuthUser>(_redisConn, uid, loginTimeSpan);
            if (await redis.SetAsync(user, loginTimeSpan) == false)
            {
                s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.RegisterUser],
                    new {AuthUser= user, ErrorCode = ErrorCode.RedisSetDuplicateKey}, "Set Redis Key Already Exists");
                return ErrorCode.RedisSetDuplicateKey;
            }
        }
        catch (Exception e)
        {
            s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.RegisterUser], e, 
                new {AuthUser = user, ErrorCode = ErrorCode.RedisFailException}, "Set Redis String Failed With Exception");
            return ErrorCode.RedisFailException;
        }
        
        return ErrorCode.None;    
    }

    public async Task<(ErrorCode, AuthUser)> GetUserAsync(string id)
    {
        var uid = UID + id;

        try
        {
            var redis = new RedisString<AuthUser>(_redisConn, uid, null);
            var user = await redis.GetAsync();
            if (!user.HasValue)
            {
                s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GetUser],
                    new {RedisKey = uid, ErrorCode = ErrorCode.RedisKeyNotFound}, "Get Redis Key Not Exist");
                return (ErrorCode.RedisKeyNotFound, new AuthUser());
            }

            return (ErrorCode.None, user.Value);
        }
        catch(Exception e)
        {
            s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GetUser], e,
                new {RedisKey = uid, ErrorCode = ErrorCode.RedisFailException}, "Get Redis String Failed");           
            return (ErrorCode.RedisFailException, new AuthUser());
        }
    }

    
    public async Task<bool> SetUserRequestLockAsync(string lockName)
    {
        var lockId = ULOCK + lockName;
        var keyTimeSpan = TimeSpan.FromSeconds(RediskeyExpireTime.NxKeyExpireSecond);
        
        try
        {
            var redis = new RedisString<AuthUser>(_redisConn, lockId, keyTimeSpan);
            if (await redis.SetAsync(null, keyTimeSpan, StackExchange.Redis.When.NotExists) == false)
            {
                s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.SetUserRequestLock],
                    new {RedisKey = lockId, ErrorCode = ErrorCode.RedisSetDuplicateKey}, "Set Redis Key Already Exists");  
                return false;
            }
        }
        catch (Exception e)
        {
            s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.SetUserRequestLock], e,
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
            var redis = new RedisString<AuthUser>(_redisConn, lockId, null);
            var redisResult = await redis.DeleteAsync();
            return redisResult;
        }
        catch (Exception e)
        {
            s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.DelUserRequestLock], e,
                new {RedisKey = lockId, ErrorCode = ErrorCode.RedisFailException}, "Delete Redis String Failed");      
            return false;
        }
    }
    
}