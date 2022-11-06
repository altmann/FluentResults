using System.Collections.Immutable;
using FluentResults.Immutable.Results.Metadata;

namespace FluentResults.Immutable.Extensions;

internal static class EnumerableExtensions
{
    public static IEnumerable<TReason> Flatten<TReason>(
        this IEnumerable<TReason> reasons,
        Func<TReason, ImmutableList<TReason>> childrenFunc)
        where TReason : Reason
    {
        var queue = new Queue<TReason>(reasons);
        var visited = new HashSet<TReason>();

        while (queue.TryDequeue(out var reason) && visited.Add(reason))
        {
            yield return reason;

            childrenFunc(reason).ForEach(queue.Enqueue);
        }
    }

    public static IEnumerable<T> Yield<T>(this T item)
    {
        yield return item;
    }
}
