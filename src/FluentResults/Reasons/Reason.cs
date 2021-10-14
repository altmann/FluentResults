using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public interface IReason
    {
        string Message { get; }

        Dictionary<string, object> Metadata { get; }
    }
    
    public static class ReasonExtensions
    {
        public static bool HasMetadataKey(this IReason reason, string key)
        {
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return reason.Metadata.ContainsKey(key);
        }

        public static bool HasMetadata(this IReason reason, string key, Func<object, bool> predicate)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (reason.Metadata.TryGetValue(key, out object actualValue))
                return predicate(actualValue);

            return false;
        }

        //protected virtual ReasonStringBuilder GetReasonStringBuilder()
        //{
        //    return new ReasonStringBuilder()
        //        .WithReasonType(GetType())
        //        .WithInfo(nameof(Message), Message)
        //        .WithInfo(nameof(Metadata), string.Join("; ", Metadata)); //todo: correct string
        //}

        //public override string ToString()
        //{
        //    return GetReasonStringBuilder()
        //        .Build();
        //}
    }
}