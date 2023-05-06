using System.ComponentModel.DataAnnotations;

namespace Com2usEduAPITester.ReqRes;

public class ReceiveAttendanceRewardRequest
{
	[Required]
	public int PlayerId { get; set; }
}

public class ReceiveAttendanceRewardResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None;
	
}