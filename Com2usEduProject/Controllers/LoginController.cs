using Com2usEduProject.ModelReqRes;
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
	readonly ILogger<Login> _logger;

	
	public Login(ILogger<Login> logger, IAccountDb accountDb, IMemoryDb memoryDb)
	{
		_logger = logger;
		_accountDb = accountDb;
		_memoryDb = memoryDb;
	}
	// GET
	[HttpPost]
	public async Task<PkLoginResponse> Post(PkLoginRequest request)
	{
		var response = new PkLoginResponse();

		// ID, PW 검증
		var (errorCode, accountId) = await _accountDb.VerifyAccountAsync(request.Id, request.Password);
		if (errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}

                
		var authToken = Security.CreateAuthToken();
		errorCode = await _memoryDb.RegisterUserAsync(request.Id, authToken, accountId);
		if(errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}

		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.Login], new { Id = request.Id, AuthToken = authToken, AccountId = accountId }, "Login Success"); 
        
		response.AuthToken = authToken; 
		return response;
	}
}