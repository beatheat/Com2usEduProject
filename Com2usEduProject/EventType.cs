namespace Com2usEduProject;

public enum EventType
{
	
	// Controller 10000~ ===================================
	//CreateAccount 10010~ ---------------------------------
	APICreateAccount = 10011,
	APICreateAccountError = 10012,
	
	//Login 10020~ -----------------------------------------
	APILogin = 10021,
	APILoginError = 10022,
	
	//LoadMailList 10030~ ----------------------------------
	APILoadMailList = 10031,
	APILoadMailListError = 10032,

	//LoadMail 10040~ --------------------------------------
	APILoadMail = 10041,
	APILoadMailError = 10042,
	
	//ReceiveMailItem 10050~ ------------------------------
	APIReceiveMailItem = 10051,
	APIReceiveMailItemError = 10052,
	
	//ReceiveAttendanceReward 10060~ ----------------------
	APIReceiveAttendanceReward = 10061,
	APIReceiveAttendanceRewardError = 10062,
	
	//ReceiveInAppPurchaseItem 10070~ ---------------------
	APIReceiveInAppPurchaseItem = 10071,
	APIReceiveInAppPurchaseItemError = 10072,
	
	//LoadPlayerItem 10080~ -------------------------------
	APILoadPlayerItem = 10081,
	APILoadPlayerItemError = 10082,
	
	//EnforceItem 10090~ ---------------------------------
	APIEnforcePlayerItem = 10091,
	APIEnforcePlayerItemError = 10092,
	
	//LoadPlayer 10100 ~ ----------------------------------
	APILoadPlayer = 10101,
	APILoadPlayerError = 10102,
	
	//EnterChatLobby 10110~ ------------------------------
	APIEnterChatLobby = 10111,
	APIEnterChatLobbyError = 10112,
	
	//ReadChat 10120 ~ -----------------------------------
	APIReadChat = 10121,
	APIReadChatError = 10122,
	
	//WriteCaht 10130 ~ ----------------------------------
	APIWriteChat = 10131,
	APIWriteChatError = 10132,
	
	// AccountDB 20000~ ===================================
	AccountDbError = 20000,
	AccountDbConnectionError = 20001,
	
	InsertAccountError = 20002,
	VerifyAccountError = 20003,
	DeleteAccountError = 20004,
	
	// GameDB 30000~ =====================================
	GameDbError = 30000,
	GameDbConnectionError = 30001,
	
	// GameDB.PlayerTable 30100 ~ ------------------------
	PlayerCreateAndInsertError = 30100,
	PlayerSelectError = 30110,
	PlayerSelectByAccountIdError = 30111,
	PlayerDeleteError = 30120,
	PlayerUpdateError = 30130,
	
	//GameDB.PlayerItemTable 30200 ~ ---------------------
	PlayerItemInsertError = 30200,
	PlayerItemSelectError = 30210,
	PlayerItemDeleteError = 30220,
	PlayerItemUpdateError = 30230,

	//GameDB.MailTable 30300 ~ ---------------------------
	MailInsertError = 30300,
	MailSelectError = 30310,
	MailSelectListError = 30311,
	MailSelectCountError = 30312,
	MailDeleteError = 30320,
	MailUpdateError = 30330,
	
	//GameDB.MailItemTable 30400 ~ -----------------------
	MailItemInsertError = 30400,
	MailItemSelectError = 30410,
	MailItemSelectListError = 30411,
	MailItemSelectCountError = 30412,
	MailItemDeleteError = 30420,
	MailItemUpdateError = 30430,
	
	//GameDB.BillTable 30500 ~ ---------------------------
	BillInsertError = 30500,
	BillSelectError = 30510,
	BillDeleteError = 30520,
	BillUpdateError = 30530,
	
	// MasterDB 40000~ ===================================
	MasterDbError = 40000,
	MasterDbConnectionError = 40001,
	
	// MemoryDB 50000~ ====================================
	MemoryDbError = 50000,
	MemoryDbConnectionError = 50001,
	
	RegisterUserError = 50002,
	GetUserError = 50003,
	SetUserRequestLockError = 50004,
	DelUserRequestLockError = 50005,
	GetNoticeError = 50006,
	
	//ChatManager 50100~ ---------------------------------
	LoadChatHistoryError = 50101,
	LoadChatFromIndexError = 50102,
	WriteChatError = 50103,
}