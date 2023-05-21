using System.ComponentModel.DataAnnotations;
using Com2usEduProject.Databases.Schema;

namespace Com2usEduProject.ReqRes;

public class ExitChatLobbyRequest
{
	[Required]
	public int PlayerId { get; set; }
}

public class ExitChatLobbyResponse
{
	[Required] 
	public ErrorCode Result { get; set; } = ErrorCode.None;
}