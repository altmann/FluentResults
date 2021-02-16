using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public interface ISuccess : IReason
    {

    }

    /// <summary>
    /// Objects from Success class cause no failed result
    /// </summary>
    public class Success : ISuccess
    {
        public string Message { get; protected set; }

        public Dictionary<string, object> Metadata { get; }

        public Success(string message)
        {
            Metadata = new Dictionary<string, object>();
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

        protected ReasonStringBuilder GetReasonStringBuilder()
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