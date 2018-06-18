using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using EntityFramework.CodeFirst;
using EntityFramework.CodeFirst.Entities;

namespace JobLogger
{
	public class JobLoggerCore : IJobLoggerCore
	{
		private readonly bool _logToFile;
		private readonly bool _logToConsole;
		private readonly bool _logToDatabase;

		private readonly IJobLoggerHelper _jobLoggerHelper;

		private readonly IJobLoggerRepository _jobLoggerRepository;
		//private readonly bool _logMessage;
		//private readonly bool _logWarning;
		//private readonly bool _logError;


		public JobLoggerCore(bool logToFile, bool logToConsole, bool logToDatabase, IJobLoggerHelper jobLoggerHelper, IJobLoggerRepository jobLoggerRepository)
		{
			_logToDatabase = logToDatabase;
			_logToFile = logToFile;
			_logToConsole = logToConsole;
			_jobLoggerHelper = jobLoggerHelper;
			_jobLoggerRepository = jobLoggerRepository;
		}

		public JobLoggerCore(IJobLoggerHelper jobLoggerHelper, IJobLoggerRepository jobLoggerRepository)
		{
			_jobLoggerRepository = jobLoggerRepository;
			_jobLoggerHelper = jobLoggerHelper;
		}

		public void LogMessage(string logMessage, bool errorMessage, bool warning, bool error)
		{
			logMessage = logMessage.Trim();
			if (string.IsNullOrEmpty(logMessage))
				throw new Exception("Empty Message");

			if (!_logToConsole && !_logToFile && !_logToDatabase)
				throw new Exception("Invalid configuration");

			if (!errorMessage && !warning && !error)
				throw new Exception("Error or Warning or Message must be specified");

			int messageTypeCode;
			ConsoleColor consoleColor;

			if (errorMessage)
			{
				messageTypeCode = 1;
				consoleColor = ConsoleColor.White;
			}
			else if (error)
			{
				messageTypeCode = 2;
				consoleColor = ConsoleColor.Red;
			}
			else
			{
				messageTypeCode = 3;
				consoleColor = ConsoleColor.Yellow;
			}

			if (_logToDatabase)
			{
				var logValue = new LogValue()
				{
					Code = messageTypeCode,
					Message = logMessage
				};
				_jobLoggerRepository.SaveIntoLogValue(logValue);
			}

			var logMessageTowrite = DateTime.Now.ToShortDateString() + " " + logMessage;
			if (_logToFile)
			{
				var dateLogName = DateTime.Now.ToShortDateString().Replace("/", "_");
				_jobLoggerHelper.FileWriteLine(dateLogName, logMessageTowrite);
			}

			if (_logToConsole)
			{
				_jobLoggerHelper.WriteConsoleOutput(logMessageTowrite, consoleColor);
			}
		}

		public void GetFromDb()
		{
			List<LogValue> logValues = _jobLoggerRepository.GetAllLogValues().ToList();

			foreach (var logs in logValues)
			{
				var text = logs.Code + @" " + logs.Message + @" " + logs.InsertDateTime.ToString();
				_jobLoggerHelper?.WriteConsoleOutput(text, ConsoleColor.Gray);
			}
		}


	}
}
