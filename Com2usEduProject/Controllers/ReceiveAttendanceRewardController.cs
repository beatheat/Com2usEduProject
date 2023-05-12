using Com2usEduProject.Databases;
using Com2usEduProject.DBSchema;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceiveAttendanceReward
{
	readonly ILogger<ReceiveAttendanceReward> _logger;
	readonly IAccountDb _accountDb;
	readonly IGameDb _gameDb;
	readonly IMasterDb _masterDb;
	
	public ReceiveAttendanceReward(ILogger<ReceiveAttendanceReward> logger, IAccountDb accountDb, IGameDb gameDb, IMasterDb masterDb)
	{
		_logger = logger;
		_accountDb = accountDb;
		_gameDb = gameDb;
		_masterDb = masterDb;
	}

	[HttpPost]
	public async Task<ReceiveAttendanceRewardResponse> Post(ReceiveAttendanceRewardRequest request)
	{
		var response = new ReceiveAttendanceRewardResponse();

		//플레이어 데이터 로드
		var (errorCode, player) = await _gameDb.PlayerTable.SelectAsync(request.PlayerId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Select Player Fail");
			response.Result = errorCode;
			return response;
		}
		
		// 이미 보상을 받은 경우
		if (DateTime.Today - player.LastAttendanceDate == TimeSpan.Zero)
		{
			errorCode = ErrorCode.ReceiveAttendanceRewardAlready;
			LogError(errorCode, request, "Player Already Received Attendance Reward");
			response.Result = errorCode;
			return response;
		}
		// 연속 출석이 아닐 경우
		if (DateTime.Today - player.LastAttendanceDate > TimeSpan.FromDays(1))
		{
			player.ContinuousAttendanceDays = 0;
		}
		
		// 연속출석보상 우편함에 추가
		(errorCode, var mailId) = await InsertAttendanceRewardItemToMail(request.PlayerId, player.ContinuousAttendanceDays+1);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Insert Attendance Reward Item To Player Mail");
			response.Result = errorCode;
			return response;
		}

		//출석일수 갱신
		player.ContinuousAttendanceDays = (player.ContinuousAttendanceDays + 1) % 30;
		player.LastAttendanceDate = DateTime.Today;

		errorCode = await _gameDb.PlayerTable.UpdateAsync(player);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, player, "Update Player Fail");
			await Rollback(mailId);
			response.Result = errorCode;
			return response;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceReward], 
			new {PlayerId = request.PlayerId, Day = player.ContinuousAttendanceDays-1},
			"Receive Attendance Reward Success");

		return response;
	}
	
	
	
	public async Task<(ErrorCode,int)> InsertAttendanceRewardItemToMail(int playerId, int day)
	{
		// 출석보상 로드
		var (errorCode, reward) = _masterDb.GetAttendanceReward(day);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, new {Day = day}, "InsertAttendanceRewardMail - GetAttendanceReward Fail");
			return (errorCode, -1);
		}

		// 출석보상 메일 생성
		Mail mail = new Mail {
			PlayerId = playerId, 
			Name = $"{day}일차 출석보상", 
			Content = $"{day}일차 출석보상", 
			TransmissionDate = DateTime.Now,
			ExpireDate = DateTime.Now + TimeSpan.FromDays(30)
		};
		
		mail.AddItem(reward.ItemCode, reward.ItemCount);
		
		// 출석보상 메일 삽입
		(errorCode, var mailId) = await _gameDb.MailTable.InsertAsync(mail);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, new {Mail = mail}, "InsertAttendanceRewardMail - Insert Mail Fail" );
			return (errorCode, -1);
		}
		
		return (ErrorCode.None, mailId);
	}
	
	public async Task Rollback(int mailId)
	{
		var errorCode = await _gameDb.MailTable.DeleteAsync(mailId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, new {MailId = mailId}, "Rollback - Delete Mail Failed");
		}
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceRewardError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}