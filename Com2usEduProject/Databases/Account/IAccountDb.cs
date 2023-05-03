namespace Com2usEduProject.Databases;


public interface IAccountDb : IDisposable
{
	public Task<(ErrorCode,int)> InsertAccountAsync(string id, string password);
    
	public Task<(ErrorCode, int)> VerifyAccountAsync(string id, string password);

	public Task<ErrorCode> DeleteAccountAsync(string id);
}