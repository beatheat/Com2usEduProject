using System.ComponentModel.DataAnnotations;
using Com2usEduAPIServer.Databases.Schema;

namespace Com2usEduAPIServer.ReqRes;

public class ReadChatRequest
{
	[Required]
	public int PlayerId { get; set; }
	[Required] 
	public int LobbyNumber { get; set; }
	[Required]
	public long LastChatIndex { get; set; }
}

public class ReadChatResponse
{
	[Required] 
	public ErrorCode Result { get; set; } = ErrorCode.None;

	public IList<Chat> Chats { get; set; }
}