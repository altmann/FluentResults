using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public class ExceptionalError : Error
    {
        public Exception Exception { get; }
        
        public ExceptionalError(Exception exception)
            : this(exception.Message, exception)
        { }

        public ExceptionalError(string message, Exception exception)
            : base(message)
        {
            Exception = exception;
        }

        protected override ReasonStringBuilder GetReasonStringBuilder()
        {
            return base.GetReasonStringBuilder()
                .WithInfo(nameof(Exception), Exception.ToString());
        }
    }
}