using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using CloudStructures;
using CloudStructures.Structures;
using Com2usEduAPIServer.Tools;
using Com2usEduAPIServer.Databases.Schema;
using Humanizer;
using StackExchange.Redis;
using ZLogger;


namespace Com2usEduAPIServer.Databases;

public class RedisDb : IMemoryDb
{
    RedisConnection _redisConnection;
    static readonly ILogger<RedisDb> s_logger = LogManager.GetLogger<RedisDb>();

    public AuthManager AuthManager { get; private set; }
    public NoticeManager NoticeManager { get; private set; }
    public ChatManager ChatManager { get; private set; }
    public StageManager StageManager { get; private set; }
    
    public void Init(string address)
    {
        var config = new RedisConfig("default", address);
        _redisConnection = new RedisConnection(config);
        s_logger.ZLogDebug($"userDbAddress:{address}");

        AuthManager = new AuthManager(_redisConnection,s_logger);
        NoticeManager = new NoticeManager(_redisConnection, s_logger);
        ChatManager = new ChatManager(_redisConnection, s_logger);
        StageManager = new StageManager(_redisConnection, s_logger);
    }
}