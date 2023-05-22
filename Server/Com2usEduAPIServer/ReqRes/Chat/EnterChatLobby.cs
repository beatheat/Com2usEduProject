using System.ComponentModel.DataAnnotations;
using Com2usEduAPIServer.Databases.Schema;

namespace Com2usEduAPIServer.ReqRes;

public class EnterChatLobbyRequest
{
	[Required]
	public int PlayerId { get; set; }
	
	public int LobbyNumber { get; set; } = -1;
}

public class EnterChatLobbyResponse
{
	[Required] 
	public ErrorCode Result { get; set; } = ErrorCode.None;

	public IList<Chat> ChatHistory { get; set; }
	
	public int LobbyNumber { get; set; }
}