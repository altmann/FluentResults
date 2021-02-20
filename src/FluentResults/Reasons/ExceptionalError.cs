using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Error class which stores additionally the exception
    /// </summary>
    public record ExceptionalError : Error
    {
        public Exception Exception { get; }

        public ExceptionalError(Exception exception, string? message = null)
            : base(message ?? exception.Message)
        {
            Exception = exception;
        }

        public override ExceptionalError WithMetadata(string metadataName, object metadataValue)
        {
            return (ExceptionalError)base.WithMetadata(metadataName, metadataValue);
        }

        public override ExceptionalError WithMetadata(IDictionary<string, object> metadata)
        {
            return (ExceptionalError)base.WithMetadata(metadata);
        }

    }
}