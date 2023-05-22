using Com2usEduAPIServer.Databases.Schema;
using Com2usEduAPIServer.Tools;
using SqlKata.Execution;
using ZLogger;

namespace Com2usEduAPIServer.Databases;

public class PlayerAttendanceTable
{
	readonly QueryFactory _queryFactory;
	readonly ILogger<GameDb> _logger;
	
	public PlayerAttendanceTable(QueryFactory queryFactory, ILogger<GameDb> logger)
	{
		_queryFactory = queryFactory;
		_logger = logger;
	}
	
	public async Task<(ErrorCode, int)> InsertAsync(int playerId)
	{
		try
		{
			var playerAttendanceId = await _queryFactory.Query("PlayerAttendance").InsertGetIdAsync<int>(new {PlayerId = playerId});
			return (ErrorCode.None, playerAttendanceId);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerAttendanceInsertError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerAttendanceInsertFailException}, "Insert PlayerAttendance Fail");
			return (ErrorCode.PlayerItemInsertFailException, -1);
		}
	}
	
	public async Task<(ErrorCode, PlayerAttendance)> SelectAsync(int playerId)
	{
		try
		{
			var playerAttendance = await _queryFactory.Query("PlayerAttendance").Where("PlayerId", playerId).FirstAsync<PlayerAttendance>();
			return (ErrorCode.None, playerAttendance);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerAttendanceSelectError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerAttendanceSelectFailException}, "Select PlayerAttendance Fail");
			return (ErrorCode.PlayerAttendanceSelectFailException, new PlayerAttendance());
		}
	}
	
	public async Task<ErrorCode> UpdateAsync(PlayerAttendance playerAttendance)
	{
		try
		{
			int count = await _queryFactory.Query("PlayerAttendance").Where("PlayerId", playerAttendance.PlayerId).UpdateAsync(playerAttendance);
			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerAttendanceUpdateError], 
					new {PlayerAttendance = playerAttendance, ErrorCode = ErrorCode.PlayerAttendanceUpdateFail}, "Update PlayerAttendance Fail");
				return ErrorCode.PlayerAttendanceUpdateFail;

			}
			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerAttendanceUpdateError], e,
				new {PlayerAttendance = playerAttendance, ErrorCode = ErrorCode.PlayerAttendanceUpdateFailException}, "Update PlayerAttendance Fail");
			return ErrorCode.PlayerAttendanceUpdateFailException;
		}
	}
}