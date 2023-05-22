using System.ComponentModel.DataAnnotations;

namespace Com2usEduAPIServer.ReqRes;

public class FarmStageItemRequest
{
	[Required]
	public int PlayerId { get; set; }
	[Required]
	public int ItemCode { get; set; }
	[Required]
	public int ItemCount { get; set; }
}

public class FarmStageItemResponse
{
	[Required] 
	public ErrorCode Result { get; set; } = ErrorCode.None;
}