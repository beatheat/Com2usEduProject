// using Com2usEduProject.Databases;
// using Com2usEduProject.ReqRes;
// using Com2usEduProject.Tools;
// using Microsoft.AspNetCore.Mvc;
// using ZLogger;
//
// namespace Com2usEduProject.Controllers;
//
//
// [ApiController]
// [Route("[controller]")]
// public class LoadStageList
// {
// 	readonly IAccountDb _accountDb;
// 	readonly IMemoryDb _memoryDb;
// 	readonly IGameDb _gameDb;
// 	readonly ILogger<Login> _logger;
//
// 	public LoadStageList(ILogger<Login> logger, IAccountDb accountDb, IMemoryDb memoryDb, IGameDb gameDb)
// 	{
// 		_logger = logger;
// 		_accountDb = accountDb;
// 		_memoryDb = memoryDb;
// 		_gameDb = gameDb;
// 	}
//
// 	[HttpPost]
// 	public async Task<LoginResponse> Post(LoginRequest request)
// 	{
// 		
// 	}
// 	
// 	private void LogError(ErrorCode errorCode, object payload, string message)
// 	{
// 		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoginError],
// 			new {ErrorCode = errorCode, Payload = payload}, 
// 			message);
// 	}
// }