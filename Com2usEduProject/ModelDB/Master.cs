namespace Com2usEduProject.ModelDB;

public class Item
{
	public int Code { get; set; }
	public string Name { get; set; }
	public string Attribute { get; set; }
	public int Sell { get; set; }
	public int Buy { get; set; }
	public int UseLevel { get; set; }
	public int Attack { get; set; }
	public int Defence { get; set; }
	public int Magic { get; set; }		
	public int EnhanceMaxCount { get; set; }
	public bool Consumable { get; set; }
}

public enum ItemType
{
	WEAPON      = 1,
	ARMOR       = 2,
	COSTUME     = 3,
	MAGICTOOL	= 4,
	MONEY		= 5,
}

public class AttendanceReward
{
	public int Date { get; set; }
	public int ItemCode { get; set; }
	public int ItemCount { get; set; }
}

public class ShopItem
{
	public int Code { get; set; }
	public int ItemCode { get; set; }
	public string ItemName { get; set; }
	public int ItemCount { get; set; }
}

public class StageItem
{
	public int Code { get; set; }
	public int ItemCode { get; set; }
}

public class StageNPC
{
	public int Code { get; set; }
	public int NPCCode { get; set; }
	public int Count { get; set; }
	public int Exp { get; set; }
}