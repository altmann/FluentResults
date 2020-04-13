using System;
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

        public bool HasMetadataKey(string key)
        {
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return Metadata.ContainsKey(key);
        }

        public bool HasMetadata(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (Metadata.TryGetValue(key, out object actualValue))
                return actualValue == value;

            return false;
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