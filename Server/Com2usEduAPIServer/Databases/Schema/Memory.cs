namespace Com2usEduAPIServer.Databases.Schema;

public class AuthUser
{
	public int AccountId { get; set; } = 0;
	public int PlayerId { get; set; } = 0;
	public string AuthToken { get; set; } = "";
}

public class ChatUser
{
	public int PlayerId { get; set; } = 0;
	public int LobbyNumber { get; set; } = 0;
}

public class Chat
{
	public long Index { get; set; }
	public int PlayerId { get; set; }
	public string PlayerNickname { get; set; }
	public string Content { get; set; }
}

public class PlayerInGameStageInfo
{
	public int PlayerId { get; set; }
	public int PlayingStageCode { get; set; }
	public int HighestClearStageCode { get; set; }
	// 스테이지에서 파밍한 아이템 코드와 개수를 저장 key: 아이템 코드, value :아이템 개수
	public Dictionary<int, int> FarmedStageItemCounts { get; set; } = new ();
	// 스테이지에서 파밍한 NPC 코드와 개수를 저장 key: npc 코드, value: npc 개수
	public Dictionary<int,int> FarmedStageNpcCounts { get; set; } = new();

	// 해당 스테이지에서 파밍할 수 있는 아이템 코드와 최대개수를 저장 key: 아이템 코드, value: 아이템 최대개수
	public Dictionary<int, int> MaxAvailableItemCounts { get; set; } = new();
	// 해당 스테이지에서 파밍할 수 있는 NPC 코드와 최대개수를 저장 key: NPC 코드, value: NPC 최대개수
	public Dictionary<int, int> MaxAvailableStageNpcCounts { get; set; } = new();
}