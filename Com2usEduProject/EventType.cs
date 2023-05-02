namespace Com2usEduProject;

public enum EventType
{
	// Controller 1000 ~
	CreateAccount = 1001,
	Login = 1002,
	OpenMailbox = 1003,
	ReceiveMail = 1004,
	
	// AccountDB 2000 ~
	AccountDbConnection = 2001,
	InsertAccount = 2002,
	VerifyAccount = 2003,
	DeleteAccount = 2004,
	
	// GameDB 3000 ~
	GameDbConnection = 3001,
	CreatePlayerData = 3002,
	LoadPlayerData = 3003,
	InsertPlayerItem = 3004,
	LoadPlayerItems = 3005,
	LoadMailboxPageCount = 3006,
	LoadMailboxPage = 3007,
	LoadMail = 3008,
	ReceiveMailItem = 3009,
	
	// MasterDB 4000 ~
	MasterDbConnection = 4001,
	
	// MemoryDB 5000 ~
	MemoryDbConnection = 5001,
	RegisterUser = 5002,
	GetUser = 5003,
	SetUserRequestLock = 5004,
	DelUserRequestLock = 5005,
	GetNotice = 5006,

}