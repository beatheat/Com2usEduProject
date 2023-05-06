namespace Com2usEduProject.DBSchema;

public class Player
{
	public int Id { get; set; }
	public int AccountId { get; set; }
	public int ContinuousAttendanceDays { get; set; }
	public DateTime? LastAttendanceDate { get; set; }
	public int Money { get; set; }
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
}

public class MailItem
{
	public int Id { get; set; }
	public int MailId { get; set; }
	public int ItemCode { get; set; }
	public int ItemCount { get; set; }
}

public class Bill
{
	public long Id { get; set; }
	public long Token { get; set; }
	public int PlayerId { get; set; }
}