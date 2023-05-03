namespace Com2usEduProject;

// 1000 ~ 19999
public enum ErrorCode : UInt16
{
    None = 0,

    // Common 1000 ~
    UnhandledException = 1001,
    InValidRequestHttpBody = 1004,
    AuthTokenFailWrongAuthToken = 1006,
    
    InvalidMasterDataVersion = 1007,
    InvalidClientVersion = 1008,
    

    // Account 2000 ~
    InsertAccountFailException = 2001,    
    LoginFailException = 2002,
    LoginFailUserNotExist = 2003,
    LoginFailPwNotMatch = 2004,
    LoginFailSetAuthToken = 2005,
    AuthTokenMismatch = 2006,
    AuthTokenNotFound = 2007,
    AuthTokenFailWrongKeyword = 2008,
    AuthTokenFailSetNx = 2009,
    AccountIdMismatch = 2010,
    DuplicatedLogin = 2011,
    CreateAccountFailInsert = 2012,
    InsertAccountDuplicate = 2013,
    LoginFailAddRedis = 2014,
    LoginFailConnectRedis = 2015,
    DeleteAccountFailException = 2016,
    

    //GameDb 3000~ 
    GetGameDbConnectionFail = 3002,
    
    NotRegisteredPlayerId = 3003,

    PlayerDataInsertFailException = 3004,
    PlayerDataSelectFailException = 3005,
    
    PlayerItemInsertFail = 3006,
    PlayerItemDeleteFail = 3007,
    PlayerItemInsertFailException = 3008,
    PlayerItemSelectFailException = 3009,
    PlayerItemDeleteFailException = 3010,
    
    MailSelectFailException = 3011,
    MailInsertFail = 3012,
    MailInsertFailException = 3013,
    MailUpdateFail = 3014,
    MailUpdateFailException = 3015,
    MailItemInsertFail = 3016,
    MailItemInsertFailException = 3017,

    //MasterDb 4000~
    UnknownCode = 4001,
    UnknownItemCode = 4002,
    UnknownAttendanceRewardDay = 4003,
    UnknownShopItemCode = 4004,
    UnknownStageItemCode = 4005,
    UnknownStageNpcCode = 4006,
    

    //RedisDb 5000 ~
    RedisFailException = 5001,
    RedisKeyNotFound = 5002,
    RedisSetDuplicateKey = 5003,
    RedisGetFailed = 5004,
    
    //Controller 6000 ~
    ReceiveMailItemAlready = 6001,
}