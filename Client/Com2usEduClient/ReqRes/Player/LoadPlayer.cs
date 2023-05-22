using System.ComponentModel.DataAnnotations;
using Com2usEduClient.Game;

namespace Com2usEduClient.ReqRes;


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