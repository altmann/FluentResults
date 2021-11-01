namespace FluentResults.Test.Mocks
{
    public class LoggingMock : IResultLogger
    {
        public void Log(string context, ResultBase<IError> result)
        {
            LoggedContext = context;
            LoggedResult = result;
        }

        public void Log<TContext>(ResultBase<IError> result)
        {
            LoggedContext = typeof(TContext).ToString();
            LoggedResult = result;
        }

        public string LoggedContext { get; private set; }
        public ResultBase<IError> LoggedResult { get; private set; }
    }
}