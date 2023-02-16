using FluentResults.Immutable.Contracts;

namespace FluentResults.Immutable.Extensions;

public static class ResultEnumerableExtensions
{
    public static Result<IEnumerable<T>> Merge<T>(this IEnumerable<IImmutableResult<T>> results) =>
        Result.Merge(results);
}