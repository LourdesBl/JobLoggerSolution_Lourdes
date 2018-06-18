using System;
using Autofac;

namespace JobLogger
{
	public class Job
	{
		public static void Main(string[] args)
		{
			var container = ConfigureContainer();
			var application = container.Resolve<JobLoggerApplication>();

			application.Run();
		}

		private static IContainer ConfigureContainer()
		{
			var builder = new ContainerBuilder();

			// Register all dependencies
			builder.RegisterType<JobLoggerApplication>().AsSelf();
			builder.RegisterType<JobLoggerRepository>().As<IJobLoggerRepository>();
			builder.RegisterType<JobLoggerHelper>().As<IJobLoggerHelper>();
			
			return builder.Build();
		}
	}

	public class JobLoggerApplication
	{
		private readonly IJobLoggerHelper _jobLoggerHelper;
		private readonly IJobLoggerRepository _jobLoggerRepository;

		public JobLoggerApplication(IJobLoggerHelper jobLoggerHelper, IJobLoggerRepository jobLoggerRepository)
		{
			this._jobLoggerRepository = jobLoggerRepository;
			this._jobLoggerHelper = jobLoggerHelper;
		}
		public void Run()
		{
			//var jobLoggerDb = new JobLoggerCore(_jobLoggerHelper, _jobLoggerRepository);
			//jobLoggerDb.GetFromDb();

			var jobLogger = new JobLoggerCore(true, true, true, _jobLoggerHelper, _jobLoggerRepository);
			jobLogger.LogMessage("Error message", true, false, false);
			jobLogger.LogMessage("Warning message", false, true, false);
			jobLogger.LogMessage("Log message", false, false, true);

			Console.ReadLine();
		}

	}
}

