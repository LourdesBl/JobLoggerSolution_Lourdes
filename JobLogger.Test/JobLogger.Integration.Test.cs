using System;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace JobLogger.Test
{
    [TestClass]
    public class JobLoggerIntegrationTest
    {
        private string _message;
        private bool _logToFile;
        private bool _logToConsole;
        private bool _logToDatabase;
        private bool _isError;
        private bool _isMessage;
        private bool _isWarning;
        private IJobLoggerHelper _jobLoggerHelper;
        private IJobLoggerRepository _jobLoggerRepository;

        [TestMethod]
        public void LogMessage_Success()
        {
            _logToDatabase = true;
            _isError = true;
            _message = "This is a Test with an error message";
            _jobLoggerRepository = new JobLoggerRepository();
            _jobLoggerHelper = new JobLoggerHelper();

            var appConsole = new JobLoggerApplication(new JobLoggerCore(_jobLoggerHelper, _jobLoggerRepository));
            appConsole.Run(_message,_isMessage,_isWarning,_isError,_logToFile,_logToConsole,_logToDatabase);

            //DB Asserts
            var allLogValues = _jobLoggerRepository.GetAllLogValues();
            Assert.IsTrue(allLogValues.ToList().Any());
            Assert.IsTrue(allLogValues.Any(x => x.Message.Contains(_message)));

            //cleaning db
            _jobLoggerRepository.DeleteFromLogValue(allLogValues.First(x => x.Message.Contains(_message)).Id);

        }
    }
}
