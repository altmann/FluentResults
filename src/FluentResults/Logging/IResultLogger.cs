// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public interface IResultLogger
    {
        void Log(string context, string content, ResultBase result);
        void Log<TContext>(string content, ResultBase result);
    }
}