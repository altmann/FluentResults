using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FluentResults
{
    public class Error : Reason
    {
        public string ErrorCode { get; protected set; }
        public List<Error> Reasons { get; }
        public HttpStatusCode HttpStatusCode { get; protected set; }

        public Error()
        {
            Reasons = new List<Error>();
            ErrorCode = string.Empty;
        }

        public Error(string message)
            : this()
        {
            Message = message;
        }

        public Error(string errorCode, string message)
            : this()
        {
            Message = message;
            ErrorCode = errorCode;
        }
        
        public Error(string errorCode, string message, HttpStatusCode httpStatusCode)
            : this()
        {
            ErrorCode = errorCode;
            Message = message;
            HttpStatusCode = httpStatusCode;
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

        public Error WithTag(string tag)
        {
            Tags.Add(tag);
            return this;
        }

        public Error WithTags(params string[] tags)
        {
            Tags.AddRange(tags);
            return this;
        }

        public Error WithHttpStatusCode(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
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

        public Error WithErrorCode(string errorCode)
        {
            ErrorCode = errorCode;
            return this;
        }

        protected override ReasonStringBuilder GetReasonStringBuilder()
        {
            return base.GetReasonStringBuilder()
                .WithInfo(nameof(ErrorCode), ErrorCode)
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