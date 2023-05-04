namespace Com2usEduProject;

public enum EventType
{
	
	// Controller 10000~ ===================================
	//CreateAccount 10100~ ---------------------------------
	APICreateAccount = 10101,
	APICreateAccountError = 10102,
	
	//Login 10200~ -----------------------------------------
	APILogin = 10201,
	APILoginError = 10202,
	
	//LoadMailList 10300~ ----------------------------------
	APILoadMailList = 10301,
	APILoadMailListError = 10302,

	//LoadMail 10400~ --------------------------------------
	APILoadMail = 10401,
	APILoadMailError = 10402,
	
	//ReceiveMailItem 10500~ ------------------------------
	APIReceiveMailItem = 10501,
	APIReceiveMailItemError = 10502,
	
	//ReceiveAttendanceReward 10600~ ----------------------
	APIReceiveAttendanceReward = 10601,
	APIReceiveAttendanceRewardError = 10602,
	
	//ReceiveInAppPurchaseItem 10700~ ---------------------
	APIReceiveInAppPurchaseItem = 10701,
	APIReceiveInAppPurchaseItemError = 10702,
	
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
	BillInsertError = 30400,
	BillSelectError = 30410,
	BillDeleteError = 30420,
	BillUpdateError = 30430,
	
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
	GetNoticeError
}