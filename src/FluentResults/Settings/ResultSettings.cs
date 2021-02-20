// ReSharper disable once CheckNamespace

using System;

namespace FluentResults
{
    public class ResultSettings
    {
        public IResultLogger Logger { get; set; } = null!;

        public Func<Exception, Error> DefaultTryCatchHandler { get; set; } = null!;
    }
}