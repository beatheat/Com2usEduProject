using Com2usEduProject.Databases;
using Com2usEduProject.ReqRes;
using Com2usEduProject.Tools;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace Com2usEduProject.Controllers;

[ApiController]
[Route("[controller]")]
public class LoadAttendanceInfo
{
	readonly ILogger<LoadAttendanceInfo> _logger;
	readonly IGameDb _gameDb;
	readonly IMasterDb _masterDb;
	 
	public LoadAttendanceInfo(ILogger<LoadAttendanceInfo> logger, IGameDb gameDb, IMasterDb masterDb)
	{
		_logger = logger;
		_gameDb = gameDb;
		_masterDb = masterDb;
	}

	[HttpPost]
	public async Task<LoadAttendanceInfoResponse> Post(LoadAttendanceInfoRequest request)
	{
		var response = new LoadAttendanceInfoResponse();

		var (errorCode, attendanceInfo) = await _gameDb.PlayerAttendanceTable.SelectAsync(request.PlayerId);
		if (errorCode != ErrorCode.None)
		{
			LogError(errorCode, request, "Select Player Attendance Fail");
			response.Result = errorCode;
			return response;
		}

		response.AttendanceInfo = attendanceInfo;

		_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.APILoadAttendanceInfo], 
			new {PlayerId = request.PlayerId},
			"Load Attendance Info Success");
		
		return response;
	}
	
	private void LogError(ErrorCode errorCode, object payload, string message)
	{
		_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.APILoadAttendanceInfoError],
			new {ErrorCode = errorCode, Payload = payload}, 
			message);
	}
}