using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Merge multiple result objects to one result together
        /// </summary>
        public static Result Merge(this IEnumerable<Result> results)
        {
            return ResultHelper.Merge(results.ToArray());
        }

        /// <summary>
        /// Merge multiple result objects to one result together
        /// </summary>
        public static Result<IEnumerable<TValue>> Merge<TValue>(this IEnumerable<Result<TValue>> results)
        {
            return ResultHelper.MergeWithValue(results.ToArray());
        }
    }
}