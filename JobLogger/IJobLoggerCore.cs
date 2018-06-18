namespace JobLogger
{
	public interface IJobLoggerCore
	{
		void LogMessage(string logMessage, bool errorMessage, bool warning, bool error, bool logToFile, bool logToConsole, bool logToDatabase);
		void GetFromDb();
	}
}