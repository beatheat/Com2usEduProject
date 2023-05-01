using Com2usEduProject.DBSchema;
using Com2usEduProject.ModelDB;

namespace Com2usEduProject.Services;

public interface IGameDb
{
	public Task<(ErrorCode,int)> CreatePlayerDataAsync(int accountId);
	public Task<(ErrorCode, Player)> LoadPlayerDataAsync(int accountId);

	public Task<(ErrorCode, int)> InsertPlayerItemAsync(PlayerItem item);
	public Task<(ErrorCode, IList<PlayerItem>)> LoadPlayerItemsAsync(int playerId);
}