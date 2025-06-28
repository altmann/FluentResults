// ReSharper disable once CheckNamespace

using Microsoft.Extensions.Logging;

namespace FluentResults
{
    /// <summary>
    /// Logging interface.  Implement this if you want to have custom logging of results
    /// </summary>
    public interface IResultLogger
    {
        /// <summary>
        /// Log result information
        /// </summary>
        /// <param name="context">Additional log context</param>
        /// <param name="content">Content to log</param>
        /// <param name="result">The result to log</param>
        /// <param name="logLevel">The <see cref="Microsoft.Extensions.Logging.LogLevel"/></param>
        void Log(string context, string content, ResultBase result, LogLevel logLevel);

        /// <summary>
        /// Log result information
        /// </summary>
        /// <typeparam name="TContext">Additional log context</typeparam>
        /// <param name="content">Content to log</param>
        /// <param name="result">The result to log</param>
        /// <param name="logLevel">The <see cref="Microsoft.Extensions.Logging.LogLevel"/></param>
        void Log<TContext>(string content, ResultBase result, LogLevel logLevel);
    }
}