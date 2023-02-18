namespace FluentResults.Immutable.Extensions;

/// <summary>
///     Contains extension methods for <see cref="IEnumerable{T}" />
///     of <see cref="Result{T}" />.
/// </summary>
public static class ResultEnumerableExtensions
{
    /// <inheritdoc cref="Result.Merge{T, TResult}(IReadOnlyCollection{TResult})" />.
    public static Result<IEnumerable<T>> Merge<T>(this IEnumerable<Result<T>> results) =>
        Result.Merge<T, Result<T>>(results.ToList());
}