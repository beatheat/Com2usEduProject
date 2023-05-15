using System.ComponentModel.DataAnnotations;

namespace Com2usEduProject.ReqRes;

public class CompleteStageRequest
{
	[Required]
	public int PlayerId { get; set; }
}

public class CompleteStageResponse
{
	[Required] 
	public ErrorCode Result { get; set; } = ErrorCode.None;

	public bool IsStageCleared { get; set; }
}