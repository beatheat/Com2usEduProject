﻿using System.Text.Json;
using ZLogger;

namespace Com2usEduAPIServer.Tools;

using Microsoft.Extensions.Logging;
using System.Collections.Generic;

public static class LogManager
{
	private static ILoggerFactory s_loggerFactory;

	public static Dictionary<EventType, EventId> EventIdDic { private set; get; }
	
	public static void SetLoggerFactory(ILoggerFactory loggerFactory)
	{
		s_loggerFactory = loggerFactory;
	}

	public static ILogger<T>? GetLogger<T>() where T : class
	{
		return s_loggerFactory.CreateLogger<T>();
	}

	public static void Init()
	{
		EventIdDic = new Dictionary<EventType, EventId>();
		foreach (EventType logEventType in Enum.GetValues(typeof(EventType)))
		{
			EventIdDic.Add(logEventType,new EventId((int)logEventType, logEventType.ToString()));
		}
	}
	
	
	public static void SettingLogger(ILoggingBuilder loggingBuilder, string logDirectory)
	{
		Init();
		
		loggingBuilder.ClearProviders();
		
		var exists = Directory.Exists(logDirectory);

		if (!exists)
		{
			Directory.CreateDirectory(logDirectory);
		}

		loggingBuilder.AddZLoggerRollingFile(
			fileNameSelector: (dt, x) => $"{logDirectory}{dt.ToLocalTime():yyyy-MM-dd}_{x:000}.log",
			timestampPattern: x => x.ToLocalTime().Date, 
			rollSizeKB: 1024,
			options =>
			{
				options.EnableStructuredLogging = true;
				var time = JsonEncodedText.Encode("Timestamp");
				//DateTime.Now는 UTC+0 이고 한국은 UTC+9이므로 9시간을 더한 값을 출력한다.
				var timeValue = JsonEncodedText.Encode(DateTime.Now.AddHours(9).ToString("yyyy/MM/dd HH:mm:ss"));

				options.StructuredLoggingFormatter = (writer, info) =>
				{
					writer.WriteString(time, timeValue);
					info.WriteToJsonWriter(writer);
				};
			}); // 1024KB
        
		loggingBuilder.AddZLoggerConsole(options =>
		{
			options.EnableStructuredLogging = true;
			var time = JsonEncodedText.Encode("EventTime");
			var timeValue = JsonEncodedText.Encode(DateTime.Now.AddHours(9).ToString("yyyy/MM/dd HH:mm:ss"));

			options.StructuredLoggingFormatter = (writer, info) =>
			{
				writer.WriteString(time, timeValue);
				info.WriteToJsonWriter(writer);
			};
		});
   
	}
}