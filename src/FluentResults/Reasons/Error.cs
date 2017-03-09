using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentResults
{
    public class Error : Reason
    {
        public string ErrorCode { get; protected set; }
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

        public Error CausedBy(string message, Exception exception)
        {
            Reasons.Add(new ExceptionalError(message, exception));
            return this;
        }

        protected override ReasonStringBuilder GetReasonStringBuilder()
        {
            return base.GetReasonStringBuilder()
                .WithInfo(nameof(ErrorCode), ErrorCode)
                .WithInfo(nameof(Message), Message)
                .WithInfo(nameof(Reasons), ReasonFormat.ErrorReasonsToString(Reasons));
        }
    }

    internal class ReasonFormat
    {
        public static string ErrorReasonsToString(List<Error> errorReasons)
        {
            return string.Join(", ", errorReasons);
        }

        public static string ReasonsToString(List<Reason> errorReasons)
        {
            return string.Join(", ", errorReasons);
        }
    }

    public class ReasonStringBuilder
    {
        private string reasonType = string.Empty;
        private readonly List<string> infos = new List<string>();
         
        public ReasonStringBuilder WithReasonType(Type type)
        {
            reasonType = type.Name;
            return this;
        }

        public ReasonStringBuilder WithInfo(string label, string value)
        {
            infos.Add(value.ToLabelValueStringOrEmpty(label));
            return this;
        }

        public string Build()
        {
            var reasonInfoText = infos.Any()
                ? " with " + ReasonInfosToString(infos)
                : " " + ReasonInfosToString(infos);

            return $"{reasonType} {reasonInfoText}";
        }

        private static string ReasonInfosToString(List<string> reasonInfos)
        {
            return string.Join(", ", reasonInfos);
        }
    }
}