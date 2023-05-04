using Com2usEduProject.DBSchema;
using Com2usEduProject.Tools;
using SqlKata.Execution;
using ZLogger;

namespace Com2usEduProject.Databases;

public class MailItemTable
{
	readonly QueryFactory _queryFactory;
	readonly ILogger<GameDb> _logger;
	
	
	public MailItemTable(QueryFactory queryFactory, ILogger<GameDb> logger)
	{
		_queryFactory = queryFactory;
		_logger = logger;
	}

	public async Task<(ErrorCode, int)> InsertAsync(MailItem mailItem)
	{
		try
		{
			var mailItemId = await _queryFactory.Query("MailItem").InsertGetIdAsync<int>(mailItem);

			_logger.ZLogDebug($"[InsertAsync] MailItemId : {mailItem.MailId}, MailItemId : {mailItemId}");
			return (ErrorCode.None, mailItemId);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.MailItemInsertError], e, 
				new {MailItem = mailItem, ErrorCode = ErrorCode.MailItemInsertFailException}, "Insert MailItem Failed");
			return (ErrorCode.MailItemInsertFailException, -1);
		}
	}


	public async Task<(ErrorCode, MailItem)> SelectAsync(int mailItemId)
	{
		try
		{
			var mailItem = await _queryFactory.Query("MailItem").Where("Id", mailItemId).FirstAsync<MailItem>();
			
			_logger.ZLogDebug($"[SelectAsync] MailItemId: {mailItemId}");
			return (ErrorCode.None, mailItem);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.MailSelectError], e,
				new {MailItemId = mailItemId, ErrorCode = ErrorCode.MailItemSelectFailException}, "Insert MailItem Failed");
			return (ErrorCode.MailItemSelectFailException, new MailItem());
		}

	}
	
	public async Task<(ErrorCode, IList<MailItem>)> SelectListAsync(int mailId)
	{
		try
		{
			var mailItems = await _queryFactory.Query("MailItem").Where("MailId", mailId).GetAsync<MailItem>();
			
			_logger.ZLogDebug($"[SelectListAsync] MailId: {mailId}");
			
			return (ErrorCode.None, mailItems.ToList());
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.MailSelectListError], e,
				new {MailId = mailId, ErrorCode = ErrorCode.MailSelectFailException}, "Select MailItem Fail");
			return (ErrorCode.MailSelectFailException, new List<MailItem>());
		}
	}
	
	public async Task<ErrorCode> DeleteAllAsync(int mailId)
	{
		try
		{
			await _queryFactory.Query("MailItem").Where("MailId", mailId).DeleteAsync();
			
			_logger.ZLogDebug($"[DeleteAllAsync] MailId: {mailId}");
			
			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.MailDeleteError], e,
				new {MailId = mailId, ErrorCode = ErrorCode.MailDeleteFailException}, "Delete MailItem Fail");
			return ErrorCode.MailDeleteFailException;
		}
	}
}