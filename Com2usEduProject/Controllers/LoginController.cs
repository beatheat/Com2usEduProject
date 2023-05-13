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
			LogError(errorCode, request, "Verify Account Fail");
			response.Result = errorCode;
			return response;
		}
		
		// 플레이어 데이터 로드
		(errorCode, var player) = await _gameDb.PlayerTable.SelectByAccountIdAsync(accountId);
		if(errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Select Player Fail");
			response.Result = errorCode;
			return response;
		}

		// Auth 생성 및 등록
		var authToken = Security.CreateAuthToken();
		errorCode = await _memoryDb.AuthManager.RegisterUserAsync(accountId, authToken, player.Id);
		if(errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Register User Auth Fail");
			response.Result = errorCode;
			return response;
		}
		
		// 공지사항 로드
		var (isNoticeExist, notice) = await _memoryDb.NoticeManager.GetNoticeAsync();
		if (!isNoticeExist)
		{
			notice = null;
		}
		
		response.AccountId = accountId;
		response.AuthToken = authToken;
		response.Player = player;
		response.Notice = notice;
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APILogin], new { LoginId = request.LoginId, AuthToken = authToken }, "Login Success");
		
		return response;
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoginError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}