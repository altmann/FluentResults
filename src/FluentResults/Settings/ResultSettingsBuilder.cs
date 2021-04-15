
using System;

namespace FluentResults.Settings
{
    public class ResultSettingsBuilder
    {
        /// <summary>
        /// Set the ResultLogger
        /// </summary>
        public IResultLogger Logger { get; set; }

        public Func<Exception, Error> DefaultTryCatchHandler { get; set; }

        public ResultSettingsBuilder()
        {
            // set defaults
            Logger = new EmptyLogger();
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