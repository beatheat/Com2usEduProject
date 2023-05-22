using Com2usEduAPIServer.Databases.Schema;
using Com2usEduAPIServer.Tools;
using SqlKata.Execution;
using ZLogger;

namespace Com2usEduAPIServer.Databases;

public class BillTable
{
	readonly QueryFactory _queryFactory;
	readonly ILogger<GameDb> _logger;
	
	
	public BillTable(QueryFactory queryFactory, ILogger<GameDb> logger)
	{
		_queryFactory = queryFactory;
		_logger = logger;
	}


	public async Task<(ErrorCode, Bill)> SelectAsync(long billToken)
	{
		try
		{
			var bill = await _queryFactory.Query("Bill").Where("Token", billToken).FirstAsync<Bill>();
			return (ErrorCode.None, bill);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.BillSelectError], e,
				new {BillToken = billToken, ErrorCode = ErrorCode.BillSelectFailException}, "Select Bill Failed");
			return (ErrorCode.BillSelectFailException, new Bill());
		}
	}
	
	public async Task<(ErrorCode, int)> InsertAsync(Bill bill)
	{
		try
		{
			var billId = await _queryFactory.Query("Bill").InsertGetIdAsync<int>(bill);
			return (ErrorCode.None, billId);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.BillInsertError], e,
				new {Bill = bill, ErrorCode = ErrorCode.BillInsertFailException}, "Insert Bill Failed");
			return (ErrorCode.BillInsertFailException, -1);
		}
	}

	public async Task<ErrorCode> DeleteAsync(int billId)
	{
		try
		{
			var count = await _queryFactory.Query("Bill").Where("Id",billId).DeleteAsync();

			if (count != -1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.BillDeleteError],
					new {BillId = billId, ErrorCode = ErrorCode.BillDeleteFail}, "Delete Bill Failed");
				return ErrorCode.BillDeleteFail;
			}
			
			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.BillDeleteError], e,
				new {BillId = billId, ErrorCode = ErrorCode.BillDeleteFailException}, "Delete Bill Failed");
			return ErrorCode.BillDeleteFailException;
		}
	}

}