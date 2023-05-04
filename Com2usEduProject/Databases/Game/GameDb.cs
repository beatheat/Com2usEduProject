﻿using System.Data;
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
	readonly ILogger<GameDb> _logger;
	readonly QueryFactory _queryFactory;

	IDbConnection _dbConnection;


	public PlayerTable PlayerTable => new PlayerTable(_queryFactory, _logger);
	public PlayerItemTable PlayerItemTable => new PlayerItemTable(_queryFactory, _logger);
	public MailTable MailTable => new MailTable(_queryFactory, _logger);
	public MailItemTable MailItemTable => new MailItemTable(_queryFactory, _logger);

	public GameDb(ILogger<GameDb> logger, IOptions<DbConnectionConfig> dbConfig)
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
			_logger.ZLogErrorWithPayload(LogManager.EventIdDic[EventType.GameDbConnectionError], e,
				new {ConnectionString = _dbConfig.Value.GameDb}, "GameDb Connection Failed");
		}
	}
	
	public void Close()
	{
		_dbConnection.Close();
	}

}