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
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceRewardError], new {ErrorCode = errorCode, PlayerId = request.PlayerId}, 
				"Player Data Loading Failed");
			response.Result = errorCode;
			return response;
		}
		
		// 최초 출석이 아닐 경우
		if (player.LastAttendanceDate != null)
		{
			// 이미 보상을 받은 경우
			if (DateTime.Today - player.LastAttendanceDate == TimeSpan.Zero)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceRewardError], 
					new {ErrorCode = ErrorCode.ReceiveAttendanceRewardAlready, PlayerId = request.PlayerId}, 
					"Player Already Received Attendance Reward");
				response.Result = ErrorCode.ReceiveAttendanceRewardAlready;
				return response;
			}
			// 연속 출석이 아닐 경우
			if (DateTime.Today - player.LastAttendanceDate > TimeSpan.FromDays(1))
			{
				_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceRewardError],
					new {PlayerData = player}, 
					"Player Attendance Date Initialized By Absent");

				player.ContinuousAttendanceDays = 0;
			}
		}

		// 모든 보상 수령했을 시
		if (player.ContinuousAttendanceDays == 30)
		{
			_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceRewardError],
				new {PlayerData = player}, 
				"Player Attendance Date Initialized By Received All Rewards");
			
			player.ContinuousAttendanceDays = 0;
		}
		
		// 연속출석보상 우편함에 추가
		(errorCode, var mailId) = await InsertAttendanceRewardMail(request.PlayerId, player.ContinuousAttendanceDays+1);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceRewardError], 
				new {ErrorCode = errorCode, }, "Player Already Received Attendance Reward");
		
			await Rollback(mailId);
			response.Result = errorCode;
			return response;
		}

		player.ContinuousAttendanceDays += 1;
		player.LastAttendanceDate = DateTime.Today;

		//출석일수 갱신
		errorCode = await _gameDb.PlayerTable.UpdateAsync(player);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceRewardError], 
				new {ErrorCode = errorCode, PlayerData = player }, "Update Player Data Failed");

			
			await Rollback(mailId);
			response.Result = errorCode;
			return response;
		}
		
		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceReward], 
			new {PlayerId = request.PlayerId, Day = player.ContinuousAttendanceDays-1},
			"Receive Attendance Reward Success");

		return response;
	}
	
	
	public async Task<(ErrorCode,int)> InsertAttendanceRewardMail(int playerId, int day)
	{
		var (errorCode, reward) = _masterDb.GetAttendanceReward(day);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceRewardError], 
				new {ErrorCode = errorCode, Day  = day}, "InsertAttendanceRewardMail - GetAttendanceReward Failed");
			return (errorCode, -1);
		}

		Mail mail = new Mail {
			PlayerId = playerId, 
			Name = $"{day}일차 출석보상", 
			Content = $"{day}일차 출석보상", 
			TransmissionDate = DateTime.Now,
			ExpireDate = DateTime.Now + TimeSpan.FromDays(30)
		};
		(errorCode, var mailId) = await _gameDb.MailTable.InsertAsync(mail);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceRewardError], 
				new {ErrorCode = errorCode, Mail  = mail}, "InsertAttendanceRewardMail - Insert Mail Failed");
			return (errorCode, -1);
		}

		MailItem mailItem = new MailItem
		{
			MailId = mailId,
			ItemCode = reward.ItemCode,
			ItemCount = reward.ItemCount
		};
		
		(errorCode, _) = await _gameDb.MailItemTable.InsertAsync(mailItem);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceRewardError], 
				new {ErrorCode = errorCode, Mail  = mail}, "InsertAttendanceRewardMail - Insert MailItem Failed");
			return (errorCode, -1);
		}
		
		return (ErrorCode.None, mailId);
	}
	
	public async Task Rollback(int mailId)
	{
		var errorCode = await _gameDb.MailTable.DeleteAsync(mailId);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceRewardError], 
				new {ErrorCode = errorCode, mailId = mailId}, "Rollback - Delete Mail Failed");
		}
		
		errorCode = await _gameDb.MailItemTable.DeleteAllAsync(mailId);
		if (errorCode != ErrorCode.None)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APIReceiveAttendanceRewardError], 
				new {ErrorCode = errorCode, mailId = mailId}, "Rollback - Delete MailItem Failed");
		}

	}
}