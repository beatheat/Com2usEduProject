using System.ComponentModel.DataAnnotations;

using Com2usEduAPITester.Model;

namespace Com2usEduAPITester.ReqRes.Stage;

public class EnterStageRequest
{
	[Required]
	public int PlayerId { get; set; }
	[Required]
	public int StageCode { get; set; }
}

public class EnterStageResponse
{
	[Required] 
	public ErrorCode Result { get; set; } = ErrorCode.None;
	
	public IList<StageItem> StageItems { get; set; }
	public IList<StageNpc> StageNpcs { get; set; }
}