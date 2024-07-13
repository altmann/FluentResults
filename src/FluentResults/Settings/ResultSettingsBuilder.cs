using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Builder for <see cref="ResultSettings"/>
    /// </summary>
    public class ResultSettingsBuilder
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

        /// <summary>
        /// Default constructor that sets default values
        /// </summary>
        public ResultSettingsBuilder()
        {
            // set defaults
            Logger = new DefaultLogger();
            DefaultTryCatchHandler = ex => Result.Settings.ExceptionalErrorFactory(ex.Message, ex);
            SuccessFactory = successMessage => new Success(successMessage);
            ErrorFactory = errorMessage => new Error(errorMessage);
            ExceptionalErrorFactory = (errorMessage, exception) => new ExceptionalError(errorMessage ?? exception.Message, exception);
        }

        /// <summary>
        /// Create a <see cref="ResultSettings"/> object using the current values
        /// </summary>
        /// <returns></returns>
        public ResultSettings Build()
        {
            return new ResultSettings
            {
                Logger = Logger,
                DefaultTryCatchHandler = DefaultTryCatchHandler,
                SuccessFactory = SuccessFactory,
                ErrorFactory = ErrorFactory,
                ExceptionalErrorFactory = ExceptionalErrorFactory
            };
        }
    }
}