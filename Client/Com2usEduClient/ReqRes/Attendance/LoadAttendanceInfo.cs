using Com2usEduClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com2usEduClient.Game;

namespace Com2usEduClient;

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