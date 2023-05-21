using System.Diagnostics;
using CloudStructures;
using CloudStructures.Structures;
using Com2usEduProject.Databases.Schema;
using Com2usEduProject.Tools;
using StackExchange.Redis;
using ZLogger;

namespace Com2usEduProject.Databases;


/*
 * 채팅 매니저
 * 1. 로비 100명이 넘어가도 접근할 수 있음
 * 2. 유저 EXPIRE되면 LOBBY COUNT 줄이는거랑 ChatUser 만료되면 lobbyCOunt 줄이기
 */
public class ChatManager
{
	readonly RedisConnection _redisConnection;
	readonly ILogger<RedisDb> _logger;

    class RedisKeyExpireTime
    {
        public const ushort ChatLobbyEnterLockExpireSec = 10;
        public const ushort ChatUserExpireMin = 30;
    }
    class RedisKeyPrefix
    {
        public const string ChatUser = "ChatUser_";
        public const string ChatLobbyLog = "ChatLobbyLog_";
        public const string ChatLobbyUserCounts = "ChatLobbyUserCounts";
        public const string ChatLobbyEnterLock = "ChatLobbyEnterLock";
    }
    class ChatConfig
    {
        public const int ChatHistorySize = 50;
        public const int MaxLobbyNum = 100;
        public const int MaxLobbyUserNum = 100;
    }

    public ChatManager(RedisConnection redisConnection, ILogger<RedisDb> logger)
	{
		_redisConnection = redisConnection;
		_logger = logger;
	}

    private static bool ValidateChatLobbyNum(int lobbyNumber)
    {
        return lobbyNumber is < 1 or > ChatConfig.MaxLobbyNum;
    }
    
    public async Task<(ErrorCode, Chat[])> LoadChatHistoryAsync(int lobbyNumber)
    {
        var redisKey = RedisKeyPrefix.ChatLobbyLog + lobbyNumber;
        if (ValidateChatLobbyNum(lobbyNumber))
        {
            return (ErrorCode.ChatLobbyOutOfIndex, Array.Empty<Chat>());
        }
        try
        {
            var redis = new RedisList<Chat>(_redisConnection, redisKey, null);
            var chatList = await redis.RangeAsync(-ChatConfig.ChatHistorySize, -1);
            return (ErrorCode.None, chatList);
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadChatHistoryError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis Range {redisKey} Fail");           
            return (ErrorCode.RedisFailException, Array.Empty<Chat>());
        }
    }
    
    public async Task<(ErrorCode, Chat[])> LoadChatHistoryFromIndexAsync(int lobbyNumber, long index)
    {
        var redisKey = RedisKeyPrefix.ChatLobbyLog + lobbyNumber;
        if (ValidateChatLobbyNum(lobbyNumber))
        {
            return (ErrorCode.ChatLobbyOutOfIndex, Array.Empty<Chat>());
        }
        try
        {
            var redis = new RedisList<Chat>(_redisConnection, redisKey, null);
            var maxChatIndex = await redis.LengthAsync();
            if(maxChatIndex - index > ChatConfig.ChatHistorySize)
            {
                index = maxChatIndex - ChatConfig.ChatHistorySize;
            }
            var chatList = await redis.RangeAsync(index, -1);
            return (ErrorCode.None, chatList);
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadChatFromIndexError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis Range {redisKey} Fail");
            return (ErrorCode.RedisFailException, Array.Empty<Chat>());
        }
    }

    public async Task<ErrorCode> SetLobbyEnterLockAsync()
    {
        var redisKey = RedisKeyPrefix.ChatLobbyEnterLock;
        var expireTime = TimeSpan.FromSeconds(RedisKeyExpireTime.ChatLobbyEnterLockExpireSec);

        try
        {
            var redisLock = new RedisString<char>(_redisConnection, redisKey, expireTime);
            if (await redisLock.SetAsync('$', expireTime, When.NotExists) == false)
            {
                return ErrorCode.None;
            }

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadChatFromIndexError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis Range {redisKey} Fail");
            return (ErrorCode.RedisFailException,new List<int>());
        }
    }
    
    public async Task<ErrorCode> DelLobbyEnterLockAsync()
    {
        var redisKey = RedisKeyPrefix.ChatLobbyEnterLock;
        var expireTime = TimeSpan.FromSeconds(RedisKeyExpireTime.ChatLobbyEnterLockExpireSec);

        try
        {
            var redisLock = new RedisString<char>(_redisConnection, redisKey, expireTime);
            if (await redisLock.DeleteAsync() == false)
            {
                return ErrorCode.None;
            }

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadChatFromIndexError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis Range {redisKey} Fail");
            return (ErrorCode.RedisFailException,new List<int>());
        }
    }

    
    public async Task<(ErrorCode, List<int>)> GetChatLobbyUserCountsAsync()
    {
        var redisKey = RedisKeyPrefix.ChatLobbyUserCounts;
        
        try
        {
            var redis = new RedisList<int>(_redisConnection, redisKey, null);
            var chatLobbyUserCounts = await redis.RangeAsync();
            return (ErrorCode.None, chatLobbyUserCounts.ToList());
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadChatFromIndexError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis Range {redisKey} Fail");
            return (ErrorCode.RedisFailException,new List<int>());
        }
    }
    
    public async Task<(ErrorCode, int)> GetChatLobbyUserCountsAsync(int index)
    {
        var redisKey = RedisKeyPrefix.ChatLobbyUserCounts;
        
        try
        {
            var redis = new RedisList<int>(_redisConnection, redisKey, null);
            var chatLobbyUserCount = await redis.GetByIndexAsync(index);
            return (ErrorCode.None, chatLobbyUserCount);
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadChatFromIndexError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis Range {redisKey} Fail");
            return (ErrorCode.RedisFailException,new List<int>());
        }
    }

    public async Task<ErrorCode> SetChatLobbyUserCountsAsync(int index, int value)
    {
        var redisKey = RedisKeyPrefix.ChatLobbyUserCounts;
        try
        {
            var redis = new RedisList<int>(_redisConnection, redisKey, null);
            await redis.SetByIndexAsync(index, value);
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadChatFromIndexError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis Range {redisKey} Fail");
            return ErrorCode.RedisFailException;
        }
    }
    
    public async Task<(ErrorCode,bool)> ValidateChatUserAsync(int playerId, int lobbyNumber)
    {
        
    }
    
    public async Task<(ErrorCode,bool)> SetChatUserAsync(int playerId, int lobbyNumber)
    {
        
    }

    public async Task<(ErrorCode, bool)> DelChatUserAsync(int playerId)
    {
        
    }

    
    
    
    public async Task<ErrorCode> WriteChatAsync(int lobbyNumber, int playerId, string playerNickname, string content)
    {
        if (lobbyNumber is < 1 or > ChatConfig.MaxLobbyNum)
        {
            return ErrorCode.ChatLobbyOutOfIndex;
        }
        
        Chat chat = new Chat
        {
            PlayerId = playerId,
            Content = content,
            Index = 0,
            PlayerNickname = playerNickname
        };
        
        try
        {
            var redis = new RedisList<Chat>(_redisConnection, CHAT_LOBBY_LOG + lobbyNumber, null);
            chat.Index = await redis.LengthAsync();
            await redis.RightPushAsync(chat);
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.WriteChatError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis {CHAT_LOBBY_LOG + lobbyNumber} Right Push Fail");
            return ErrorCode.RedisFailException;
        }
    }


}