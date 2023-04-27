using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using CloudStructures;
using CloudStructures.Structures;
using Com2usEduProject.Tools;
using ZLogger;


namespace Com2usEduProject.Services;

public class RedisDb : IMemoryDb
{
    RedisConnection _redisConn;

    private static readonly ILogger<RedisDb> s_logger = LogManager.GetLogger<RedisDb>();


    public void Init(string address)
    {
        var config = new RedisConfig("default", address);
        _redisConn = new RedisConnection(config);
        s_logger.ZLogDebug($"userDbAddress:{address}");
    }

    public Task<ErrorCode> RegisterUserAsync(string id, string authToken, long accountId)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorCode> CheckUserAuthAsync(string id, string authToken)
    {
        throw new NotImplementedException();
    }

    //
    // public async Task<ErrorCode> RegistUserAsync(string email, string authToken, Int64 accountId)
    // {
    //     var key = MemoryDbKeyMaker.MakeUIDKey(email);
    //     var result = ErrorCode.None;
    //
    //     var user = new AuthUser
    //     {
    //         Email = email,
    //         AuthToken = authToken,
    //         AccountId = accountId, 
    //         State = UserState.Default.ToString()
    //     };
    //
    //     try
    //     {
    //         var redis = new RedisString<AuthUser>(_redisConn, key, LoginTimeSpan());
    //         if (await redis.SetAsync(user, LoginTimeSpan()) == false)
    //         {
    //             s_logger.ZLogError(EventIdDic[EventType.LoginAddRedis],
    //                 $"Email:{email}, AuthToken:{authToken},ErrorMessage:UserBasicAuth, RedisString set Error");
    //             result = ErrorCode.LoginFailAddRedis;
    //             return result;
    //         }
    //     }
    //     catch
    //     {
    //         s_logger.ZLogError(EventIdDic[EventType.LoginAddRedis],
    //             $"Email:{email},AuthToken:{authToken},ErrorMessage:Redis Connection Error");
    //         result = ErrorCode.LoginFailAddRedis;
    //         return result;
    //     }
    //
    //     return result;
    // }
    //
    // public async Task<ErrorCode> CheckUserAuthAsync(string id, string authToken)
    // {
    //     var key = MemoryDbKeyMaker.MakeUIDKey(id);
    //     var result = ErrorCode.None;
    //
    //     try
    //     {
    //         var redis = new RedisString<AuthUser>(_redisConn, key, null);
    //         var user = await redis.GetAsync();
    //
    //         if (!user.HasValue)
    //         {
    //             s_logger.ZLogError(EventIdDic[EventType.Login],
    //                 $"RedisDb.CheckUserAuthAsync: Email = {id}, AuthToken = {authToken}, ErrorMessage:ID does Not Exist");
    //             result = ErrorCode.CheckAuthFailNotExist;
    //             return result;
    //         }
    //
    //         if (user.Value.Email != id || user.Value.AuthToken != authToken)
    //         {
    //             s_logger.ZLogError(EventIdDic[EventType.Login],
    //                 $"RedisDb.CheckUserAuthAsync: Email = {id}, AuthToken = {authToken}, ErrorMessage = Wrong ID or Auth Token");
    //             result = ErrorCode.CheckAuthFailNotMatch;
    //             return result;
    //         }            
    //     }
    //     catch
    //     {
    //         s_logger.ZLogError(EventIdDic[EventType.Login],
    //             $"RedisDb.CheckUserAuthAsync: Email = {id}, AuthToken = {authToken}, ErrorMessage:Redis Connection Error");
    //         result = ErrorCode.CheckAuthFailException;
    //         return result;
    //     }
    //
    //
    //     return result;
    // }
    //
    // public async Task<bool> SetUserStateAsync(AuthUser user, UserState userState)
    // {
    //     var uid = MemoryDbKeyMaker.MakeUIDKey(user.Email);
    //     try
    //     {
    //         var redis = new RedisString<AuthUser>(_redisConn, uid, null);
    //
    //         user.State = userState.ToString();
    //
    //         if (await redis.SetAsync(user) == false)
    //         {
    //             return false;
    //         }
    //
    //         return true;
    //     }
    //     catch
    //     {
    //         return false;
    //     }
    // }
    //         
    // public async Task<(bool, AuthUser)> GetUserAsync(string id)
    // {
    //     var uid = MemoryDbKeyMaker.MakeUIDKey(id);
    //
    //     try
    //     {
    //         var redis = new RedisString<AuthUser>(_redisConn, uid, null);
    //         var user = await redis.GetAsync();
    //         if (!user.HasValue)
    //         {
    //             s_logger.ZLogError(
    //                 $"RedisDb.UserStartCheckAsync: UID = {uid}, ErrorMessage = Not Assigned User, RedisString get Error");
    //             return (false, null);
    //         }
    //
    //         return (true, user.Value);
    //     }
    //     catch
    //     {
    //         s_logger.ZLogError($"UID:{uid},ErrorMessage:ID does Not Exist");
    //         return (false, null);
    //     }
    // }
    //
    // public async Task<bool> SetUserReqLockAsync(string key)
    // {
    //     try
    //     {
    //         var redis = new RedisString<AuthUser>(_redisConn, key, NxKeyTimeSpan());
    //         if (await redis.SetAsync(new AuthUser
    //         {
    //             // emtpy value
    //         }, NxKeyTimeSpan(), StackExchange.Redis.When.NotExists) == false)
    //         {
    //             return false;
    //         }
    //     }
    //     catch
    //     {
    //         return false;
    //     }
    //
    //     return true;
    // }
    //
    // public async Task<bool> DelUserReqLockAsync(string key)
    // {
    //     if(string.IsNullOrEmpty(key))
    //     {
    //         return false;   
    //     }
    //     
    //     try
    //     {
    //         var redis = new RedisString<AuthUser>(_redisConn, key, null);
    //         var redisResult = await redis.DeleteAsync();
    //         return redisResult;
    //     }
    //     catch
    //     {
    //         return false;
    //     }
    // }
    //     
    //
    // public TimeSpan LoginTimeSpan()
    // {
    //     return TimeSpan.FromMinutes(RediskeyExpireTime.LoginKeyExpireMin);
    // }
    //
    // public TimeSpan TicketKeyTimeSpan()
    // {
    //     return TimeSpan.FromSeconds(RediskeyExpireTime.TicketKeyExpireSecond);
    // }
    //
    // public TimeSpan NxKeyTimeSpan()
    // {
    //     return TimeSpan.FromSeconds(RediskeyExpireTime.NxKeyExpireSecond);
    // }
}