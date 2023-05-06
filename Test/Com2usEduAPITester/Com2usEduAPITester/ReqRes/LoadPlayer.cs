using Com2usEduAPITester.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com2usEduAPITester.ReqRes;

public class LoadPlayerRequest
{
    [Required]
    public int PlayerId { get; set; }
}

public class LoadPlayerResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;

    public Player Player { get; set; }
}
