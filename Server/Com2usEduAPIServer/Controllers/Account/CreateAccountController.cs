using Com2usEduAPIServer.Databases;
using Com2usEduAPIServer.Databases.Schema;
using Com2usEduAPIServer.GameLogic;
using Com2usEduAPIServer.ReqRes;
using Com2usEduAPIServer.Tools;
using Microsoft.AspNetCore.Mvc;
using Com2usEduAPIServer.Databases.Schema.Extension;
using ZLogger;

namespace Com2usEduAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateAccount : ControllerBase
{
	readonly ILogger<CreateAccount> _logger;
	readonly IAccountDb _accountDb;
	readonly IGameDb _gameDb;
	readonly IMasterDb _masterDb;
	
	public CreateAccount(ILogger<CreateAccount> logger, IAccountDb accountDb, IGameDb gameDb, IMasterDb masterDb)
	{
		_logger = logger;
		_accountDb = accountDb;
		_gameDb = gameDb;
		_masterDb = masterDb;
	}
	
	[HttpPost]
	public async Task<CreateAccountResponse> Post(CreateAccountRequest request)
	{
		var response = new CreateAccountResponse();
        
		// 계정 생성
		var (errorCode,accountId) = await _accountDb.InsertAsync(request.LoginId, request.Password);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode,request, "Insert Account Fail");
			response.Result = errorCode;
			return response;
		}

		// 플레이어 기본 데이터 생성
		(errorCode, var playerId) = await _gameDb.PlayerTable.InsertAsync(accountId, request.LoginId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode,request,"Create Player Fail");
			await Rollback(request.LoginId, playerId);
			response.Result = errorCode;
			return response;
		}

		//플레이어 출석 데이터 생성
		(errorCode,_) = await _gameDb.PlayerAttendanceTable.InsertAsync(playerId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode,request,"Create PlayerAttendance Fail");
			await Rollback(request.LoginId, playerId);
			response.Result = errorCode;
			return response;
		}
		
		// 플레이어 기본 아이템 생성
		errorCode = await InsertInitialPlayerItem(playerId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode,request,"Create Player Initial Item Fail");
			await Rollback(request.LoginId, playerId);
			response.Result = errorCode;
			return response;
		}

		// 메일함 테스트용 테스트 메일 삽입
		errorCode = await InsertTestMails(playerId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode,request,"Player Test Mail Creation Fail");
			response.Result = errorCode;
			return response;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APICreateAccount], new {LoginId = request.LoginId}, "CreateAccount Success");
		return response;
	}

	private async Task<ErrorCode> InsertInitialPlayerItem(int playerId)
	{
		var initialPlayerItemList = _masterDb.GetInitialPlayerItem();
		foreach (var item in initialPlayerItemList)
		{
			var (_,itemInfo) = _masterDb.GetItem(item.ItemCode);
			var playerItemReceiver = new PlayerItemReceiver(_logger, _masterDb, _gameDb);
			var errorCode = await playerItemReceiver.Receive(playerId, new ItemBundle {ItemCode = itemInfo.Code, ItemCount = item.ItemCount});
			if (errorCode != ErrorCode.None)
			{
				return errorCode;
			}
		}
		return ErrorCode.None;
	}

	private async Task<ErrorCode> InsertTestMails(int playerId)
	{
		Random random = new Random();
		for (int i = 0; i < 1000; i++)
		{
			Mail mail = new Mail();
			
			mail.PlayerId = playerId;
			mail.Name = $"테스트 메일 ({i})";
			mail.ExpireDate = DateTime.Now + TimeSpan.FromDays(7);
			mail.TransmissionDate = DateTime.Now;
			mail.Content = $"Lorem ipsum~~~{i}";
			mail.IsItemReceived = false;
			
			for (int j = 0; j < 4; j++)
			{
				int itemCode = random.Next(1,6);
				int itemCount = itemCode switch
				{
					1 => random.Next(1000),
					6 => random.Next(10),
					_ => 1
				};
				mail.AddItem(itemCode, itemCount);
			}

			var (errorCode,_) = await _gameDb.MailTable.InsertAsync(mail);
			if (errorCode != ErrorCode.None)
				return errorCode;
		}
	
		return ErrorCode.None;
	}

	private async Task Rollback(string loginId, int playerId)
	{
		var errorCode = await _accountDb.DeleteAccountAsync(loginId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, new {LoginId = loginId}, "Rollback - Delete Account Failed");
		}
		
		errorCode = await _gameDb.PlayerTable.DeleteAsync(playerId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, new {PlayerId = playerId}, "Rollback - Delete Player Failed");
		}

		errorCode = await _gameDb.PlayerItemTable.DeleteAllAsync(playerId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, new {PlayerId = playerId}, "Rollback - Delete PlayerItem Failed");
		}
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APICreateAccountError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}