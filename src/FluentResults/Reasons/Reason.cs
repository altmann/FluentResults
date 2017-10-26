using System.Collections.Generic;

namespace FluentResults
{
    public class Reason
    {
        public string Message { get; protected set; }
        public Dictionary<string, object> Metadata { get; protected set; }

        protected Reason()
        {
            Metadata = new Dictionary<string, object>();
        }

        protected virtual ReasonStringBuilder GetReasonStringBuilder()
        {
            return new ReasonStringBuilder()
                .WithReasonType(GetType())
                .WithInfo(nameof(Message), Message)
                .WithInfo(nameof(Metadata), string.Join("; ", Metadata)); //todo: correct string
        }

        public override string ToString()
        {
            return GetReasonStringBuilder()
                .Build();
        }
    }
}