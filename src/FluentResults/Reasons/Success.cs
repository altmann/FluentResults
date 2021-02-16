using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Objects from Success class cause no failed result
    /// </summary>
    public record Success : Reason
    {
        public Success(string message = "")
            : base(message)
        {
        }

        public override Success WithMetadata(string metadataName, object metadataValue)
        {
            return (Success)base.WithMetadata(metadataName, metadataValue);
        }

        public override Success WithMetadata(IDictionary<string, object> metadata)
        {
            return (Success)base.WithMetadata(metadata);
        }
    }
}