namespace FluentResults.Immutable.Extensions;

public static class ResultEnumerableExtensions
{
    public static Result<IEnumerable<T>> Merge<T>(this IEnumerable<Result<T>> results) =>
        Result.Merge<T, Result<T>>(results.ToList());
}