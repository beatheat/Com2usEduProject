using System.Data;
using Com2usEduProject.DBSchema;
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

	public AccountDb(ILogger<AccountDb> logger, IOptions<DbConnectionConfig> dbConfig)
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
		try
		{
			_dbConnection = new MySqlConnection(_dbConfig.Value.AccountDb);
			_dbConnection.Open();
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.AccountDbConnection], e,
				new {ConnectionString = _dbConfig.Value.AccountDb}, "AccountDB Connection Failed");
		}
	}
	
	public void Close()
	{
		_dbConnection.Close();
	}
	
	public async Task<(ErrorCode,int)> CreateAccountAsync(string id, string password)
	{
        try
        {
            var saltValue = Security.MakeSaltString();
            var hashingPassword = Security.MakeHashPassWord(saltValue, password);
            _logger.ZLogDebug($"[CreateAccount] Id: {id}, SaltValue : {saltValue}, HashingPassword:{hashingPassword}");
			
            var accountId = await _queryFactory.Query("account").InsertGetIdAsync<int>(new {Id = id, SaltValue = saltValue, HashedPassword = hashingPassword});

            return (ErrorCode.None,accountId);
        }
        catch(MySqlException e)
        {
	        if (e.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
	        {
		        _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.InsertAccount], e, new {AccountId = id, ErrorCode = ErrorCode.CreateAccountDuplicate}, "Insert Account Duplicated");
		        return (ErrorCode.CreateAccountDuplicate, -1);
	        }
	        _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.InsertAccount], e, new {AccountId = id, ErrorCode = ErrorCode.CreateAccountFailException}, "Insert Account Failed");
	        return (ErrorCode.CreateAccountFailException, -1);
        }
        catch (Exception e)
        {
	        _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.InsertAccount], e, new {AccountId = id, ErrorCode = ErrorCode.CreateAccountFailException}, "Insert Account Failed");
	        return (ErrorCode.CreateAccountFailException, -1);
        }	
	}
	
	public async Task<(ErrorCode, int)> VerifyAccountAsync(string id, string password)
	{
		try
		{
			var accountInfo = await _queryFactory.Query("account").Where("Id", id).FirstOrDefaultAsync<Account>();

			if (accountInfo is null || accountInfo.AccountId == 0)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.VerifyAccount], new {AccountId = id, ErrorCode = ErrorCode.LoginFailUserNotExist}, "Account Id Not Exist");
				return (ErrorCode.LoginFailUserNotExist, -1);
			}   
           
			var hashingPassword = Security.MakeHashPassWord(accountInfo.SaltValue, password);
			if (accountInfo.HashedPassword != hashingPassword)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.VerifyAccount], new {AccountId = id, ErrorCode = ErrorCode.LoginFailPwNotMatch}, "Password Not Match");
				return (ErrorCode.LoginFailPwNotMatch, -1);
			}

			return (ErrorCode.None, accountInfo.AccountId);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.VerifyAccount], new {AccountId = id, ErrorCode = ErrorCode.LoginFailException}, "Select Account Fail Exception");
			return (ErrorCode.LoginFailException, -1);
		}	
	}
}