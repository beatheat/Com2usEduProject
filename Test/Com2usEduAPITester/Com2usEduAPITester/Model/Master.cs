using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com2usEduAPITester.Model;

public class StageNpc
{
    public int StageCode { get; set; }
    public int Code { get; set; }
    public int Count { get; set; }
    public int Exp { get; set; }
}

public class StageItem
{
    public int StageCode { get; set; }
    public int ItemCode { get; set; }
    public int MaxItemCount { get; set; }
}
