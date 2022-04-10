using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Error class which stores additionally the exception
    /// </summary>
    public class ExceptionalError : Error, IExceptionalError
    {
        /// <summary>
        /// Exception of the error
        /// </summary>
        public Exception Exception { get; }
        
        public ExceptionalError(Exception exception)
            : this(exception.Message, exception)
        { }

        public ExceptionalError(string message, Exception exception)
            : base(message)
        {
            Exception = exception;
        }

        public override string ToString()
        {
            return new ReasonStringBuilder()
                .WithReasonType(GetType())
                .WithInfo(nameof(Message), Message)
                .WithInfo(nameof(Metadata), string.Join("; ", Metadata))
                .WithInfo(nameof(Reasons), ReasonFormat.ErrorReasonsToString(Reasons))
                .WithInfo(nameof(Exception), Exception.ToString())
                .Build();
        }
    }
}