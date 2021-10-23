// ReSharper disable once CheckNamespace

using System;

namespace FluentResults
{
    public class ResultSettings
    {
        public IResultLogger Logger { get; set; }

        public Func<Exception, IError> DefaultTryCatchHandler { get; set; }
    }
}