using FluentResults.Immutable.Metadata;

namespace FluentResults.Immutable.Contracts;

public interface IImmutableResult
{
    bool IsFailed { get; }

    bool IsSuccess { get; }

    ImmutableList<Reason> Reasons { get; }

    ImmutableList<Error> Errors { get; }

    ImmutableList<Success> Successes { get; }
}
