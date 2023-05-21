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
 * 1. 로비 100명이 넘어갈 때 동시성 제어 접근할 수 있음
 * 2. 유저 EXPIRE되면 LOBBY COUNT 줄이는거랑 ChatUser 만료되면 lobbyCount 줄이기
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
        Init();
	}

    private async void Init()
    {
        var redisKey = RedisKeyPrefix.ChatLobbyUserCounts;
        try
        {
            var redis = new RedisList<int>(_redisConnection, redisKey, null);
            for (int i = 0; i < ChatConfig.MaxLobbyNum; i++)
                await redis.SetByIndexAsync(i, 0);
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.ChatInitError], e, 
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis List Set {redisKey} Fail");
        }
    }
    
    private bool ValidateChatLobbyNum(int lobbyNumber)
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
                new {ErrorCode = ErrorCode.RedisFailException, LobbyNumber = lobbyNumber}, $"Redis List Range {redisKey} Fail");           
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
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadChatHistoryFromIndexError], e,
                new {ErrorCode = ErrorCode.RedisFailException, LobbyNumber = lobbyNumber, Index = index}, $"Redis List Range ({redisKey}) Fail");
            return (ErrorCode.RedisFailException, Array.Empty<Chat>());
        }
    }

        
    public async Task<ErrorCode> WriteChatAsync(int lobbyNumber, int playerId, string playerNickname, string content)
    {
        if (lobbyNumber is < 1 or > ChatConfig.MaxLobbyNum)
        {
            return ErrorCode.ChatLobbyOutOfIndex;
        }

        var redisKey = RedisKeyPrefix.ChatLobbyLog + lobbyNumber;
        
        Chat chat = new Chat
        {
            PlayerId = playerId,
            Content = content,
            Index = 0,
            PlayerNickname = playerNickname
        };
        
        try
        {
            var redis = new RedisList<Chat>(_redisConnection, redisKey, null);
            chat.Index = await redis.LengthAsync();
            await redis.RightPushAsync(chat);
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.WriteChatError], e,
                new {ErrorCode = ErrorCode.RedisFailException, LobbyNumber = lobbyNumber, PlayerId = playerId}, $"Redis List Right Push ({redisKey}) Fail");
            return ErrorCode.RedisFailException;
        }
    }

    public async Task<ErrorCode> ValidateChatUserAsync(int playerId, int lobbyNumber)
    {
        var redisKey = RedisKeyPrefix.ChatUser + playerId;
        try
        {
            var redis = new RedisString<ChatUser>(_redisConnection, redisKey, null);
            var chatUser = await redis.GetAsync();
            if (!chatUser.HasValue || chatUser.Value.LobbyNumber != lobbyNumber)
            {
                _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.ValidateChatUserError],
                    new {ErrorCode = ErrorCode.ChatUserNotFound, PlayerId = playerId, LobbyNumber = lobbyNumber}, $"Chat User Not Found in Redis");
                return ErrorCode.ChatUserNotFound;
            }

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.ValidateChatUserError], e,
                new {ErrorCode = ErrorCode.RedisFailException, PlayerId = playerId, LobbyNumber = lobbyNumber}, $"Redis String Get ({redisKey}) Fail");
            return ErrorCode.RedisFailException;
        }
    }

    
    public async Task<ErrorCode> EnterLobby(int playerId, int lobbyNumber)
    {
        var errorCode = await SetLobbyEnterLockAsync();
        if (errorCode != ErrorCode.None)
        {
            return errorCode;
        }
		
        (errorCode, var lobbyUserCounts)  = await GetLobbyUserCountsAsync(lobbyNumber);
        if (lobbyUserCounts >= ChatConfig.MaxLobbyUserNum)
        {
            await DelLobbyEnterLockAsync();
            return errorCode;
        }
		
        errorCode = await SetLobbyUserCountsAsync(lobbyNumber, lobbyUserCounts + 1);
        if (errorCode != ErrorCode.None)
        {
            await DelLobbyEnterLockAsync();
            return errorCode;
        }
		
        await DelLobbyEnterLockAsync();

        errorCode = await SetChatUserAsync(playerId, lobbyNumber);
        if (errorCode != ErrorCode.None)
        {
            return errorCode;
        }

        return ErrorCode.None;
    }

    public async Task<ErrorCode> ExitLobby(int playerId)
    {
        var (errorCode,chatUser) = await GetChatUserAsync(playerId);
        if (errorCode != ErrorCode.None)
        {
            return errorCode;
        }

        errorCode = await DelChatUserAsync(playerId);
        if (errorCode != ErrorCode.None)
        {
            return errorCode;
        }
        
        (errorCode, var lobbyUserCounts)  = await GetLobbyUserCountsAsync(chatUser.LobbyNumber);
        if (lobbyUserCounts >= ChatConfig.MaxLobbyUserNum)
        {
            return errorCode;
        }
		
        errorCode = await SetLobbyUserCountsAsync(chatUser.LobbyNumber, lobbyUserCounts - 1);
        if (errorCode != ErrorCode.None)
        {
            return errorCode;
        }

        return ErrorCode.None;
    }

    public async Task<(ErrorCode,int)> GetRecommendLobbyNumber()
    {
        var (errorCode, lobbyUserCounts) = await GetLobbyUserCountsAsync();
        if (errorCode != ErrorCode.None)
        {
            return (errorCode, -1);
        }
        
        for (int lobbyNumber = 0; lobbyNumber < lobbyUserCounts.Count; lobbyNumber++)
        {
            if (lobbyUserCounts[lobbyNumber] < ChatConfig.MaxLobbyUserNum * 0.75)
            {
                return (ErrorCode.None, lobbyNumber);
            }
        }
        
        for (int lobbyNumber = 0; lobbyNumber < lobbyUserCounts.Count; lobbyNumber++)
        {
            if (lobbyUserCounts[lobbyNumber] < ChatConfig.MaxLobbyUserNum)
            {
                return (ErrorCode.None, lobbyNumber);
            }
        }

        return (ErrorCode.ChatLobbyFull, -1);
    }
    
    private async Task<ErrorCode> SetLobbyEnterLockAsync()
    {
        var redisKey = RedisKeyPrefix.ChatLobbyEnterLock;
        var expireTime = TimeSpan.FromSeconds(RedisKeyExpireTime.ChatLobbyEnterLockExpireSec);

        try
        {
            var redisLock = new RedisString<char>(_redisConnection, redisKey, expireTime);
            if (await redisLock.SetAsync('$', expireTime, When.NotExists) == false)
            {
                _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.SetLobbyEnterLockError],
                    new {ErrorCode = ErrorCode.RedisSetDuplicateKey}, $"Redis String ({redisKey}) Duplicate");
                return ErrorCode.RedisSetDuplicateKey;
            }

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.SetLobbyEnterLockError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis String Set ({redisKey}) Fail");
            return ErrorCode.RedisFailException;
        }
    }
    
    private async Task<ErrorCode> DelLobbyEnterLockAsync()
    {
        var redisKey = RedisKeyPrefix.ChatLobbyEnterLock;
        var expireTime = TimeSpan.FromSeconds(RedisKeyExpireTime.ChatLobbyEnterLockExpireSec);

        try
        {
            var redisLock = new RedisString<char>(_redisConnection, redisKey, expireTime);
            if (await redisLock.DeleteAsync() == false)
            {
                _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.DelLobbyEnterLockError],
                    new {ErrorCode = ErrorCode.RedisKeyNotFound}, $"Redis String ({redisKey}) Key Not Found");
                return ErrorCode.RedisKeyNotFound;
            }
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.DelLobbyEnterLockError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis String Del ({redisKey}) Fail");
            return ErrorCode.RedisFailException;
        }
    }

    
    private async Task<(ErrorCode, List<int>)> GetLobbyUserCountsAsync()
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
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GetLobbyUserCountsError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis List Range ({redisKey}) Fail");
            return (ErrorCode.RedisFailException,new List<int>());
        }
    }
    
    private async Task<(ErrorCode, int)> GetLobbyUserCountsAsync(int index)
    {
        var redisKey = RedisKeyPrefix.ChatLobbyUserCounts;
        
        try
        {
            var redis = new RedisList<int>(_redisConnection, redisKey, null);
            var chatLobbyUserCount = await redis.GetByIndexAsync(index);
            if (!chatLobbyUserCount.HasValue)
            {
                _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GetLobbyUserCountsError],
                    new {ErrorCode = ErrorCode.LobbyUserCountNotExist, Index = index}, $"Redis List Get ({redisKey}) Fail");
                return (ErrorCode.LobbyUserCountNotExist, 0);
            }
            return (ErrorCode.None, chatLobbyUserCount.Value);
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GetLobbyUserCountsError], e,
                new {ErrorCode = ErrorCode.RedisFailException, Index = index}, $"Redis List Get ({redisKey}) Fail");
            return (ErrorCode.RedisFailException, -1);
        }
    }

    private async Task<ErrorCode> SetLobbyUserCountsAsync(int index, int value)
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
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.SetLobbyUserCountsError], e,
                new {ErrorCode = ErrorCode.RedisFailException, Index = index, Value = value}, $"Redis List Set ({redisKey}) Fail");
            return ErrorCode.RedisFailException;
        }
    }
    

    private async Task<ErrorCode> SetChatUserAsync(int playerId, int lobbyNumber)
    {
        var chatUser = new ChatUser
        {
            PlayerId = playerId,
            LobbyNumber = lobbyNumber
        };
        var redisKey = RedisKeyPrefix.ChatUser + playerId;
        var expireTime = TimeSpan.FromMinutes(RedisKeyExpireTime.ChatUserExpireMin);
        try
        {
            var redis = new RedisString<ChatUser>(_redisConnection, redisKey, expireTime);
            if (await redis.SetAsync(chatUser, expireTime) == false)
            {
                _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.SetChatUserError],
                    new {ErrorCode = ErrorCode.SetChatUserFail, PlayerId = playerId, LobbyNumber = lobbyNumber}, $"Redis String ({redisKey}) Fail");
                return ErrorCode.SetChatUserFail;
            }
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.SetChatUserError], e,
                new {ErrorCode = ErrorCode.RedisFailException, PlayerId = playerId, LobbyNumber = lobbyNumber}, $"Redis String Set ({redisKey}) Fail");
            return ErrorCode.RedisFailException;
        }
    }
    
    private async Task<(ErrorCode, ChatUser)> GetChatUserAsync(int playerId)
    {
        var redisKey = RedisKeyPrefix.ChatUser + playerId;
        try
        {
            var redis = new RedisString<ChatUser>(_redisConnection, redisKey, null);
            var chatUser = await redis.GetAsync();
            if (!chatUser.HasValue)
            {
                _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GetChatUserError],
                    new {ErrorCode = ErrorCode.RedisKeyNotFound}, $"Redis String Get ({redisKey}) Key Not Found");
                return (ErrorCode.RedisKeyNotFound, new ChatUser());
            }
            return (ErrorCode.None, chatUser.Value);
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GetChatUserError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis String Get ({redisKey}) Fail");
            return (ErrorCode.RedisFailException, new ChatUser());
        }
    }

    private async Task<ErrorCode> DelChatUserAsync(int playerId)
    {
        var redisKey = RedisKeyPrefix.ChatUser + playerId;
        var expireTime = TimeSpan.FromMinutes(RedisKeyExpireTime.ChatUserExpireMin);
        try
        {
            var redis = new RedisString<ChatUser>(_redisConnection, redisKey, expireTime);
            if (await redis.DeleteAsync() == false)
            {
                _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.DelChatUserError],
                    new {ErrorCode = ErrorCode.RedisKeyNotFound}, $"Redis String ({redisKey}) Key Not Found");
                return ErrorCode.RedisKeyNotFound;
            }
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.DelChatUserError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis String ({redisKey}) Fail");
            return ErrorCode.RedisFailException;
        }
    }



}