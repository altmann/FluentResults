namespace FluentResults
{
    public interface ILogger
    {
        void Log(string context, ResultBase result);
    }
}