using System;
using System.Configuration;
using System.IO;

namespace JobLogger
{
	public interface IJobLoggerHelper
	{
		void WriteConsoleOutput(string text,ConsoleColor color);
		void FileWriteLine(string dateFileName, string logMessageTowrite);
	}

	public class JobLoggerHelper : IJobLoggerHelper
	{
		public void WriteConsoleOutput(string text,ConsoleColor color)
		{
			Console.ForegroundColor = color ;
			Console.WriteLine(text);
		}

		public void FileWriteLine(string dateFileName, string logMessageTowrite)
		{
			string logFile = null;
			var fileName = String.Format(ConfigurationManager.AppSettings["LogFileDirectory"], "LogFile", dateFileName, "txt");
			if (File.Exists(fileName))
			{
				var file = new FileStream(fileName, FileMode.Open);

				using (TextReader tr = new StreamReader(file))
				{
					logFile = tr.ReadLine();
				}
			}
			logFile = !string.IsNullOrEmpty(logFile) ? Environment.NewLine + logMessageTowrite : logMessageTowrite;
			File.AppendAllText(fileName, logFile);
		}
	}
}
