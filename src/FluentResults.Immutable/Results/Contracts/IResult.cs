using System.Collections.Immutable;
using FluentResults.Immutable.Results.Metadata;

namespace FluentResults.Immutable.Results.Contracts;

public interface IResult
{
    bool IsFailed { get; }

    bool IsSuccess { get; }

    ImmutableList<Reason> Reasons { get; }

    ImmutableList<Error> Errors { get; }

    ImmutableList<Success> Successes { get; }
}
