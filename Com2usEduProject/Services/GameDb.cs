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

namespace Com2usEduProject.Services;

public class GameDb : IGameDb
{
	readonly IOptions<DbConnectionConfig> _dbConfig;
	readonly ILogger<AccountDb> _logger;
	readonly QueryFactory _queryFactory;

	IDbConnection _dbConnection;

	const int MAILBOX_PAGE_SIZE = 20;

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
			return (ErrorCode.CreateAccountFailException, new Player());
		}
	}

	
	public async Task<(ErrorCode, int)> InsertPlayerItemAsync(PlayerItem item)
	{
		try
		{
			var playerItemUniqueNo = await _queryFactory.Query("PlayerItem").InsertGetIdAsync<int>(item);
		
			_logger.ZLogDebug($"[InsertPlayerItem] PlayerId: {item.PlayerId} ItemCode {item.ItemCode} Count {item.Count}");

			return (ErrorCode.None, playerItemUniqueNo);
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
			
			var playerItemUniqueNo = await _queryFactory.Query("PlayerItem").InsertGetIdAsync<int>(playerItem);
		
			_logger.ZLogDebug($"[InsertPlayerItem] PlayerId: {playerId} ItemCode {item.Code} Count {count}");

			return (ErrorCode.None, playerItemUniqueNo);
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
	public async Task<ErrorCode> InsertMailAsync(Mail mail)
	{
		try
		{
			var count = await _queryFactory.Query("Mail").InsertAsync(mail);

			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadMailboxPageCount],
					new {Mail = mail, ErrorCode = ErrorCode.MailInsertFail}, "Insert Mailbox Failed");
				return ErrorCode.MailInsertFail;
			}
			
			_logger.ZLogDebug($"[InsertMailAsync] PlayerId: {mail.PlayerId},MailName : {mail.PostName}");
			
			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.LoadMailboxPageCount], e,
				new {Mail = mail, ErrorCode = ErrorCode.MailInsertFailException}, "Insert Mailbox Failed");
			return ErrorCode.MailInsertFailException;
		}
	}
	
	public async Task<(ErrorCode, int)> LoadMailboxPageCountAsync(int playerId)
	{
		try
		{
			var mailCount = await _queryFactory.Query("Mail").Where("PlayerId", playerId).CountAsync<int>();

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
			var mailboxPage = await _queryFactory.Query("Mail").Where("PlayerId", playerId).Limit(MAILBOX_PAGE_SIZE).Offset(MAILBOX_PAGE_SIZE * (pageNo-1)).GetAsync<Mail>();

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

	public async Task<ErrorCode> ReceiveMailItem(int mailId)
	{
		try
		{
			var count = await _queryFactory.Query("Mail").Where("Id", mailId).UpdateAsync(new {IsItemReceived = true});
			
			if (count != 1)
			{
				_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.ReceiveMailItem],
					new {MailId = mailId, ErrorCode = ErrorCode.MailUpdateFail}, "Update Mailbox Failed");
				return ErrorCode.MailUpdateFail;
			}

			_logger.ZLogDebug($"[ReceiveMailItem] MailId: {mailId}");

			return ErrorCode.None;
		}
		catch (Exception e)
		{
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.ReceiveMailItem], e,
				new {MailId = mailId, ErrorCode = ErrorCode.MailUpdateFailException}, "Update Mailbox Failed");
			return ErrorCode.MailUpdateFailException;
		}
	}
}