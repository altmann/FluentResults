using System;

namespace FluentResults.Settings
{
    public class ResultSettings
    {
        public IResultLogger Logger { get; set; }

        public Func<Exception, Error> DefaultTryCatchHandler { get; set; }
    }
}