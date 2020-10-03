using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Objects from Success class cause no failed result
    /// </summary>
    public class Success : Reason
    {
        public Success(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Set the metadata
        /// </summary>
        public Success WithMetadata(string metadataName, object metadataValue)
        {
            Metadata.Add(metadataName, metadataValue);
            return this;
        }

        /// <summary>
        /// Set the metadata
        /// </summary>
        public Success WithMetadata(Dictionary<string, object> metadata)
        {
            foreach (var metadataItem in metadata)
            {
                Metadata.Add(metadataItem.Key, metadataItem.Value);
            }
            
            return this;
        }
    }
}