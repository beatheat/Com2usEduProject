using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com2usEduAPITester.Game;
using Com2usEduAPITester.Model;

namespace Com2usEduAPITester;

public static class MasterData
{
    public static string[] ItemName =
    {
       "dummy", "돈", "작은 칼" , "도금 칼", "나무 방패" , "보통 모자" , "포션"
    };

    public static List<StageNpc> StageNpcs= new List<StageNpc>();
    public static List<ItemBundle> attendanceRewards = new List<ItemBundle>();


    public static void Init()
    {
        StageNpcs.Add(new StageNpc { StageCode = 1, Code = 101, Count = 10, Exp = 10 });
        StageNpcs.Add(new StageNpc { StageCode = 1, Code = 110, Count = 12, Exp = 15 });
        StageNpcs.Add(new StageNpc { StageCode = 2, Code = 201, Count = 40, Exp = 20 });
        StageNpcs.Add(new StageNpc { StageCode = 2, Code = 211, Count = 20, Exp = 35 });
        StageNpcs.Add(new StageNpc { StageCode = 2, Code = 221, Count = 1, Exp = 50 });
        StageNpcs.Add(new StageNpc { StageCode = 3, Code = 301, Count = 10, Exp = 25 });
        StageNpcs.Add(new StageNpc { StageCode = 3, Code = 311, Count = 5, Exp = 40 });
        StageNpcs.Add(new StageNpc { StageCode = 4, Code = 401, Count = 20, Exp = 35 });
        StageNpcs.Add(new StageNpc { StageCode = 4, Code = 411, Count = 10, Exp = 45 });
        StageNpcs.Add(new StageNpc { StageCode = 4, Code = 421, Count = 2, Exp = 80 });

        //1~5
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 100 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 100 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 100 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 200 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 200 });
        //6~10
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 200 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 2, ItemCount = 1 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 100 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 100 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 100 });
        //11~15
        attendanceRewards.Add(new ItemBundle { ItemCode = 6, ItemCount = 5 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 150 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 150 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 150 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 150 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 150 });
        //16~20
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 150 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 150 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 4, ItemCount = 1 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 200 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 200 });
        //21~25
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 200 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 200 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 200 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 5, ItemCount = 1 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 250 });
        //26~30
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 250 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 250 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 250 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 1, ItemCount = 250 });
        attendanceRewards.Add(new ItemBundle { ItemCode = 3, ItemCount = 1 });
    }
}