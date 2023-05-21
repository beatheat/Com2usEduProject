namespace Com2usEduAPITester;

// 1000 ~ 19999
public enum ErrorCode : UInt16
{
    None = 0,

    // Common 10000 ~ ===========================================
    UnhandledException = 10001,
    InValidRequestHttpBody = 10004,
    AuthTokenFailWrongAuthToken = 10006,

    InvalidMasterDataVersion = 10007,
    InvalidClientVersion = 10008,

    AuthTokenFailWrongKeyword = 10009,
    AuthTokenFailSetLock = 10010,

    UnAuthorizedPlayerId = 10011,

    // Controller 20000 ~ =======================================
    // CreateAccount 20010~
    // LoadMail 20020~
    LoadMailRequestFromNonOwnerPlayer = 20020,
    ReceiveMailItemRequestFromNonOwnerPlayer = 20021,
    // LoadMailList 20030~
    LoadMailListRequestFromNonOwnerPlayer = 20030,
    // Login 20040~

    // ReceiveAttendanceReward 20050~
    ReceiveAttendanceRewardAlready = 20050,
    // ReceiveMailItem 20060~
    ReceiveMailItemAlready = 20060,
    // ReceiveInAppPurchaseItem 20070~
    DuplicatedReceiveInAppPurchaseItemRequest = 20070,
    // LoadPlayerItem 20080 ~

    // EnforcePlayerItem 20090 ~
    EnforcePlayerItemRequestFromNonOwnerPlayer = 20090,

    // LoadPlayer 20100 ~

    //CompleteStage 20110 ~

    //FarmStageNpc 20120 ~
    FarmStageNpcInfoNotExist = 20120,
    FarmStageNpcInvalidNpc = 20121,
    //EnterStage 20130 ~
    EnterStageDuplicate = 20130,
    WrongStageCode = 20131,
    //FarmStage 20140 ~
    FarmStageItemInfoUpdateFail = 20140,
    FarmStageItemInvalidItem = 20141,
    //LoadCompleteStageList 20150 ~ 


    // AccountDb 30000 ~ =========================================
    AccountDbConnectionFail = 30001,

    // InsertAccount 30010 ~
    InsertAccountFailException = 30010,
    InsertAccountDuplicate = 30011,

    // DeleteAccount 30020 ~ 
    DeleteAccountFailException = 30020,

    //VerifyAccount 30030 ~
    VerifyAccountFailAccountNotExist = 30030,
    VerifyAccountFailPasswordNotMatch = 30031,

    VerifyAccountFailException = 30032,


    //GameDb 40000~ =================================================

    GameDbConnectionFail = 40001,


    // GameDb.PlayerTable 30100 ~ 
    PlayerInsertFailException = 30101,
    PlayerSelectFailException = 30102,
    PlayerDeleteFailException = 30103,
    PlayerDeleteFail = 30104,
    PlayerUpdateFailException = 30105,
    PlayerUpdateFail = 30106,

    //GameDb.PlayerItemTable 30200 ~
    PlayerItemInsertFailException = 30201,
    PlayerItemSelectFailException = 30202,
    PlayerItemSelectNotExist = 30203,
    PlayerItemUpdateFailException = 30204,
    PlayerItemUpdateFail = 30205,
    PlayerItemDeleteFailException = 30206,
    PlayerItemDeleteFail = 30207,

    //GameDb.MailTable 30300 ~
    MailInsertFailException = 30301,
    MailSelectFailException = 30302,
    MailUpdateFailException = 30303,
    MailUpdateFail = 30304,
    MailDeleteFailException = 30304,
    MailDeleteFail = 30305,

    //GameDb.MailItemTable 30400 ~ 
    MailItemInsertFailException = 30301,
    MailItemSelectFailException = 30302,
    MailItemUpdateFailException = 30303,
    MailItemUpdateFail = 30304,
    MailItemDeleteFailException = 30304,
    MailItemDeleteFail = 30305,

    //GameDb.BillTable 30500 ~
    BillInsertFailException = 30401,
    BillInsertFail = 30402,
    BillSelectFailException = 30403,
    BillUpdateFailException = 30404,
    BillUpdateFail = 30405,
    BillDeleteFailException = 30406,
    BillDeleteFail = 30407,

    //GameDb.PlayerAttendanceTable 30600 ~
    PlayerAttendanceInsertFailException = 30601,
    PlayerAttendanceInsertFail = 30602,
    PlayerAttendanceSelectFailException = 30603,
    PlayerAttendanceUpdateFailException = 30604,
    PlayerAttendanceUpdateFail = 30605,
    PlayerAttendanceDeleteFailException = 30606,
    PlayerAttendanceDeleteFail = 30607,

    //GameDb.PlayerCompletedStageTable 30700 ~
    PlayerCompletedStageInsertFailException = 30701,
    PlayerCompletedStageInsertFail = 30702,
    PlayerCompletedStageSelectFailException = 30703,
    PlayerCompletedStageUpdateFailException = 30704,
    PlayerCompletedStageUpdateFail = 30705,
    PlayerCompletedStageDeleteFailException = 30706,
    PlayerCompletedStageDeleteFail = 30707,
    PlayerCompletedStageInsertDuplicate = 30710,

    //MasterDb 40000~ =====================================================
    UnknownCode = 40001,
    UnknownItemCode = 40002,
    UnknownAttendanceRewardDay = 40003,
    UnknownShopItemCode = 40004,
    UnknownStageItemCode = 40005,
    UnknownStageNpcCode = 40006,


    //RedisDb 50000 ~ ======================================================
    RedisFailException = 50001,
    RedisKeyNotFound = 50002,
    RedisSetDuplicateKey = 50003,
    RedisGetFailed = 50004,
    //AuthManager 50100 ~ --------------------------------------------------

    //NoticeManager 50200 ~ ------------------------------------------------
    NoticeManager = 50201,
    //ChatManager 50300 ~ --------------------------------------------------
    ChatLobbyOutOfIndex = 50301,
    EnterLockDuplicate = 50302,
    DelLobbyEnterLockFail = 50303,
    LobbyUserCountNotExist = 50304,
    SetChatUserFail = 50305,
    ChatUserNotFound = 50306,
    ChatLobbyFull = 50307,
    //StageManager 50400 ~ -------------------------------------------------

}