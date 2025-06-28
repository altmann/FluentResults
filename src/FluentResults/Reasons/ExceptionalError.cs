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
        

        /// <summary>
        /// Initialize a new instance with an exception
        /// </summary>
        /// <param name="exception">The exception</param>
        public ExceptionalError(Exception exception)
            : this(exception.Message, exception)
        { }

        /// <summary>
        /// Initialize a new instance with a custom message and an exception
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="exception">The exception</param>
        public ExceptionalError(string message, Exception exception)
            : base(message)
        {
            Exception = exception;
        }

        /// <summary>
        /// ToString override
        /// </summary>
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