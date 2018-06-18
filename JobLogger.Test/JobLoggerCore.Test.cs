using System;
using System.Security.Cryptography.X509Certificates;
using EntityFramework.CodeFirst.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace JobLogger.Test
{
    [TestClass]
    public class JobLoggerCoreTest
    {
        private string _message = String.Empty;
        private bool _logToFile;
        private bool _logToConsole;
        private bool _logToDatabase;
        private bool _isError;
        private bool _isMessage;
        private bool _isWarning;

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void LogMessage_ThrowInvalidConfiguration()
        {
            _message = "Message";
            var mockHelper = new Mock<IJobLoggerHelper>();
            var mockRepository = new Mock<IJobLoggerRepository>();

            var core = new JobLoggerCore(mockHelper.Object, mockRepository.Object);
            core.LogMessage(_message, _isMessage, _isWarning, _isError,_logToFile,_logToConsole,_logToDatabase );

            Assert.ThrowsException<Exception>(() => core, "Invalid configuration");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void LogMessage_ThrowEmptySpecification()
        {
            _message = "Message";
            _logToFile = true;
            var mockHelper = new Mock<IJobLoggerHelper>();
            var mockRepository = new Mock<IJobLoggerRepository>();

            var core = new JobLoggerCore(mockHelper.Object, mockRepository.Object);
            core.LogMessage(_message, _isMessage, _isWarning, _isError, _logToFile, _logToConsole, _logToDatabase);

            Assert.ThrowsException<Exception>(() => core, "Error or Warning or Message must be specified");
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void LogMessage_ThrowEmptyMessage()
        {
            _logToFile = true;
            _logToConsole = true;
            _logToDatabase = true;
            _message = string.Empty;
            _isMessage = true;

            var mockHelper = new Mock<IJobLoggerHelper>();
            var mockRepository = new Mock<IJobLoggerRepository>();

            var core = new JobLoggerCore(mockHelper.Object, mockRepository.Object);
            core.LogMessage(_message, _isMessage, _isWarning, _isError, _logToFile, _logToConsole, _logToDatabase);

            Assert.ThrowsException<Exception>(() => core, "Empty String");
        }

        [TestMethod]
        public void LogMessage_Message_Console()
        {
            _logToConsole = true;
            _message = "Message To Log";
            var color = ConsoleColor.White;

            _isMessage = true;
            var mockHelper = new Mock<IJobLoggerHelper>();
            mockHelper.Setup(c => c.WriteConsoleOutput(_message, color));

            var mockRepository = new Mock<IJobLoggerRepository>();
            var core = new JobLoggerCore(mockHelper.Object, mockRepository.Object);
            core.LogMessage(_message, _isMessage, _isWarning, _isError, _logToFile, _logToConsole, _logToDatabase);

            //Asserts
            mockHelper.Verify(x => x.WriteConsoleOutput(It.Is<string>(s => s.Contains(_message)), It.Is<ConsoleColor>(c => c == color)));

        }

        [TestMethod]
        public void LogMessage_Error_Console()
        {
            _logToConsole = true;
            _message = "ErrorMessage To Log";
            var color = ConsoleColor.Red;
            _isError = true;

            var mockHelper = new Mock<IJobLoggerHelper>();
            mockHelper.Setup(c => c.WriteConsoleOutput(_message, color));

            var mockRepository = new Mock<IJobLoggerRepository>();

            var core = new JobLoggerCore(mockHelper.Object, mockRepository.Object);
            core.LogMessage(_message, _isMessage, _isWarning, _isError, _logToFile, _logToConsole, _logToDatabase);

            //Asserts
            mockHelper.Verify(x => x.WriteConsoleOutput(It.Is<string>(s => s.Contains(_message)), It.Is<ConsoleColor>(c => c == color)));

        }

        [TestMethod]
        public void LogMessage_Warning_Console()
        {
            _logToConsole = true;
            _message = "WarningMessage To Log";
            var color = ConsoleColor.Yellow;
            _isWarning = true;

            var mockHelper = new Mock<IJobLoggerHelper>();
            mockHelper.Setup(c => c.WriteConsoleOutput(_message, color));
            var mockRepository = new Mock<IJobLoggerRepository>();

            var core = new JobLoggerCore(mockHelper.Object, mockRepository.Object);
            core.LogMessage(_message, _isMessage, _isWarning, _isError, _logToFile, _logToConsole, _logToDatabase);

            //Asserts
            mockHelper.Verify(x => x.WriteConsoleOutput(It.Is<string>(s => s.Contains(_message)), It.Is<ConsoleColor>(c => c == color)));

        }

        [TestMethod]
        public void LogMessage_Warning_File()
        {
            _logToFile = true;
            _message = "This is a Test_Warning Message To Log";
            _isWarning = true;

            var logMessageTowrite = DateTime.Now.ToShortDateString() + " " + _message;
            var dateFileName = DateTime.Now.ToShortDateString().Replace("/", "_");

            var mockHelper = new Mock<IJobLoggerHelper>();
            mockHelper.Setup(c => c.FileWriteLine(dateFileName, logMessageTowrite));
            var mockRepository = new Mock<IJobLoggerRepository>();

            var core = new JobLoggerCore(mockHelper.Object, mockRepository.Object);
            core.LogMessage(_message, _isMessage, _isWarning, _isError, _logToFile, _logToConsole, _logToDatabase);

            //Asserts
            mockHelper.Verify(x => x.FileWriteLine(It.Is<string>(c => c.Contains(dateFileName)), It.Is<string>(s => s.Contains(_message))), Times.Once);
        }

        [TestMethod]
        public void LogMessage_Warning_DataBase()
        {
            _logToDatabase = true;
            _message = "This is a Test_Warning Message To Log";
            _isWarning = true;
            var logValue = new LogValue()
            {
                Code = 3,
                Message = _message
            };

            var mockHelper = new Mock<IJobLoggerHelper>();

            var mockRepository = new Mock<IJobLoggerRepository>();

            mockRepository.Setup(x => x.SaveIntoLogValue(logValue));

            var core = new JobLoggerCore(mockHelper.Object, mockRepository.Object);
            core.LogMessage(_message, _isMessage, _isWarning, _isError, _logToFile, _logToConsole, _logToDatabase);

            //Asserts
            mockRepository.Verify(x => x.SaveIntoLogValue(It.Is<LogValue>(c => c.Code == 3)), Times.Once);

        }

    }
}
