using Com2usEduProject.DBSchema;

namespace Com2usEduProject.Services;

public interface IMemoryDb
{
	public void Init(string address);
    
	public Task<ErrorCode> RegisterUserAsync(string id, string authToken, int accountId);
	
	public Task<(ErrorCode, AuthUser)> GetUserAsync(string accountId);

	public Task<bool> SetUserRequestLockAsync(string lockName);
	
	public Task<bool> DelUserRequestLockAsync(string lockName);

}

