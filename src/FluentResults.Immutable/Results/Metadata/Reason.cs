using System.Collections.Immutable;

namespace FluentResults.Immutable.Results.Metadata;

public record Reason(
    string Message,
    ImmutableDictionary<string, object> Metadata)
{
    public Reason(string message)
        : this(
              message,
              ImmutableDictionary<string, object>.Empty)
    {
    }
}
