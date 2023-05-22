using System.ComponentModel.DataAnnotations;
using Com2usEduAPIServer.Databases.Schema;

namespace Com2usEduAPIServer.ReqRes;

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