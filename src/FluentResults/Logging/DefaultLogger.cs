// ReSharper disable once CheckNamespace

using Microsoft.Extensions.Logging;

namespace FluentResults
{
    public class DefaultLogger : IResultLogger
    {
        public void Log(string context, string content, ResultBase result, LogLevel logLevel)
        {

        }

        public void Log<TContext>(string content, ResultBase result, LogLevel logLevel)
        {

        }
    }
}