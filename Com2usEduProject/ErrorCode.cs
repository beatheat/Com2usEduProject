﻿namespace Com2usEduProject;

// 1000 ~ 19999
public enum ErrorCode : UInt16
{
    None = 0,

    // Common 1000 ~
    UnhandleException = 1001,
    InValidRequestHttpBody = 1004,
    AuthTokenFailWrongAuthToken = 1006,
    
    InvalidMasterDataVersion = 1007,
    InvalidClientVersion = 1008,
    

    // Account 2000 ~
    CreateAccountFailException = 2001,    
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
    CreateAccountDuplicate = 2013,
    LoginFailAddRedis = 2014,
    LoginFailConnectRedis = 2015,
    

    //GameDb 3000~ 
    GetGameDbConnectionFail = 3002,
    
    NotRegisteredPlayerId = 3003,

    PlayerDataInsertFailException = 3004,
    PlayerDataSelectFailException = 3005,
    PlayerItemInsertFailException = 3006,
    PlayerItemSelectFailException = 3007,
    
    MailSelectFailException = 3008,
    MailInsertFail = 3009,
    MailInsertFailException = 3010,
    MailUpdateFail = 3011,
    MailUpdateFailException = 3012,

    //MasterDb 4000~
    UnknownCode = 4001,

    //RedisDb 5000 ~
    RedisFailException = 5001,
    RedisKeyNotFound = 5002,
    RedisSetDuplicateKey = 5003,
    RedisGetFailed = 5004,
    
    //Controller 6000 ~
    ReceiveMailAlready = 6001,
}