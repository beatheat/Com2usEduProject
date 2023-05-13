using System.ComponentModel.DataAnnotations;

namespace Com2usEduAPITester.ReqRes;

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