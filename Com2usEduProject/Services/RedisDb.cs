using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using CloudStructures;
using CloudStructures.Structures;
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

    public void LoadMasterData(IMasterDb masterDb)
    {
        masterDb.
    }

    public async Task<ErrorCode> RegisterUserAsync(string id, string authToken, long accountId)
    {
        var uid = UID + id;
        var result = ErrorCode.None;
        var loginTimeSpan = TimeSpan.FromMinutes(RediskeyExpireTime.LoginKeyExpireMin);
        
        var user = new AuthUser
        {
            Id = id,
            AuthToken = authToken,
            AccountId = accountId, 
            State = UserState.Default.ToString()
        };
        
        try
        {
            var redis = new RedisString<AuthUser>(_redisConn, uid, loginTimeSpan);
            if (await redis.SetAsync(user, loginTimeSpan) == false)
            {
                s_logger.ZLogError($"[RedisDb.RegisterUserAsync] ErrorCode: {ErrorCode.LoginFailAddRedis} Id:{id}, AuthToken:{authToken}");
                result = ErrorCode.LoginFailAddRedis;
                return result;
            }
        }
        catch
        {
            s_logger.ZLogError($"[RedisDb.RegisterUserAsync] ErrorCode: {ErrorCode.RedisFailException} Id:{id}, AuthToken:{authToken}");
            result = ErrorCode.RedisFailException;
            return result;
        }
        
        return result;    
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
                s_logger.ZLogError($"[RedisDb.GetUserAsync] ErrorCode: {ErrorCode.RedisKeyNotFound} Id = {id}, ErrorMessage = Not Assigned User, RedisString get Error");
                return (ErrorCode.RedisKeyNotFound, null);
            }

            return (ErrorCode.None, user.Value);
        }
        catch
        {
            s_logger.ZLogError($"[RedisDb.GetUserAsync] ErrorCode: {ErrorCode.RedisFailException} Id = {id}");
            return (ErrorCode.RedisFailException, null);
        }
    }

    public async Task<(ErrorCode, string)> GetLatestMasterDataVersionAsync()
    {
        try
        {
            var redis = new RedisString<string>(_redisConn, "MasterDataVersion", null);
            var version = await redis.GetAsync();
            if (!version.HasValue)
            {
                s_logger.ZLogError($"[RedisDb.GetLatestMasterDataVersionAsync] ErrorCode: {ErrorCode.RedisKeyNotFound} , ErrorMessage = Cannot Find MasterDataVersion, RedisString get Error");
                return (ErrorCode.RedisKeyNotFound, null);
            }

            return (ErrorCode.None, version.Value);
        }
        catch (Exception e)
        {
            s_logger.ZLogError($"[RedisDb.GetLatestMasterDataVersionAsync] ErrorCode: {ErrorCode.RedisFailException}");
            return (ErrorCode.RedisFailException, null);
        }
    }

    public async Task<(ErrorCode, string)> GetLatestClientVersionAsync()
    {
        try
        {
            var redis = new RedisString<string>(_redisConn, "ClientVersion", null);
            var version = await redis.GetAsync();
            if (!version.HasValue)
            {
                s_logger.ZLogError($"[RedisDb.GetLatestClientVersionAsync] ErrorCode: {ErrorCode.RedisKeyNotFound} , ErrorMessage = Cannot Find ClientVersion, RedisString get Error");
                return (ErrorCode.RedisKeyNotFound, null);
            }

            return (ErrorCode.None, version.Value);
        }
        catch (Exception e)
        {
            s_logger.ZLogError($"[RedisDb.GetLatestClientVersionAsync] ErrorCode: {ErrorCode.RedisFailException}");
            return (ErrorCode.RedisFailException, null);
        }
    }
    
    public async Task<bool> SetUserReqLockAsync(string lockName)
    {
        var lockId = ULOCK + lockName;
        var keyTimeSpan = TimeSpan.FromSeconds(RediskeyExpireTime.NxKeyExpireSecond);
        
        try
        {
            var redis = new RedisString<AuthUser>(_redisConn, lockId, keyTimeSpan);
            if (await redis.SetAsync(null, keyTimeSpan, StackExchange.Redis.When.NotExists) == false)
            {
                return false;
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<bool> DelUserReqLockAsync(string lockName)
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
        catch
        {
            return false;
        }
    }

    
}