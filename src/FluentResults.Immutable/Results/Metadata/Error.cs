using System.Collections.Immutable;

namespace FluentResults.Immutable.Results.Metadata;

public record Error(
    string Message,
    ImmutableList<Error> Errors,
    ImmutableDictionary<string, object> Metadata)
    : Reason(Message, Metadata)
{
    public Error(string errorMessage)
        : this(
              errorMessage,
              ImmutableList<Error>.Empty,
              ImmutableDictionary<string, object>.Empty)
    {
    }
}
