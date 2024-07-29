using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Extensions for <see cref="IReason"/>
    /// </summary>
    public static class ReasonExtensions
    {
        /// <summary>
        /// Check if a metadata key exists
        /// </summary>
        /// <param name="reason">The reason instance</param>
        /// <param name="key">The metadata key</param>
        /// <returns>True if the metadata key exists</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool HasMetadataKey(this IReason reason, string key)
        {
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return reason.Metadata.ContainsKey(key);
        }

        /// <summary>
        /// Check if a metadata key exists and matches the supplied predicate
        /// </summary>
        /// <param name="reason">The reason instance</param>
        /// <param name="key">The metadata key</param>
        /// <param name="predicate">The predicate to check if the metadata key exists</param>
        /// <returns>True if the metadata value matches the predicate</returns>
        /// <exception cref="ArgumentNullException"></exception>
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