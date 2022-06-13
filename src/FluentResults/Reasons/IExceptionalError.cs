using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public interface IExceptionalError : IError
    {
        Exception Exception { get; }

    }
}