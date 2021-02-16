using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public interface IError : IReason
    {
        List<IError> Reasons { get; }
    }

    /// <summary>
    /// Objects from Error class cause a failed result
    /// </summary>
    public class Error : IError
    {
        public string Message { get; protected set; }

        public Dictionary<string, object> Metadata { get; }

        /// <summary>
        /// Get the reasons of an error
        /// </summary>
        public List<IError> Reasons { get; }

        public Error()
        {
            Metadata = new Dictionary<string, object>();
            Reasons = new List<IError>();
        }

        public Error(string message)
            : this()
        {
            Message = message;
        }

        public Error(string message, IError causedBy)
            : this(message)
        {
            Reasons.Add(causedBy);
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(IError error)
        {
            Reasons.Add(error);
            return this;
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(Exception exception)
        {
            Reasons.Add(new ExceptionalError(exception));
            return this;
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(string message, Exception exception)
        {
            Reasons.Add(new ExceptionalError(message, exception));
            return this;
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(string message)
        {
            Reasons.Add(new Error(message));
            return this;
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(IEnumerable<IError> errors)
        {
            Reasons.AddRange(errors);
            return this;
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(IEnumerable<string> errors)
        {
            Reasons.AddRange(errors.Select(errorMessage => new Error(errorMessage)));
            return this;
        }

        /// <summary>
        /// Set the message
        /// </summary>
        public Error WithMessage(string message)
        {
            Message = message;
            return this;
        }

        /// <summary>
        /// Set the metadata
        /// </summary>
        public Error WithMetadata(string metadataName, object metadataValue)
        {
            Metadata.Add(metadataName, metadataValue);
            return this;
        }

        /// <summary>
        /// Set the metadata
        /// </summary>
        public Error WithMetadata(Dictionary<string, object> metadata)
        {
            foreach (var metadataItem in metadata)
            {
                Metadata.Add(metadataItem.Key, metadataItem.Value);
            }

            return this;
        }

        protected ReasonStringBuilder GetReasonStringBuilder()
        {
            return new ReasonStringBuilder()
                .WithReasonType(GetType())
                .WithInfo(nameof(Message), Message)
                .WithInfo(nameof(Metadata), string.Join("; ", Metadata)) //todo: correct string
                .WithInfo(nameof(Reasons), ReasonFormat.ErrorReasonsToString(Reasons));
        }

        public override string ToString()
        {
            return GetReasonStringBuilder()
                .Build();
        }
    }

    internal class ReasonFormat
    {
        public static string ErrorReasonsToString(List<IError> errorReasons)
        {
            return string.Join("; ", errorReasons);
        }

        public static string ReasonsToString(List<IReason> errorReasons)
        {
            return string.Join("; ", errorReasons);
        }
    }
}