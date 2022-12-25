namespace FluentResults.Immutable.Metadata;

public record Success(
    string Message,
    ImmutableList<Success> Successes,
    ImmutableDictionary<string, object> Metadata)
    : Reason(Message, Metadata)
{
    public Success(string message)
        : this(
              message,
              ImmutableList<Success>.Empty,
              ImmutableDictionary<string, object>.Empty)
    {
    }
}
