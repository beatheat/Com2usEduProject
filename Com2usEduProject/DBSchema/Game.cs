﻿namespace Com2usEduProject.DBSchema;

public class Player
{
	public int PlayerId { get; set; }
	public int AccountId { get; set; }
	public int ContinuousAttendanceDays { get; set; }
	public DateTime LastAttendanceDate { get; set; }
}

public class PlayerItem
{
	public int UniqueNo { get; set; }
	public int PlayerId { get; set; }
	public int ItemCode { get; set; }
	public int Attack { get; set; }
	public int Defence { get; set; }
	public int Magic { get; set; }
	public int EnhanceCount { get; set; }
	public int Count { get; set; }
}

public class Mailbox
{
	public int UniqueNo { get; set; }
	public int PostName { get; set; }
	public int ItemCode { get; set; }
	public int ItemCount { get; set; }
	public DateTime ExpireDate { get; set; }
	public bool IsItemReceived { get; set; }
}

public class Bill
{
	public long UniqueToken { get; set; }
	public int PlayerId { get; set; }
}