using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public static class EnumerableExtensions
    {
        public static Result Merge(this IEnumerable<Result> results)
        {
            return ResultHelper.Merge(results.ToArray());
        }

        public static Result<IEnumerable<TValue>> Merge<TValue>(this IEnumerable<Result<TValue>> results)
        {
            return ResultHelper.MergeWithValue(results.ToArray());
        }
    }
}