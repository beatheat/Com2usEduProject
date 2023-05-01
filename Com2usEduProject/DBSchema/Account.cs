namespace Com2usEduProject.DBSchema;

public class Account
{
	public int AccountId { get; set; }

	public string Id { get; set; }
	public string HashedPassword { get; set; }
	public string SaltValue { get; set; }
}

/// Redis 관련=========================================

public class AuthUser
{
	public string Id { get; set; } = "";
	public string AuthToken { get; set; } = "";
	public int AccountId { get; set; } = 0;
	public string State { get; set; } = ""; 
}


public class RediskeyExpireTime
{
	public const ushort NxKeyExpireSecond = 3;
	public const ushort RegistKeyExpireSecond = 6000;
	public const ushort LoginKeyExpireMin = 60; 
	public const ushort TicketKeyExpireSecond = 6000; // 현재 테스트를 위해 티켓은 10분동안 삭제하지 않는다. 
}
