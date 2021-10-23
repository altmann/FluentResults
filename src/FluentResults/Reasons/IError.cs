using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public interface IError : IReason
    {
        /// <summary>
        /// Reasons of the error
        /// </summary>
        List<IError> Reasons { get; }
    }
}