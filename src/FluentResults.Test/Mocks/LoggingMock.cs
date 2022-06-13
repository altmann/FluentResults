namespace FluentResults.Test.Mocks
{
    public class LoggingMock : IResultLogger
    {
        public void Log(string context, string content, ResultBase result)
        {
            LoggedContext = context;
            LoggedContent = content;
            LoggedResult = result;
        }

        public void Log<TContext>(string content, ResultBase result)
        {
            LoggedContext = typeof(TContext).ToString();
            LoggedContent = content;
            LoggedResult = result;
        }

        public string LoggedContext { get; private set; }
        public string LoggedContent { get; private set; }
        public ResultBase LoggedResult { get; private set; }
    }
}