namespace JobLogger
{
	public interface IJobLoggerCore
	{
		void LogMessage(string logMessage, bool errorMessage, bool warning, bool error);
		void GetFromDb();
	}
}