using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public interface IError : IReason
    {
        List<IError> Reasons { get; }
    }
}