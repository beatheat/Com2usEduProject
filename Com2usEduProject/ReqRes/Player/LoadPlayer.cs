using System.ComponentModel.DataAnnotations;
using Com2usEduProject.Databases.Schema;

namespace Com2usEduProject.ReqRes;


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