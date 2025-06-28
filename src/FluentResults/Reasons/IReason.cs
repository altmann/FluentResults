using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Definition of a reason.  This is also the base of ISuccess and IError
    /// </summary>
    public interface IReason
    {
        /// <summary>
        /// The reason's message
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Any metadata added to the reason
        /// </summary>
        Dictionary<string, object> Metadata { get; }
    }
}