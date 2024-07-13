// ReSharper disable once CheckNamespace

using Microsoft.Extensions.Logging;

namespace FluentResults
{
    /// <summary>
    /// Default implementation of <see cref="IResultLogger"/> that doesn't log anything
    /// </summary>
    public class DefaultLogger : IResultLogger
    {
        /// <inheritdoc/>
        public void Log(string context, string content, ResultBase result, LogLevel logLevel)
        {

        }

        /// <inheritdoc/>
        public void Log<TContext>(string content, ResultBase result, LogLevel logLevel)
        {

        }
    }
}