using Com2usEduProject.DBSchema;
using Com2usEduProject.Authorization;

namespace Com2usEduProject.Databases;

public interface IMemoryDb
{
	public void Init(string address);
    
	public Task<ErrorCode> RegisterUserAsync(int accountId, string authToken, int playerId);
	
	public Task<(ErrorCode, AuthUser)> GetUserAsync(int accountId);

	public Task<bool> SetUserRequestLockAsync(string lockName);
	
	public Task<bool> DelUserRequestLockAsync(string lockName);

	public Task<(bool, string)> GetNoticeAsync();
}

