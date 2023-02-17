namespace FluentResults.Immutable.Tests.Extensions;

internal static class ResultExtensions
{
    public static bool ValueMatches<T>(
        this Result<T> result,
        Predicate<T> predicate) =>
        result is { Value: Some<T> { Value: var value, }, } && predicate(value);
}