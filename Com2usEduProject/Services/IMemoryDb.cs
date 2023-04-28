using Com2usEduProject.ModelDB;

namespace Com2usEduProject.Services;

public interface IMemoryDb
{
	public void Init(string address);
    
	public Task<ErrorCode> RegisterUserAsync(string id, string authToken, long accountId);

	// public Task<ErrorCode> CheckUserAuthAsync(string id, string authToken);
	
	public Task<(ErrorCode, AuthUser)> GetUserAsync(string id);
	
	//
	// public Task<bool> SetUserStateAsync(AuthUser user, UserState userState);
	//

	public Task<(ErrorCode,string)> GetLatestMasterDataVersionAsync();

	public Task<(ErrorCode,string)> GetLatestClientVersionAsync();

	public Task<(ErrorCode, T)> GetMasterData<T>(string name);
	public Task<(ErrorCode, T)> GetMasterDataRow<T>(string rowName, int index);
	
	public Task<bool> SetUserReqLockAsync(string lockName);
	
	public Task<bool> DelUserReqLockAsync(string lockName);

}

