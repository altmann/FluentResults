using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
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
    }
}