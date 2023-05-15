using System.ComponentModel.DataAnnotations;

namespace Com2usEduAPITester.ReqRes;


public class LoadPlayerStageInfoRequest
{
	[Required]
	public int PlayerId { get; set; }
}

public class LoadPlayerStageInfoResponse
{
	[Required] 
	public ErrorCode Result { get; set; } = ErrorCode.None;

	public IList<int> CompletedStages { get; set; }
	public IList<int> AccessibleStages { get; set; }

	public int MaxStageCode { get; set; }
}