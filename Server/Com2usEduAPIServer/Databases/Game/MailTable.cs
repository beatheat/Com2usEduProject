﻿using Com2usEduAPIServer.Databases.Schema;
using Com2usEduAPIServer.Tools;
using SqlKata.Execution;
using ZLogger;

namespace Com2usEduAPIServer.Databases;

public class MailTable
{
	readonly QueryFactory _queryFactory;
	readonly ILogger<GameDb> _logger;
	

	public MailTable(QueryFactory queryFactory, ILogger<GameDb> logger)
	{
		_queryFactory = queryFactory;
		_logger = logger;
	}
	

	public async Task<(ErrorCode,int)> InsertAsync(Mail mail)
	{
		try
		{
			var mailId = await _queryFactory.Query("Mail").InsertGetIdAsync<int>(mail);
			return (ErrorCode.None, mailId);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.MailInsertError], e,
				new {Mail = mail, ErrorCode = ErrorCode.MailInsertFailException}, "Insert Mail Failed");
			return (ErrorCode.MailInsertFailException,-1);
		}
	}
	
	public async Task<(ErrorCode, Mail)> SelectAsync(int mailId)	
	{
		try
		{
			var mail = await _queryFactory.Query("Mail").Where("Id", mailId).FirstAsync<Mail>();
			return (ErrorCode.None, mail);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.MailSelectError], e,
				new {MailId = mailId, ErrorCode = ErrorCode.MailSelectFailException}, "Select Mail Failed");
			return (ErrorCode.MailSelectFailException, new Mail());
		}
	}

	
	public async Task<(ErrorCode, int)> SelectCountAsync(int playerId)
	{
		try
		{
			var count = await _queryFactory.Query("Mail").Where("PlayerId", playerId).Where("ExpireDate",">",DateTime.Now).CountAsync<int>();
			return (ErrorCode.None, count);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.MailSelectCountError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.MailSelectFailException}, "Select Mail Failed");
			return (ErrorCode.MailSelectFailException, -1);
		}
	}

	
	public async Task<(ErrorCode, List<Mail>)> SelectList(int playerId, int size, int offset)
	{
		try
		{
			var mailboxPage = await _queryFactory.Query("Mail").
				Select("Id","PlayerId","Name","TransmissionDate","ExpireDate", "IsItemReceived").
				Where("PlayerId", playerId).
				Where("ExpireDate", ">", DateTime.Now).
				OrderByDesc("Id","TransmissionDate").
				Limit(size).Offset(offset).GetAsync<Mail>();

			return (ErrorCode.None, mailboxPage.ToList());
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.MailSelectListError], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.MailSelectFailException}, "Select Mail Failed");
			return (ErrorCode.MailSelectFailException, new List<Mail>());
		}
	}


	public async Task<ErrorCode> UpdateItemReceivedToTrue(int mailId)
	{
		try
		{
			var count = await _queryFactory.Query("Mail").Where("Id", mailId).UpdateAsync(new {IsItemReceived = true});
			
			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.MailUpdateError],
					new {MailId = mailId, ErrorCode = ErrorCode.MailUpdateFail}, "Update Mail Failed");
				return ErrorCode.MailUpdateFail;
			}
			
			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.MailUpdateError], e,
				new {MailId = mailId, ErrorCode = ErrorCode.MailUpdateFailException}, "Update Mail Failed");
			return ErrorCode.MailUpdateFailException;
		}
	}
	
	public async Task<ErrorCode> DeleteAsync(int mailId)
	{
		try
		{
			var count = await _queryFactory.Query("Mail").Where("Id", mailId).UpdateAsync(new {IsItemReceived = true});
			
			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.MailDeleteError],
					new {MailId = mailId, ErrorCode = ErrorCode.MailDeleteFail}, "Delete Mail Failed");
				return ErrorCode.MailDeleteFail;
			}
			
			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.MailDeleteError], e,
				new {MailId = mailId, ErrorCode = ErrorCode.MailDeleteFailException}, "Delete Mail Failed");
			return ErrorCode.MailDeleteFailException;
		}
	}
}