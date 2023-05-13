using System.ComponentModel.DataAnnotations;
using Com2usEduAPITester.Game;

namespace Com2usEduAPITester.ReqRes;


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