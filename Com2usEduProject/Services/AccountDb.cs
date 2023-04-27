using System.Data;
using Com2usEduProject.ModelDB;
using Com2usEduProject.Tools;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;
using ZLogger;

namespace Com2usEduProject.Services;

public class AccountDb : IAccountDb
{
	readonly IOptions<DbConnectionConfig> _dbConfig;
	readonly ILogger<AccountDb> _logger;

	IDbConnection _dbConnection;

	readonly QueryFactory _queryFactory;

	public AccountDb( ILogger<AccountDb> logger, IOptions<DbConnectionConfig> dbConfig)
	{
		_dbConfig = dbConfig;
		_logger = logger;

		Open();
		
		_queryFactory = new QueryFactory(_dbConnection, new MySqlCompiler());
	}


	public void Dispose()
	{
		Close();
	}

	private void Open()
	{
		_dbConnection = new MySqlConnection(_dbConfig.Value.AccountDb);
		_dbConnection.Open();
	}
	
	public void Close()
	{
		_dbConnection.Close();
	}
	
	public async Task<ErrorCode> CreateAccountAsync(string id, string password)
	{
        try
        {
            var saltValue = Security.MakeSaltString();
            var hashingPassword = Security.MakeHashPassWord(saltValue, password);
            _logger.ZLogDebug($"[CreateAccount] Id: {id}, SaltValue : {saltValue}, HashingPassword:{hashingPassword}");
			
            var count = await _queryFactory.Query("account").InsertAsync(new {Id = id, SaltValue = saltValue, HashedPassword = hashingPassword});            
            
            if(count != 1)
            {
                return ErrorCode.CreateAccountFailInsert;
            }
            
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[AccountDb.CreateAccount] ErrorCode: {ErrorCode.CreateAccountFailException}, Id: {id}");
            return ErrorCode.CreateAccountFailException;
        }	
	}


	public async Task<Tuple<ErrorCode, long>> VerifyAccountAsync(string id, string password)
	{
		try
		{
			var accountInfo = await _queryFactory.Query("account").Where("Id", id).FirstOrDefaultAsync<Account>();

			if (accountInfo is null || accountInfo.AccountId == 0)
			{
				return new Tuple<ErrorCode, long>(ErrorCode.LoginFailUserNotExist, 0);
			}   
           
			var hashingPassword = Security.MakeHashPassWord(accountInfo.SaltValue, password);
			if (accountInfo.HashedPassword != hashingPassword)
			{
				_logger.ZLogError($"[AccountDb.VerifyAccount] ErrorCode: {ErrorCode.LoginFailPwNotMatch}, Id: {id}");
				return new Tuple<ErrorCode, long>(ErrorCode.LoginFailPwNotMatch, 0);
			}

			return new Tuple<ErrorCode, long>(ErrorCode.None, accountInfo.AccountId);
		}
		catch (Exception e)
		{
			_logger.ZLogError(e, $"[AccountDb.VerifyAccount] ErrorCode: {ErrorCode.LoginFailException}, Id: {id}");
			return new Tuple<ErrorCode, long>(ErrorCode.LoginFailException, 0);
		}	
	}
}