namespace Com2usEduProject.Databases.Schema;

public class Player
{
	public int Id { get; set; }
	public string Nickname { get; set; }
	public int AccountId { get; set; }
	public int HP { get; set; }
	public int Attack { get; set; }
	public int Defence { get; set; }
	public int Magic { get; set; }
	public int Exp { get; set; }
	public int Level { get; set; }
	public int Money { get; set; }
}

public class PlayerAttendance
{
	public int PlayerId { get; set; }
	public int ContinuousAttendanceDays { get; set; }
	public DateTime LastAttendanceDate { get; set; }
}

public class PlayerCompletedStage
{
	public int PlayerId { get; set; }
	public int StageCode { get; set; }
}


public class PlayerItem
{
	public int Id { get; set; }
	public int PlayerId { get; set; }
	public int ItemCode { get; set; }
	public int Attack { get; set; }
	public int Defence { get; set; }
	public int Magic { get; set; }
	public int EnhanceCount { get; set; }
	public int Count { get; set; }
}

public class Mail
{
	// 메일 외부 정보
	public int Id { get; set; }
	public int PlayerId { get; set; }
	public string Name { get; set; }
	public DateTime TransmissionDate { get; set; }
	public DateTime ExpireDate { get; set; }
	public bool IsItemReceived { get; set; }

	// 메일 내부 정보
	public string Content { get; set; }

	public int ItemCode1 { get; set; } = -1;
	public int ItemCode2 { get; set; } = -1;
	public int ItemCode3 { get; set; } = -1;
	public int ItemCode4 { get; set; } = -1;
	
	public int ItemCount1 { get; set; } = 0;
	public int ItemCount2 { get; set; } = 0;
	public int ItemCount3 { get; set; } = 0;
	public int ItemCount4 { get; set; } = 0;
}

public class Bill
{
	public long Token { get; set; }
	public int PlayerId { get; set; }
}