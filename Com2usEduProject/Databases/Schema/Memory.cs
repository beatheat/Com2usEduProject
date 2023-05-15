namespace Com2usEduProject.Databases.Schema;

public class AuthUser
{
	public int AccountId { get; set; } = 0;
	public int PlayerId { get; set; } = 0;
	public string AuthToken { get; set; } = "";
}

public class Chat
{
	public long Index { get; set; }
	public int PlayerId { get; set; }
	public string PlayerNickname { get; set; }
	public string Content { get; set; }
}

public class PlayerStageInfo
{
	public int PlayerId { get; set; }
	public int StageCode { get; set; }
	public Dictionary<int, int> FarmedStageItemCounts { get; set; } = new ();
	public Dictionary<int,int> FarmedStageNpcCounts { get; set; } = new();
	public int FarmedExp { get; set; } = 0;
}
