using Com2usEduProject.DBSchema;

namespace Com2usEduProject.Databases;

public interface IMasterDb
{
	public void Init(string dbConnectionString);
	
	public (ErrorCode, Item) GetItem(int itemCode);
	public (ErrorCode, AttendanceReward) GetAttendanceReward(int day);
	public (ErrorCode, IList<ShopItem>) GetShopItem(int shopItemCode);
	public (ErrorCode, IList<StageItem>) GetStageItem(int stageCode);
	public (ErrorCode, IList<StageNpc>) GetStageNpc(int stageCode);
	
	public IList<InitialPlayerItem> GetInitialPlayerItem();
	
	public string GetVersion();
	public string GetClientVersion();
}