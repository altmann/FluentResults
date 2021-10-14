using System.Collections.Generic;

namespace FluentResults
{
    public interface IError : IReason
    {
        List<IError> Reasons { get; }
    }
}