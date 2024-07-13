using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Settings for creating and handling result elements
    /// </summary>
    public class ResultSettings
    {
        /// <summary>
        /// Set the ResultLogger
        /// </summary>
        public IResultLogger Logger { get; set; }

        /// <summary>
        /// Factory to create an IError object.  Used in all scenarios where an error is created within Try methods
        /// </summary>
        public Func<Exception, IError> DefaultTryCatchHandler { get; set; }

        /// <summary>
        /// Factory to create an ISuccess object. Used in all scenarios where a success is created within FluentResults. 
        /// </summary>
        public Func<string, ISuccess> SuccessFactory { get; set; }

        /// <summary>
        /// Factory to create an IError object. Used in all scenarios where an error is created within FluentResults. 
        /// </summary>
        public Func<string, IError> ErrorFactory { get; set; }

        /// <summary>
        /// Factory to create an IExceptionalError object. Used in all scenarios where an exceptional error is created within FluentResults. 
        /// </summary>
        public Func<string, Exception, IExceptionalError> ExceptionalErrorFactory { get; set; }
    }
}