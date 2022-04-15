// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public class DefaultLogger : IResultLogger
    {
        public void Log(string context, string content, ResultBase result)
        {

        }

        public void Log<TContext>(string content, ResultBase result)
        {

        }
    }
}