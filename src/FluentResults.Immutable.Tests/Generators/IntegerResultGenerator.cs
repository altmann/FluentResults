using FsCheck;

namespace FluentResults.Immutable.Tests.Generators;

internal static class IntegerResultGenerator
{
    public static Gen<Result<int>> GetIntegerResultGenerator() =>
        Arb.Generate<int>()
            .SelectMany(static i => Gen.Elements(Result.Ok(i), Result.Fail<int>("An error")));

    public static Gen<Result<int>> GetSuccessfulIntegerResultWithValueGenerator() =>
        Arb.Generate<int>()
            .Select(static i => Result.Ok(i));

    public static Gen<Result<int>> GetSuccessfulIntegerResultWithoutValueGenerator() =>
        Gen.Fresh(static () => Result.Ok<int>());

    public static Gen<Result<int>> GetSuccessfulIntegerResultGenerator() =>
        Gen.OneOf(
            GetSuccessfulIntegerResultWithValueGenerator(),
            GetSuccessfulIntegerResultWithoutValueGenerator());
}