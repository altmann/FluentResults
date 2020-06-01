using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public class Success : Reason
    {
        public Success(string message)
        {
            Message = message;
        }

        public Success WithMetadata(string metadataName, object metadataValue)
        {
            Metadata.Add(metadataName, metadataValue);
            return this;
        }

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