using FluentResults.Immutable.Metadata;

namespace FluentResults.Immutable.Contracts;

public interface IImmutableResult<T>
{
    bool IsAFailure { get; }

    bool IsSuccessful { get; }

    Option<T> Value { get; }

    ImmutableList<Reason> Reasons { get; }

    ImmutableList<Error> Errors { get; }

    ImmutableList<Success> Successes { get; }
}
