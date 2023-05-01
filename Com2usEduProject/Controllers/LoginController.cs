using Com2usEduProject.ReqRes;
using Com2usEduProject.Services;
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

		//TODO: 이중 로그인 막기 
		// ID, PW 검증
		var (errorCode, accountId) = await _accountDb.VerifyAccountAsync(request.Id, request.Password);
		if (errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}
		
		// auth 생성 및 등록
		var authToken = Security.CreateAuthToken();
		errorCode = await _memoryDb.RegisterUserAsync(request.Id, authToken, accountId);
		if(errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}

		// 플레이어 데이터 로드
		(errorCode, response.playerData) = await _gameDb.LoadPlayerDataAsync(accountId);
		if(errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}
		
		// 플레이어 아이템 데이터 로드
		(errorCode, response.playerItems) = await _gameDb.LoadPlayerItemsAsync(response.playerData.PlayerId);
		if(errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}

		// 공지사항 로드
		(var isNoticeExist, response.Notice) = await _memoryDb.GetNoticeAsync();
		if (!isNoticeExist)
		{
			response.Notice = null;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.Login], new { Id = request.Id, AuthToken = authToken, AccountId = accountId }, "Login Success"); 
        
		response.AuthToken = authToken; 
		return response;
	}
}