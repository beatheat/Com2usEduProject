using Com2usEduProject.ReqRes;
using Com2usEduProject.Databases;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class Login : ControllerBase
{
	readonly IAccountDb _accountDb;
	readonly IMemoryDb _memoryDb;
	readonly IGameDb _gameDb;
	readonly ILogger<Login> _logger;

	public Login(ILogger<Login> logger, IAccountDb accountDb, IMemoryDb memoryDb, IGameDb gameDb)
	{
		_logger = logger;
		_accountDb = accountDb;
		_memoryDb = memoryDb;
		_gameDb = gameDb;
	}
	
	[HttpPost]
	public async Task<LoginResponse> Post(LoginRequest request)
	{
		var response = new LoginResponse();

		// ID, PW 검증
		var (errorCode, accountId) = await _accountDb.VerifyAccountAsync(request.LoginId, request.Password);
		if (errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}
		
		// 플레이어 데이터 로드
		(errorCode, response.Player) = await _gameDb.PlayerTable.SelectByAccountIdAsync(accountId);
		if(errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoginError], new {ErrorCode = errorCode, AccountId = accountId}, 
				"Select Player Fail");
			
			response.Result = errorCode;
			return response;
		}

		// Auth 생성 및 등록
		var authToken = Security.CreateAuthToken();
		errorCode = await _memoryDb.RegisterUserAsync(accountId, authToken, response.Player.Id);
		if(errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoginError], new {ErrorCode = errorCode, LoginId = accountId}, 
				"Register User Auth Fail");
			
			response.Result = errorCode;
			return response;
		}


		// 공지사항 로드
		(var isNoticeExist, response.Notice) = await _memoryDb.GetNoticeAsync();
		if (!isNoticeExist)
		{
			response.Notice = null;
		}
		
		
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APILogin], new { LoginId = request.LoginId, AuthToken = authToken }, "Login Success");

		response.AccountId = accountId;
		response.AuthToken = authToken; 
		
		return response;
	}
}