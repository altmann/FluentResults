using Microsoft.Extensions.Logging;

namespace FluentResults.Test.Mocks
{
    public class LoggingMock : IResultLogger
    {
        public void Log(string context, string content, ResultBase result, LogLevel logLevel = LogLevel.Information)
        {
            LoggedContext = context;
            LoggedContent = content;
            LoggedResult = result;
            LoggedLevel = logLevel;
        }

        public void Log<TContext>(string content, ResultBase result, LogLevel logLevel = LogLevel.Information)
        {
            LoggedContext = typeof(TContext).ToString();
            LoggedContent = content;
            LoggedResult = result;
            LoggedLevel = logLevel;
        }

        public string LoggedContext { get; private set; }
        public string LoggedContent { get; private set; }
        public ResultBase LoggedResult { get; private set; }
        public LogLevel LoggedLevel { get; private set; }
    }
}