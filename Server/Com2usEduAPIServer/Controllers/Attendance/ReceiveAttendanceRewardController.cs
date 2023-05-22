using Com2usEduAPIServer.Databases;
using Com2usEduAPIServer.Databases.Schema;
using Com2usEduAPIServer.ReqRes;
using Com2usEduAPIServer.Tools;
using Com2usEduAPIServer.Databases.Schema.Extension;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceiveAttendanceReward : ControllerBase
{
	readonly ILogger<ReceiveAttendanceReward> _logger;
	readonly IGameDb _gameDb;
	readonly IMasterDb _masterDb;
	
	public ReceiveAttendanceReward(ILogger<ReceiveAttendanceReward> logger, IGameDb gameDb, IMasterDb masterDb)
	{
		_logger = logger;
		_gameDb = gameDb;
		_masterDb = masterDb;
	}

	[HttpPost]
	public async Task<ReceiveAttendanceRewardResponse> Post(ReceiveAttendanceRewardRequest request)
	{
		var response = new ReceiveAttendanceRewardResponse();

		//플레이어 출석부 데이터 로드
		var (errorCode, attendanceInfo) = await _gameDb.PlayerAttendanceTable.SelectAsync(request.PlayerId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Select PlayerAttendance Fail");
			response.Result = errorCode;
			return response;
		}
		
		// 연속출석보상 우편함에 추가
		(errorCode, var mailId) = await InsertAttendanceRewardItemToMail(attendanceInfo);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Insert Attendance Reward Item To Player Mail");
			response.Result = errorCode;
			return response;
		}

		//출석일수 갱신
		attendanceInfo.ContinuousAttendanceDays = (attendanceInfo.ContinuousAttendanceDays + 1) % 30;
		attendanceInfo.LastAttendanceDate = DateTime.Today;

		errorCode = await _gameDb.PlayerAttendanceTable.UpdateAsync(attendanceInfo);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, attendanceInfo, "Update Player Fail");
			await Rollback(mailId);
			response.Result = errorCode;
			return response;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceReward], 
			new {PlayerId = request.PlayerId, Day = attendanceInfo.ContinuousAttendanceDays-1},
			"Receive Attendance Reward Success");

		return response;
	}
	
	
	
	public async Task<(ErrorCode,int)> InsertAttendanceRewardItemToMail(PlayerAttendance attendanceInfo)
	{
		var errorCode = ErrorCode.None;
		// 이미 보상을 받은 경우
		if (DateTime.Today - attendanceInfo.LastAttendanceDate == TimeSpan.Zero)
		{
			errorCode = ErrorCode.ReceiveAttendanceRewardAlready;
			LogError(errorCode, new { PlayerAttendance = attendanceInfo }, "Player Already Received Attendance Reward");
			return (errorCode, -1);
		}
		// 연속 출석이 아닐 경우
		if (DateTime.Today - attendanceInfo.LastAttendanceDate > TimeSpan.FromDays(1))
		{
			attendanceInfo.ContinuousAttendanceDays = 0;
		}

		var rewardDay = attendanceInfo.ContinuousAttendanceDays + 1;
		
		// 출석보상 로드
		(errorCode, var reward) = _masterDb.GetAttendanceReward(rewardDay);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, new {RewardDay = rewardDay}, "InsertAttendanceRewardMail - GetAttendanceReward Fail");
			return (errorCode, -1);
		}

		// 출석보상 메일 생성
		Mail mail = new Mail {
			PlayerId = attendanceInfo.PlayerId, 
			Name = $"{rewardDay}일차 출석보상", 
			Content = $"{rewardDay}일차 출석보상", 
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