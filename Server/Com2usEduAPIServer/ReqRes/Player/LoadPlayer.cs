using System.ComponentModel.DataAnnotations;
using Com2usEduAPIServer.Databases.Schema;

namespace Com2usEduAPIServer.ReqRes;


public class LoadPlayerRequest
{
	[Required]
	public int PlayerId { get; set; }
}

public class LoadPlayerResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None;

	public Player Player { get; set; }
}