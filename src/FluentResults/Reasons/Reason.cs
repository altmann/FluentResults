using System.Collections.Generic;

namespace FluentResults
{
    public class Reason
    {
        public string Message { get; protected set; }
        public List<string> Tags { get; protected set; }

        protected Reason()
        {
            Tags = new List<string>();
        }

        protected virtual ReasonStringBuilder GetReasonStringBuilder()
        {
            return new ReasonStringBuilder()
                .WithReasonType(GetType())
                .WithInfo(nameof(Message), Message)
                .WithInfo(nameof(Tags), string.Join("; ", Tags));
        }

        public override string ToString()
        {
            return GetReasonStringBuilder()
                .Build();
        }
    }
}