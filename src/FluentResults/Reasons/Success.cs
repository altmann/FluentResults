using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Objects from Success class cause no failed result
    /// </summary>
    public class Success : ISuccess
    {
        /// <summary>
        /// Message of the success
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// Metadata of the success
        /// </summary>
        public Dictionary<string, object> Metadata { get; }

        protected Success()
        {
            Metadata = new Dictionary<string, object>();
        }

        public Success(string message) : this()
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

        public override string ToString()
        {
            return new ReasonStringBuilder()
                .WithReasonType(GetType())
                .WithInfo(nameof(Message), Message)
                .WithInfo(nameof(Metadata), string.Join("; ", Metadata))
                .Build();
        }
    }
}