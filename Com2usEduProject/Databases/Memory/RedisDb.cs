using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using CloudStructures;
using CloudStructures.Structures;
using Com2usEduProject.DBSchema;
using Com2usEduProject.ModelDB;
using Com2usEduProject.Authorization;
using Com2usEduProject.Tools;
using Humanizer;
using ZLogger;


namespace Com2usEduProject.Databases;

public class RedisDb : IMemoryDb
{

    const string UID = "UID_";
    const string ULOCK = "ULOCK_";
    const string NOTICE = "Notice";

    public class RedisKeyExpireTime
    {
        public const ushort KeyExpireSecond = 3;
        public const ushort RegisterKeyExpireSecond = 6000;
        public const ushort LoginKeyExpireMin = 60; 
        public const ushort TicketKeyExpireSecond = 6000; 
    }

    
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
        //TODO: 로그인 키 갱신 필요
        var loginTimeSpan = TimeSpan.FromMinutes(RedisKeyExpireTime.LoginKeyExpireMin);
        
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
                s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.RegisterUserError],
                    new {AuthUser= user, ErrorCode = ErrorCode.RedisSetDuplicateKey}, "Set Redis Key Already Exists");
                return ErrorCode.RedisSetDuplicateKey;
            }
        }
        catch (Exception e)
        {
            s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.RegisterUserError], e, 
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
                s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GetUserError],
                    new {RedisKey = uid, ErrorCode = ErrorCode.RedisKeyNotFound}, "Get Redis Key Not Exist");
                return (ErrorCode.RedisKeyNotFound, new AuthUser());
            }

            return (ErrorCode.None, user.Value);
        }
        catch(Exception e)
        {
            s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GetUserError], e,
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
            var redis = new RedisString<AuthUser>(_redisConn, lockId, keyTimeSpan);
            if (await redis.SetAsync(null, keyTimeSpan, StackExchange.Redis.When.NotExists) == false)
            {
                s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.SetUserRequestLockError],
                    new {RedisKey = lockId, ErrorCode = ErrorCode.RedisSetDuplicateKey}, "Set Redis Key Already Exists");  
                return false;
            }
        }
        catch (Exception e)
        {
            s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.SetUserRequestLockError], e,
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
            s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.DelUserRequestLockError], e,
                new {RedisKey = lockId, ErrorCode = ErrorCode.RedisFailException}, "Delete Redis String Failed");      
            return false;
        }
    }

    
    public async Task<(bool,string)> GetNoticeAsync()
    {
        try
        {
            var redis = new RedisString<string>(_redisConn, NOTICE, null);
            var notice = await redis.GetAsync();
            
            return (notice.HasValue, notice.Value);
        }
        catch(Exception e)
        {
            s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GetNoticeError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, "Get Redis String Failed");           
            return (false, "");
        }
    }
}