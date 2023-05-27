using System.ComponentModel.DataAnnotations;

namespace Com2usEduAPIServer.ReqRes;


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
	public IList<int> ClearStageCodes { get; set; }
	public IList<int> AccessibleStageCodes { get; set; }
}