namespace FluentResults.Immutable.Metadata;

public sealed record ExceptionalError(Exception CausedBy)
    : Error(
        CausedBy.ToString(),
        ImmutableList<Error>.Empty,
        ImmutableDictionary<string, object>.Empty);