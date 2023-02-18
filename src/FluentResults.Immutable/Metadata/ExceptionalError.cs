namespace FluentResults.Immutable.Metadata;

/// <summary>
///     Represents an error caused by an exception.
/// </summary>
/// <param name="CausedBy">
///     Cause of the error.
/// </param>
public sealed record ExceptionalError(Exception CausedBy)
    : Error(
        CausedBy.ToString(),
        ImmutableList<Error>.Empty,
        ImmutableDictionary<string, object>.Empty);