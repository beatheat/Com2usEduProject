using Com2usEduProject.Databases.Schema;

namespace Com2usEduProject.Databases;

public interface IMasterDb
{
	public void Init(string dbConnectionString);
	
	public (ErrorCode, Item) GetItem(int itemCode);
	public (ErrorCode, AttendanceReward) GetAttendanceReward(int day);
	public (ErrorCode, List<ShopItem>) GetShopItem(int shopItemCode);
	public (ErrorCode, List<StageItem>) GetStageItem(int stageCode);
	public (ErrorCode, List<StageNpc>) GetStageNpc(int stageCode);
	
	public List<InitialPlayerItem> GetInitialPlayerItem();
	
	public string GetVersion();
	public string GetClientVersion();
}