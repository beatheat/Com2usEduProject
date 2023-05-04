﻿namespace Com2usEduProject.Databases;


public interface IAccountDb : IDisposable
{
	public Task<(ErrorCode,int)> InsertAccountAsync(string loginId, string password);
    
	public Task<(ErrorCode, int)> VerifyAccountAsync(string loginId, string password);

	public Task<ErrorCode> DeleteAccountAsync(string loginId);
}