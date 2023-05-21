using System.ComponentModel.DataAnnotations;

namespace Com2usEduAPITester.ReqRes;

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