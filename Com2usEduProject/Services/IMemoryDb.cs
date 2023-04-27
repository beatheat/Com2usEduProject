namespace Com2usEduProject.Services;

public interface IMemoryDb
{
	public void Init(string address);
    
	public Task<ErrorCode> RegisterUserAsync(string id, string authToken, long accountId);

	public Task<ErrorCode> CheckUserAuthAsync(string id, string authToken);
	
	// public Task<(bool, AuthUser)> GetUserAsync(string id);
	//
	// public Task<bool> SetUserStateAsync(AuthUser user, UserState userState);
	//
	// public Task<bool> SetUserReqLockAsync(string key);
	//
	// public Task<bool> DelUserReqLockAsync(string key);

}

