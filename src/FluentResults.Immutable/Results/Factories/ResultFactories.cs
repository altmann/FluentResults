using FluentResults.Immutable.Results.Metadata;

namespace FluentResults.Immutable.Results;

public readonly partial record struct Result
{
    public static Result Ok() => new();

    public static Result Fail(string errorMessage) => new(new[] { new Error(errorMessage), });

    public static Result Fail(Error error) => new(new[] { error, });
}
