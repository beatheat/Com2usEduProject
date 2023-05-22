namespace Com2usEduAPIServer.Databases.Schema;

public class Item
{
	public int Code { get; set; }
	public string Name { get; set; }
	public ItemAttribute Attribute { get; set; }
	public int Sell { get; set; }
	public int Buy { get; set; }
	public int UseLevel { get; set; }
	public int Attack { get; set; }
	public int Defence { get; set; }
	public int Magic { get; set; }		
	public int MaxEnhanceCount { get; set; }
	public bool Consumable { get; set; }
}

public enum ItemAttribute
{
	WEAPON      = 1,
	ARMOR       = 2,
	COSTUME     = 3,
	MAGICTOOL	= 4,
	MONEY		= 5,
}

public class AttendanceReward
{
	public int Day { get; set; }
	public int ItemCode { get; set; }
	public int ItemCount { get; set; }
}

public class ShopItem
{
	public int UniqueNo { get; set; }
	public int Code { get; set; }
	public int ItemCode { get; set; }
	public int ItemCount { get; set; }
}

public class StageItem
{
	public int UniqueNo { get; set; }
	public int StageCode { get; set; }
	public int ItemCode { get; set; }
	public int MaxItemCount { get; set; }
}

public class StageNpc
{
	public int Code { get; set; }
	public int StageCode { get; set; }
	public int Count { get; set; }
	public int Exp { get; set; }
}

public class InitialPlayerItem
{
	public int Code { get; set; }
	public int ItemCode { get; set; }
	public int ItemCount { get; set; }
}

