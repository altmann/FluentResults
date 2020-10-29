using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Objects from Error class cause a failed result
    /// </summary>
    public class Error : Reason
    {
        /// <summary>
        /// Get the reasons of an error
        /// </summary>
        public List<Error> Reasons { get; }

        public Error()
        {
            Reasons = new List<Error>();
        }

        public Error(string message)
            : this()
        {
            Message = message;
        }

        public Error(string message, Error causedBy)
            : this(message)
        {
            Reasons.Add(causedBy);
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(Error error)
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
        public Error CausedBy(IEnumerable<Error> errors)
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
        
        protected override ReasonStringBuilder GetReasonStringBuilder()
        {
            return base.GetReasonStringBuilder()
                .WithInfo(nameof(Reasons), ReasonFormat.ErrorReasonsToString(Reasons));
        }
    }

    internal class ReasonFormat
    {
        public static string ErrorReasonsToString(List<Error> errorReasons)
        {
            return string.Join("; ", errorReasons);
        }

        public static string ReasonsToString(List<Reason> errorReasons)
        {
            return string.Join("; ", errorReasons);
        }
    }
}