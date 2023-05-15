using Com2usEduProject.Databases.Schema;
using Com2usEduProject.Tools;
using MySqlConnector;
using SqlKata.Execution;
using ZLogger;

namespace Com2usEduProject.Databases;

public class PlayerCompletedStageTable
{
	readonly QueryFactory _queryFactory;
	readonly ILogger<GameDb> _logger;
	
	public PlayerCompletedStageTable(QueryFactory queryFactory, ILogger<GameDb> logger)
	{
		_queryFactory = queryFactory;
		_logger = logger;
	}

	public async Task<ErrorCode> InsertAsync(int playerId, int stageCode)
	{
		var playerCompletedStage = new PlayerCompletedStage
		{
			PlayerId = playerId,
			StageCode = stageCode
		};
		try
		{
			var count = await _queryFactory.Query("PlayerCompletedStage").InsertAsync(playerCompletedStage);
			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerCompletedStageInsertError], 
					new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerCompletedStageInsertFail}, "Insert PlayerCompletedStage Fail");
				return ErrorCode.PlayerCompletedStageInsertFail;
			}
			return ErrorCode.None;
		}
		catch (MySqlException e)
		{
			if (e.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
			{
				return ErrorCode.PlayerCompletedStageInsertDuplicate;
			}
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerCompletedStageInsertError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerCompletedStageInsertFailException}, "Insert PlayerCompletedStage Fail");
			return ErrorCode.PlayerCompletedStageInsertFailException;
		}
	}
	
	public async Task<(ErrorCode,IList<int>)> SelectListAsync(int playerId)
	{
		try
		{
			var completedStageCodes = await _queryFactory.Query("PlayerCompletedStage").Select("StageCode").Where("PlayerId", playerId).GetAsync<int>();
			return (ErrorCode.None, completedStageCodes.ToList());
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.PlayerCompletedStageSelectError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerCompletedStageSelectFailException}, "Select PlayerAttendance Fail");
			return (ErrorCode.PlayerCompletedStageSelectFailException, new List<int>());
		}
	}
}