using System.ComponentModel.DataAnnotations;

namespace Com2usEduProject.ReqRes;


public class LoadPlayerStageInfoRequest
{
	[Required]
	public int PlayerId { get; set; }
}

public class LoadPlayerStageInfoResponse
{
	[Required] 
	public ErrorCode Result { get; set; } = ErrorCode.None;

	public int MaxStageCode { get; set; }
	public List<int> AccessibleStageCode { get; set; }
}