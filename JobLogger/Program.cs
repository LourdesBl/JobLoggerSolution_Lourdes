using Autofac;

namespace JobLogger
{
    public class Job
    {
        public static void Main(string[] args)
        {
            var container = ConfigureContainer();
            var application = container.Resolve<JobLoggerApplication>();


            application.Run("Error message", true, false, false, true, true, true);
            application.Run("Warning message", false, true, false, true, true, true);
            application.Run("Log message", false, false, true, true, true, true);

            //application.Run();
        }

        private static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Register all dependencies
            builder.RegisterType<JobLoggerApplication>().AsSelf();
            builder.RegisterType<JobLoggerRepository>().As<IJobLoggerRepository>();
            builder.RegisterType<JobLoggerHelper>().As<IJobLoggerHelper>();
            builder.RegisterType<JobLoggerCore>().As<IJobLoggerCore>();

            return builder.Build();
        }
    }

    public class JobLoggerApplication
    {
        private readonly IJobLoggerCore _jobLoggerCore;
        public JobLoggerApplication(IJobLoggerCore jobLoggerCore)
        {
            this._jobLoggerCore = jobLoggerCore;
        }
        public void Run(string logMessage, bool errorMessage, bool warning, bool error, bool logToFile, bool logToConsole, bool logToDatabase)
        {
            _jobLoggerCore.LogMessage(logMessage, errorMessage, warning, error, logToFile, logToConsole, logToDatabase);

            //Console.ReadLine();
        }

    }
}

