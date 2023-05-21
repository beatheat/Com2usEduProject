using Com2usEduProject.Databases;
using Com2usEduProject.Databases.Schema;
using Com2usEduProject.ReqRes;
using Microsoft.AspNetCore.Mvc;

namespace Com2usEduProject.Controllers;



[ApiController]
[Route("[controller]")]
public class LoadChatLobbyInfo
{
    readonly IMemoryDb _memoryDb;
    readonly ILogger<LoadChatLobbyInfo> _logger;
	
    public LoadChatLobbyInfo(ILogger<LoadChatLobbyInfo> logger, IMemoryDb memoryDb)
    {
        _logger = logger;
        _memoryDb = memoryDb;
    }
    [HttpPost]
    public async Task<LoadChatLobbyInfoResponse> Post(LoadChatLobbyInfoRequest request)
    {
        var response = new LoadChatLobbyInfoResponse();
        var errorCode = ErrorCode.None;

        (errorCode, var chatLobbyUserCounts) = await _memoryDb.ChatManager.GetChatLobbyUserCountsAsync();
        if (errorCode!= ErrorCode.None)
        {
            
        }

        response.RecommendLobbyNumber = GetRecommendLobbyNumber(chatLobbyUserCounts);
        
        return response;
    }

    private int GetRecommendLobbyNumber(List<int> chatLobbyUserCounts)
    {
        for (var i = 0; i < chatLobbyUserCounts.Count; i++)
        {
            if (i < 75)
            {
                return i;
            }
        }
        
        for (var i = 0; i < chatLobbyUserCounts.Count; i++)
        {
            if (i < 100)
            {
                return i;
            }
        }

        return -1;
    }
}