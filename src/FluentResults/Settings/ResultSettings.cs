using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public class ResultSettings
    {
        public IResultLogger Logger { get; set; }

        public Func<Exception, IError> DefaultTryCatchHandler { get; set; }

        public Func<string, ISuccess> SuccessFactory { get; set; }

        public Func<string, IError> ErrorFactory { get; set; }
        
        public Func<IEnumerable<string>, IEnumerable<IError>> MultipleErrorFactory { get; set; }

        public Func<string, Exception, IExceptionalError> ExceptionalErrorFactory { get; set; }
    }
}