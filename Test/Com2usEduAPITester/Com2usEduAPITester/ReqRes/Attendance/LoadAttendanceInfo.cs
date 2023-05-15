using Com2usEduAPITester;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com2usEduAPITester.Game;

namespace Com2usEduAPITester;

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