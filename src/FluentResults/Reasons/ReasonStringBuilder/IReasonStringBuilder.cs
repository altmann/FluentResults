using System;

namespace FluentResults.ReasonStringBuilder
{
    public interface IReasonStringBuilder
    {
        IReasonStringBuilder WithReasonType(Type type);
        IReasonStringBuilder WithInfo(string label, string value);
        string Build();
    }
}