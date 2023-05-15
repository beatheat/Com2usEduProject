using System.ComponentModel.DataAnnotations;
using Com2usEduProject.Databases.Schema;

namespace Com2usEduProject.ReqRes;

public class LoadAttendanceInfoRequest
{
	[Required]
	public int PlayerId { get; set; }
}

public class LoadAttendanceInfoResponse
{
	[Required] public ErrorCode Result { get; set; } = ErrorCode.None;
	
	public PlayerAttendance AttendanceInfo { get; set; }
}