using System.ComponentModel.DataAnnotations;

namespace Com2usEduClient.ReqRes;

public class ReceiveAttendanceRewardRequest
{
	[Required]
	public int PlayerId { get; set; }
}

public class ReceiveAttendanceRewardResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None;
	
}