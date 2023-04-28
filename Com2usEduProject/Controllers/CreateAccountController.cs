using Com2usEduProject.ModelReqRes;
using Microsoft.AspNetCore.Mvc;
using Com2usEduProject.Services;
using Com2usEduProject.Tools;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateAccount : ControllerBase
{
	ILogger<CreateAccount> _logger;
	IAccountDb _accountDB;
	
	public CreateAccount(ILogger<CreateAccount> logger, IAccountDb accountDb)
	{
		_logger = logger;
		_accountDB = accountDb;
	}
	// GET
	[HttpPost]
	public async Task<PkCreateAccountRes> Post(PkCreateAccountReq request)
	{
		var response = new PkCreateAccountRes();
        
		var errorCode = await _accountDB.CreateAccountAsync(request.Id, request.Password);
		if (errorCode != ErrorCode.None)
		{
			response.Result = errorCode;
			return response;
		}

		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.CreateAccount], new { Id = request.Id }, "CreateAccount Success");
		return response;
	}
}