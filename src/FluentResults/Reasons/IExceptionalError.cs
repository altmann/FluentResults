using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Definition of an error containing an exception
    /// </summary>
    public interface IExceptionalError : IError
    {
        /// <summary>
        /// The exception
        /// </summary>
        Exception Exception { get; }

    }
}