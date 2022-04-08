using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public class ResultSettingsBuilder
    {
        /// <summary>
        /// Set the ResultLogger
        /// </summary>
        public IResultLogger Logger { get; set; }
        
        public Func<Exception, IError> DefaultTryCatchHandler { get; set; }

        /// <summary>
        /// Factory to create an ISuccess object. Used in all scenarios where a success is created within FluentResults. 
        /// </summary>
        public Func<string, ISuccess> SuccessFactory { get; set; }

        /// <summary>
        /// Factory to create an IError object. Used in all scenarios where an error is created within FluentResults. 
        /// </summary>
        public Func<string, IError> ErrorFactory { get; set; }

        public ResultSettingsBuilder()
        {
            // set defaults
            Logger = new DefaultLogger();
            DefaultTryCatchHandler = ex => new ExceptionalError(ex.Message, ex);
            SuccessFactory = successMessage => new Success(successMessage);
            ErrorFactory = errorMessage => new Error(errorMessage);
        }

        public ResultSettings Build()
        {
            return new ResultSettings
            {
                Logger = Logger,
                DefaultTryCatchHandler = DefaultTryCatchHandler,
                SuccessFactory = SuccessFactory,
                ErrorFactory = ErrorFactory
            };
        }
    }
}