using System;

namespace JobLogger
{
	public class Job
	{
		public static void Main()
		{
			var jobLoggerHelper = new JobLoggerHelper();
			var jobLoggerRepository = new JobLoggerRepository();

			var jobLoggerDb = new JobLoggerCore(jobLoggerHelper,jobLoggerRepository);
			jobLoggerDb.GetFromDb();

			var jobLogger = new JobLoggerCore(true, true, true,jobLoggerHelper, jobLoggerRepository);
			jobLogger.LogMessage("Error message", true, false, false);
			jobLogger.LogMessage("Warning message", false, true, false);
			jobLogger.LogMessage("Log message", false, false, true);

			Console.ReadLine();
		}

	}
}

