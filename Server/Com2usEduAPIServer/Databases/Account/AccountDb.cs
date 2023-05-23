using System.Data;
using Com2usEduAPIServer.Databases.Schema;
using Com2usEduAPIServer.Tools;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;
using ZLogger;

namespace Com2usEduAPIServer.Databases;

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
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.AccountDbConnectionError], e,
				new {ConnectionString = _dbConfig.Value.AccountDb}, "AccountDB Connection Failed");
		}
	}
	
	public void Close()
	{
		_dbConnection.Close();
	}


	public async Task<(ErrorCode,int)> InsertAsync(string loginId, string password)
	{
        try
        {
            var saltValue = Security.MakeSaltString();
            var hashingPassword = Security.MakeHashPassWord(saltValue, password);
	        
            var accountId = await _queryFactory.Query("account").InsertGetIdAsync<int>(new {LoginId = loginId, SaltValue = saltValue, HashedPassword = hashingPassword});

            _logger.ZLogDebug($"[InsertAccountAsync] LoginId: {loginId}, SaltValue : {saltValue}, HashingPassword:{hashingPassword}");
            

            return (ErrorCode.None,accountId);
        }
        catch(MySqlException e)
        {
	        if (e.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
	        {
		        _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.InsertAccountError], e,
			        new {AccountId = loginId, ErrorCode = ErrorCode.InsertAccountDuplicate}, "Insert Account Duplicate");
		        return (ErrorCode.InsertAccountDuplicate, -1);
	        }
	        _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.InsertAccountError], e, 
		        new {AccountId = loginId, ErrorCode = ErrorCode.
		        InsertAccountFailException}, "Insert Account Failed");
	        return (ErrorCode.InsertAccountFailException, -1);
        }
        catch (Exception e)
        {
	        _logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.InsertAccountError], e, 
		        new {AccountId = loginId, ErrorCode = ErrorCode.InsertAccountFailException}, "Insert Account Fail");
	        return (ErrorCode.InsertAccountFailException, -1);
        }	
	}
	
	public async Task<ErrorCode> DeleteAccountAsync(string loginId)
	{
		try
		{
	        
			var accountId = await _queryFactory.Query("account").Where("LoginId", loginId).DeleteAsync();
			_logger.ZLogDebug($"[DeleteAccountAsync] LoginId: {loginId}");

			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.DeleteAccountError], e,
				new {LoginId = loginId, ErrorCode = ErrorCode.DeleteAccountFailException}, "Delete Account Fail");
			return ErrorCode.DeleteAccountFailException;
		}	
	}
	
	public async Task<(ErrorCode, int)> VerifyAccountAsync(string loginId, string password)
	{
		try
		{
			var accountInfo = await _queryFactory.Query("account").Where("LoginId", loginId).FirstOrDefaultAsync<Account>();

			if (accountInfo is null || accountInfo.Id == 0)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.VerifyAccountError], 
					new {AccountId = loginId, ErrorCode = ErrorCode.VerifyAccountFailAccountNotExist}, "Account Id Not Exist");
				return (ErrorCode.VerifyAccountFailAccountNotExist, -1);
			}   
           
			var hashingPassword = Security.MakeHashPassWord(accountInfo.SaltValue, password);
			if (accountInfo.HashedPassword != hashingPassword)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.VerifyAccountError], 
					new {AccountId = loginId, ErrorCode = ErrorCode.VerifyAccountFailPasswordNotMatch}, "Password Not Match");
				return (ErrorCode.VerifyAccountFailPasswordNotMatch, -1);
			}
			
			_logger.ZLogDebug($"[VerifyAccountAsync] LoginId: {loginId}");


			return (ErrorCode.None, accountInfo.Id);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.VerifyAccountError], e,
				new {AccountId = loginId, ErrorCode = ErrorCode.VerifyAccountFailException}, "Verify Account Fail Exception");
			return (ErrorCode.VerifyAccountFailException, -1);
		}	
	}
}