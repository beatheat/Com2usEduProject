﻿using System.Data;
using CloudStructures;
using CloudStructures.Structures;
using Com2usEduProject.DBSchema;
using Com2usEduProject.Tools;
using MySqlConnector;
using SqlKata.Compilers;
using SqlKata.Execution;
using ZLogger;

namespace Com2usEduProject.Services;

public class MasterDb : IMasterDb
{
	static readonly ILogger<MasterDb> s_logger = LogManager.GetLogger<MasterDb>();

	List<Item> _itemList;
	List<AttendanceReward> _attendanceRewards;
	List<ShopItem> _shopItemList;
	List<StageItem> _stageItemList;
	List<StageNpc> _stageNpcList;
	List<InitialPlayerItem> _initialItemList;

	string _version;
	string _clientVersion;
	
	public void Init(string dbConnectionString)
	{
		MySqlConnection dbConnection;
		QueryFactory queryFactory;
		try
		{
			dbConnection = new MySqlConnection(dbConnectionString);
			dbConnection.Open();
			queryFactory = new QueryFactory(dbConnection, new MySqlCompiler());

		}
		catch (Exception e)
		{
			s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.MasterDbConnection],e, 
				new {ConnectionString = dbConnectionString}, "MasterDB Connection Failed");
			return;
		}
		
		try
		{
			_itemList = queryFactory.Query("Item").Select("*").Get<Item>().ToList();
			_attendanceRewards = queryFactory.Query("AttendanceReward").Get<AttendanceReward>().ToList();
			_shopItemList = queryFactory.Query("ShopItem").Get<ShopItem>().ToList();
			_stageItemList = queryFactory.Query("StageItem").Get<StageItem>().ToList();
			_stageNpcList = queryFactory.Query("StageNpc").Get<StageNpc>().ToList();

			_initialItemList = queryFactory.Query("InitialPlayerItem").Get<InitialPlayerItem>().ToList();
			
			_version = queryFactory.Query("Version").Select("version").First<string>();
			_clientVersion = queryFactory.Query("Version").Select("clientVersion").First<string>();
			dbConnection.Close();;
		}
		catch (Exception e)
		{
			s_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.MasterDbConnection],e, "MasterDB Load Failed");
		}
	}

	public (ErrorCode, Item) GetItem(int itemCode)
	{
		if (itemCode > 0 && itemCode < _itemList.Count)
		{
			return (ErrorCode.None, _itemList[itemCode]);
		}
		
		return (ErrorCode.UnknownCode, new Item());
	}

	public (ErrorCode, AttendanceReward) GetAttendanceReward(int day)
	{
		if(day > 0 && day < _attendanceRewards.Count)
		{
			return (ErrorCode.UnknownCode, _attendanceRewards[day]);

		}
		return (ErrorCode.UnknownCode, new AttendanceReward());
	}

	public (ErrorCode, IList<ShopItem>) GetShopItem(int shopItemCode)
	{
		var selectedShopItems = _shopItemList.Where(x => x.ItemCode == shopItemCode).ToList();

		if(selectedShopItems.Count  == 0)
		{
			return (ErrorCode.UnknownCode, new List<ShopItem>());
		}
		
		return (ErrorCode.None, selectedShopItems);
	}

	public (ErrorCode,IList<StageItem>) GetStageItem(int stageCode)
	{
		var selectedStageItems = _stageItemList.Where(x => x.StageCode == stageCode).ToList();

		if (selectedStageItems.Count == 0)
		{
			return (ErrorCode.UnknownCode, new List<StageItem>());
		}
		
		return (ErrorCode.None, selectedStageItems);
	}

	public (ErrorCode, IList<StageNpc>) GetStageNpc(int stageCode)
	{ 
		var selectedStageNpcs = _stageNpcList.Where(x => x.StageCode == stageCode).ToList();

		if (selectedStageNpcs.Count == 0)
		{
			return (ErrorCode.UnknownCode, new List<StageNpc>());
		}
		
		return (ErrorCode.None, selectedStageNpcs);
	}

	public IList<InitialPlayerItem> GetInitialPlayerItem()
	{
		return _initialItemList;
	}

	public string GetVersion()
	{
		return _version;
	}

	public string GetClientVersion()
	{
		return _clientVersion;
	}
}