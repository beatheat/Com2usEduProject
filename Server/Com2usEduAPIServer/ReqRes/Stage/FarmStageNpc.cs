using System.ComponentModel.DataAnnotations;

namespace Com2usEduAPIServer.ReqRes;

public class FarmStageNpcRequest
{
	[Required]
	public int PlayerId { get; set; }
	[Required]
	public int NpcCode { get; set; }
}

public class FarmStageNpcResponse
{
	[Required] 
	public ErrorCode Result { get; set; } = ErrorCode.None;
}