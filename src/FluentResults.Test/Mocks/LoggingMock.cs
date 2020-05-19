namespace FluentResults.Test.Mocks
{
    public class LoggingMock : IResultLogger
    {
        public void Log(string context, ResultBase result)
        {
            LoggedContext = context;
            LoggedResult = result;
        }

        public string LoggedContext { get; private set; }
        public ResultBase LoggedResult { get; private set; }
    }
}
