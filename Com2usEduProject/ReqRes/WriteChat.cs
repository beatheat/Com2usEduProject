using System.ComponentModel.DataAnnotations;
using Com2usEduProject.Chatting;

namespace Com2usEduProject.ReqRes;

public class WriteChatRequest
{
	[Required]
	public int PlayerId { get; set; }
	[Required]
	public string PlayerNickName { get; set; }
	[Required] 
	public int LobbyNumber { get; set; }
	[Required]
	public string Chat { get; set; }
}

public class WriteChatResponse
{
	[Required] 
	public ErrorCode Result { get; set; } = ErrorCode.None;
}