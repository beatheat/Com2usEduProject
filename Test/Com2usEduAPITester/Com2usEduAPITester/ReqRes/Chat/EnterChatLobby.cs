using System.ComponentModel.DataAnnotations;


using Com2usEduAPITester.Game;

namespace Com2usEduAPITester.ReqRes;

public class EnterChatLobbyRequest
{
	[Required]
	public int PlayerId { get; set; }
	[Required] 
	public int LobbyNumber { get; set; } = 1;
}

public class EnterChatLobbyResponse
{
	[Required] 
	public ErrorCode Result { get; set; } = ErrorCode.None;

	public IList<Chat> ChatHistory { get; set; }
	
	public int LobbyNumber { get; set; } = 1;
}