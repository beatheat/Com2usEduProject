namespace Com2usEduProject.Services;


public interface IAccountDb : IDisposable
{
	public Task<ErrorCode> CreateAccountAsync(string id, string password);
    
	public Task<Tuple<ErrorCode, long>> VerifyAccountAsync(string id, string password);
}