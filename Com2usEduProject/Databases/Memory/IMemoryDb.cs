using Com2usEduProject.DBSchema;
using Com2usEduProject.Authorization;

namespace Com2usEduProject.Databases;

public interface IMemoryDb
{
	public void Init(string address);
    
	public Task<ErrorCode> RegisterUserAsync(string id, string authToken, int accountId);
	
	public Task<(ErrorCode, AuthUser)> GetUserAsync(string accountId);

	public Task<bool> SetUserRequestLockAsync(string lockName);
	
	public Task<bool> DelUserRequestLockAsync(string lockName);

	public Task<(bool, string)> GetNoticeAsync();
}

