namespace FluentResults.Immutable.Metadata;

public record Error(
    string Message,
    ImmutableList<Error> Errors,
    ImmutableDictionary<string, object> Metadata)
    : Reason(Message, Metadata)
{
    public Error(string message)
        : this(
              message,
              ImmutableList<Error>.Empty,
              ImmutableDictionary<string, object>.Empty)
    {
    }
}
