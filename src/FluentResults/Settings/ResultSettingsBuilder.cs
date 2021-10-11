// ReSharper disable once CheckNamespace

using System;

namespace FluentResults
{
    public class ResultSettingsBuilder
    {
        /// <summary>
        /// Set the ResultLogger
        /// </summary>
        public IResultLogger Logger { get; set; }

        public Func<Exception, IError> DefaultTryCatchHandler { get; set; }

        public ResultSettingsBuilder()
        {
            // set defaults
            Logger = new DefaultLogger();
            DefaultTryCatchHandler = ex => new ExceptionalError(ex.Message, ex);
        }

        public ResultSettings Build()
        {
            return new ResultSettings
            {
                Logger = Logger,
                DefaultTryCatchHandler = DefaultTryCatchHandler
            };
        }
    }
}