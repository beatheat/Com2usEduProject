using System.ComponentModel.DataAnnotations;
using Com2usEduProject.Databases.Schema;

namespace Com2usEduProject.ReqRes;

public class LoadChatLobbyInfoRequest
{
    [Required]
    public int PlayerId { get; set; }
}

public class LoadChatLobbyInfoResponse
{
    [Required] 
    public ErrorCode Result { get; set; } = ErrorCode.None;

    public int RecommendLobbyNumber { get; set; }
}