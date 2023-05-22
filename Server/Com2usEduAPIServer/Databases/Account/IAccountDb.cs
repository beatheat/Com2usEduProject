namespace Com2usEduAPIServer.Databases;


public interface IAccountDb : IDisposable
{
	public Task<(ErrorCode,int)> InsertAsync(string loginId, string password);
    
	public Task<(ErrorCode, int)> VerifyAccountAsync(string loginId, string password);

	public Task<ErrorCode> DeleteAccountAsync(string loginId);
}