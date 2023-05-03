using Com2usEduProject.DBSchema;
using Com2usEduProject.ModelDB;

namespace Com2usEduProject.Databases;

public interface IGameDb
{
	public Task<(ErrorCode,int)> CreatePlayerDataAsync(int accountId);
	public Task<(ErrorCode, Player)> LoadPlayerDataAsync(int accountId);

	public Task<(ErrorCode, int)> InsertPlayerItemAsync(PlayerItem item);
	public Task<(ErrorCode, int)> InsertPlayerItemAsync(int playerId, Item item, int count);
	public Task<(ErrorCode, IList<PlayerItem>)> LoadPlayerItemsAsync(int playerId);
	public Task<ErrorCode> DeletePlayerItemAsync(int itemId);

	public Task<(ErrorCode,int)> InsertMailAsync(Mail mail);
	public Task<(ErrorCode,int)> InsertMailItemAsync(MailItem mailItem);
	public Task<(ErrorCode, int)> LoadMailboxPageCountAsync(int playerId);
	public Task<(ErrorCode, IList<Mail>)> LoadMailboxPageAsync(int playerId, int pageNo);
	public Task<(ErrorCode, Mail)> LoadMailAsync(int mailId);
	public Task<ErrorCode> UpdateMailItemReceivedToTrueAsync(int mailId);

	public Task<(ErrorCode, IList<MailItem>)> LoadMailItemsAsync(int mailId);

}