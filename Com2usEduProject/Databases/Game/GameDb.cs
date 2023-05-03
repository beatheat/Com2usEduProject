using System.Data;
using Com2usEduProject.DBSchema;
using Com2usEduProject.ModelDB;
using Com2usEduProject.Tools;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;
using ZLogger;
using Exception = System.Exception;

namespace Com2usEduProject.Databases;

public class GameDb : IGameDb
{
	readonly IOptions<DbConnectionConfig> _dbConfig;
	readonly ILogger<AccountDb> _logger;
	readonly QueryFactory _queryFactory;

	IDbConnection _dbConnection;

	const int MAILBOX_PAGE_SIZE = 20;
	const int MAIL_MAX_ITEM_SLOT_COUNT = 4;

	public GameDb(ILogger<AccountDb> logger, IOptions<DbConnectionConfig> dbConfig)
	{
		_dbConfig = dbConfig;
		_logger = logger;

		Open();
		
		_queryFactory = new QueryFactory(_dbConnection, new MySqlCompiler());
	}
	
	public void Dispose()
	{
		Close();
	}

	private void Open()
	{
		try
		{
			_dbConnection = new MySqlConnection(_dbConfig.Value.GameDb);
			_dbConnection.Open();
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GameDbConnection], e,
				new {ConnectionString = _dbConfig.Value.GameDb}, "GameDb Connection Failed");
		}
	}
	
	public void Close()
	{
		_dbConnection.Close();
	}

	public async Task<(ErrorCode, int)> CreatePlayerDataAsync(int accountId)
	{
		try
		{
			var playerId = await _queryFactory.Query("Player").InsertGetIdAsync<int>(new
			{
				AccountId = accountId,
				ContinuousAttendanceDays = 0,
				LastAttendanceDate = DateTime.Today,
			});
			
			_logger.ZLogDebug($"[CreatePlayerData] PlayerId: {playerId} AccountId {accountId}");

			return (ErrorCode.None, playerId);
		}
		catch  (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.CreatePlayerData], e,
				new {AccountId = accountId, ErrorCode = ErrorCode.PlayerDataInsertFailException}, "Insert PlayerData Failed");
			return (ErrorCode.PlayerDataInsertFailException, -1);
		}
	}

	public async Task<(ErrorCode, Player)> LoadPlayerDataAsync(int accountId)
	{
		try
		{
			var playerData = await _queryFactory.Query("Player").Where("AccountId", accountId).FirstAsync<Player>();
			_logger.ZLogDebug($"[LoadPlayerData] Id: {accountId}");
			
			return (ErrorCode.None, playerData);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadPlayerData], e,
				new {AccountId = accountId, ErrorCode = ErrorCode.PlayerDataSelectFailException}, "Select PlayerData Failed");		
			return (ErrorCode.InsertAccountFailException, new Player());
		}
	}

	
	public async Task<(ErrorCode, int)> InsertPlayerItemAsync(PlayerItem item)
	{
		try
		{
			var playerItemId = await _queryFactory.Query("PlayerItem").InsertGetIdAsync<int>(item);
		
			_logger.ZLogDebug($"[InsertPlayerItem] PlayerId: {item.PlayerId} ItemCode {item.ItemCode} Count {item.Count}");

			return (ErrorCode.None, playerItemId);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.InsertPlayerItem], e,
				new {PlayerItem = item, ErrorCode = ErrorCode.PlayerDataInsertFailException}, "Insert PlayerItem Failed");
			return (ErrorCode.PlayerItemInsertFailException, -1);
		}
	}

	public async Task<(ErrorCode, int)> InsertPlayerItemAsync(int playerId, Item item, int count)
	{
		try
		{
			PlayerItem playerItem = new PlayerItem
			{
				PlayerId = playerId,
				ItemCode = item.Code,
				Count = count,
				Attack = item.Attack,
				Defence = item.Defence,
				Magic = item.Magic,
				EnhanceCount = 0
			};
			
			var playerItemId = await _queryFactory.Query("PlayerItem").InsertGetIdAsync<int>(playerItem);
		
			_logger.ZLogDebug($"[InsertPlayerItem] PlayerId: {playerId} ItemCode {item.Code} Count {count}");

			return (ErrorCode.None, playerItemId);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.InsertPlayerItem], e,
				new {PlayerItem = item, ErrorCode = ErrorCode.PlayerDataInsertFailException}, "Insert PlayerItem Failed");
			return (ErrorCode.PlayerItemInsertFailException, -1);
		}
	}
	
	
	public async Task<(ErrorCode, IList<PlayerItem>)> LoadPlayerItemsAsync(int playerId)
	{
		try
		{
			var playerItems = await _queryFactory.Query("PlayerItem").Where("PlayerId", playerId).GetAsync<PlayerItem>();

			_logger.ZLogDebug($"[LoadPlayerItems] PlayerId: {playerId}");
			
			return (ErrorCode.None, playerItems.ToList());
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadPlayerItems], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.PlayerItemSelectFailException}, "Select PlayerItem Failed");
			return (ErrorCode.PlayerItemSelectFailException, new List<PlayerItem>());
		}
	}

	public async Task<ErrorCode> DeletePlayerItemAsync(int playerItemId)
	{
		try
		{
			int count = await _queryFactory.Query("PlayerItem").Where("Id", playerItemId).DeleteAsync();
			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.DeletePlayerItem],
					new {PlayerItemId = playerItemId, ErrorCode = ErrorCode.PlayerItemDeleteFail}, "Delete PlayerItem Failed");
				return ErrorCode.PlayerItemDeleteFail;

			}
			_logger.ZLogDebug($"[DeletePlayerItem] PlayerItemId: {playerItemId}");
			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.DeletePlayerItem], e,
				new {PlayerItemId = playerItemId, ErrorCode = ErrorCode.PlayerItemDeleteFailException}, "Delete PlayerItem Failed");
			return ErrorCode.PlayerItemDeleteFailException;
		}
	}

	public Task<(ErrorCode, int)> InsertMailAsync(Mail mail)
	{
		throw new NotImplementedException();
	}

	public Task<(ErrorCode, int)> InsertMailItemAsync(MailItem mailItem)
	{
		throw new NotImplementedException();
	}


	public async Task<(ErrorCode,int)> InsertMailAsync(Mail mail, IList<MailItem> mailItems)
	{
		try
		{
			var mailId = await _queryFactory.Query("Mail").InsertGetIdAsync<int>(mail);
			
			_logger.ZLogDebug($"[InsertMailAsync] PlayerId: {mail.PlayerId},MailName : {mail.Name}");
			return (ErrorCode.None, mailId);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadMailboxPageCount], e,
				new {Mail = mail, ErrorCode = ErrorCode.MailInsertFailException}, "Insert Mail Failed");
			return (ErrorCode.MailInsertFailException,-1);
		}
	}

	public async Task<(ErrorCode, int)> LoadMailItemAsync(MailItem mailItem)
	{
				
		try
		{
			var id = await _queryFactory.Query("MailItem").InsertGetIdAsync<int>(mailItem);
			
			//_logger.ZLogDebug($"[InsertMailItemAsync] PlayerId: {mail.PlayerId},MailName : {mail.Name}");
			return (ErrorCode.None,id);
		}
		catch (Exception e)
		{
			//_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadMail], e,
			//	new {Mail = mail, ErrorCode = ErrorCode.MailItemInsertFailException}, "Insert Mail Failed");
			return (ErrorCode.MailItemInsertFailException,-1);
		}

	}
	
	public async Task<(ErrorCode, int)> LoadMailboxPageCountAsync(int playerId)
	{
		try
		{
			var mailCount = await _queryFactory.Query("Mail").Where("PlayerId", playerId).
				Where("ExpireDate",">","NOW()").CountAsync<int>();

			_logger.ZLogDebug($"[LoadMailboxPageCount] PlayerId: {playerId}, MailCount : {mailCount}");
			
			return (ErrorCode.None, mailCount / MAILBOX_PAGE_SIZE);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadMailboxPageCount], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.MailSelectFailException}, "Select Mailbox Failed");
			return (ErrorCode.MailSelectFailException, -1);
		}
	}

	
	public async Task<(ErrorCode, IList<Mail>)> LoadMailboxPageAsync(int playerId, int pageNo)
	{
		try
		{
			var mailboxPage = await _queryFactory.Query("Mail").
				Select("Id","PlayerId","Name","TransmissionDate","ExpireDate", "IsItemReceived").
				Where("PlayerId", playerId).
				Where("ExpireDate", ">", "NOW()").
				Limit(MAILBOX_PAGE_SIZE).Offset(MAILBOX_PAGE_SIZE * (pageNo-1)).
				OrderByDesc("TransmissionDate").GetAsync<Mail>();

			
			_logger.ZLogDebug($"[LoadMailboxPage] PlayerId: {playerId}");
			
			return (ErrorCode.None, mailboxPage.ToList());
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadMailboxPage], e,
				new {PlayerId = playerId, ErrorCode = ErrorCode.MailSelectFailException}, "Select Mailbox Failed");
			return (ErrorCode.MailSelectFailException, new List<Mail>());
		}
	}

	public async Task<(ErrorCode, Mail)> LoadMailAsync(int mailId)	
	{
		try
		{
			var mail = await _queryFactory.Query("Mail").Where("Id", mailId).FirstAsync<Mail>();
			
			_logger.ZLogDebug($"[LoadMail] MailId: {mailId}");
			
			return (ErrorCode.None, mail);
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadMail], e,
				new {MailId = mailId, ErrorCode = ErrorCode.MailSelectFailException}, "Select Mailbox Failed");
			return (ErrorCode.MailSelectFailException, new Mail());
		}
	}

	public async Task<ErrorCode> UpdateMailItemReceivedToTrueAsync(int mailId)
	{
		try
		{
			var count = await _queryFactory.Query("Mail").Where("Id", mailId).UpdateAsync(new {IsItemReceived = true});
			
			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.UpdateMailItemReceived],
					new {MailId = mailId, ErrorCode = ErrorCode.MailUpdateFail}, "Update Mailbox Failed");
				return ErrorCode.MailUpdateFail;
			}

			_logger.ZLogDebug($"[ReceiveMailItem] MailId: {mailId}");

			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.UpdateMailItemReceived], e,
				new {MailId = mailId, ErrorCode = ErrorCode.MailUpdateFailException}, "Update Mailbox Failed");
			return ErrorCode.MailUpdateFailException;
		}
	}
	
	
	public async Task<(ErrorCode, IList<MailItem>)> LoadMailItemsAsync(int mailId)
	{
		try
		{
			var mailItems = await _queryFactory.Query("MailItem").Where("MailId", mailId).GetAsync<MailItem>();
			
			_logger.ZLogDebug($"[LoadMail] MailId: {mailId}");
			
			return (ErrorCode.None, mailItems.ToList());
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadMail], e,
				new {MailId = mailId, ErrorCode = ErrorCode.MailSelectFailException}, "Select Mailbox Failed");
			return (ErrorCode.MailSelectFailException, new List<MailItem>());
		}
	}

}