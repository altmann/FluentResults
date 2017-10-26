using System;
using System.Collections.Generic;

namespace FluentResults
{
    public class Error : Reason
    {
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

        public Error CausedBy(Error error)
        {
            Reasons.Add(error);
            return this;
        }
        
        public Error CausedBy(Exception exception)
        {
            Reasons.Add(new ExceptionalError(exception));
            return this;
        }

        public Error WithMessage(string message)
        {
            Message = message;
            return this;
        }

        public Error WithMetadata(string metadataName, object metadataValue)
        {
            Metadata.Add(metadataName, metadataValue);
            return this;
        }

        public Error WithMetadata(Dictionary<string, object> metadata)
        {
            foreach (var metadataItem in metadata)
            {
                Metadata.Add(metadataItem.Key, metadataItem.Value);
            }

            return this;
        }

        public Error CausedBy(string message, Exception exception)
        {
            Reasons.Add(new ExceptionalError(message, exception));
            return this;
        }

        public Error CausedBy(string message)
        {
            Reasons.Add(new Error(message));
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