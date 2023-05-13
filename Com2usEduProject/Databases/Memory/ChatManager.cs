using System.Diagnostics;
using CloudStructures;
using CloudStructures.Structures;
using Com2usEduProject.Chatting;
using Com2usEduProject.Tools;
using ZLogger;

namespace Com2usEduProject.Databases;

public class ChatManager
{
	readonly RedisConnection _redisConnection;
	readonly ILogger<RedisDb> _logger;
    
	
	const string CHAT_LOBBY = "ChatLobby_";

    private const int CHAT_HISTORY_SIZE = 50;
    
	public ChatManager(RedisConnection redisConnection, ILogger<RedisDb> logger)
	{
		_redisConnection = redisConnection;
		_logger = logger;
	}

    public void Init()
    {
        // var redis = new RedisList<Chat>(_redisConnection, CHAT_LOBBY, null);
    }

    public async Task<(ErrorCode, IList<Chat>)> LoadChatHistory(int lobbyNumber)
    {
        if (lobbyNumber is < 1 or > 100)
        {
            return (ErrorCode.ChatLobbyOutOfIndex, Array.Empty<Chat>());
        }
        try
        {
            var redis = new RedisList<Chat>(_redisConnection, CHAT_LOBBY + lobbyNumber, null);
            var len = await redis.LengthAsync();
            var chatList = await redis.RangeAsync(-CHAT_HISTORY_SIZE, -1);
            return (ErrorCode.None, chatList);
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadChatHistoryError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis Range {CHAT_LOBBY+lobbyNumber} Fail");           
            return (ErrorCode.RedisFailException, Array.Empty<Chat>());
        }
    }
    
    public async Task<(ErrorCode,IList<Chat>)> LoadChatFromIndex(int lobbyNumber, long index)
    {
        Console.WriteLine("?");
        if (lobbyNumber is < 1 or > 100)
        {
            return (ErrorCode.ChatLobbyOutOfIndex, Array.Empty<Chat>());
        }
        try
        {
            var redis = new RedisList<Chat>(_redisConnection, CHAT_LOBBY + lobbyNumber, null);
            var chatList = await redis.RangeAsync(index, -1);
            return (ErrorCode.None, chatList);
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadChatFromIndexError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis Range {CHAT_LOBBY+lobbyNumber} Fail");
            return (ErrorCode.RedisFailException, Array.Empty<Chat>());
        }
    }

    public async Task<ErrorCode> WriteChat(int lobbyNumber, int playerId, string playerNickname, string content)
    {
        if (lobbyNumber is < 1 or > 100)
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
            var redis = new RedisList<Chat>(_redisConnection, CHAT_LOBBY + lobbyNumber, null);
            var len = await redis.LengthAsync();
            chat.Index = len;
            await redis.RightPushAsync(chat);
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.WriteChatError], e,
                new {ErrorCode = ErrorCode.RedisFailException}, $"Redis {CHAT_LOBBY + lobbyNumber} Right Push Fail");
            return ErrorCode.RedisFailException;
        }
    }


}