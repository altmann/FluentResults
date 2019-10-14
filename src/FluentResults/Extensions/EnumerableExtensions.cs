using System.Collections.Generic;
using System.Linq;

namespace FluentResults
{
    public static class EnumerableExtensions
    {
        public static Result Merge(this IEnumerable<Result> results)
        {
            return ResultHelper.Merge<Result>(results.ToArray());
        }

        public static Result Merge<TValue>(this IEnumerable<Result<TValue>> results)
        {
            return ResultHelper.Merge<Result<TValue>>(results.ToArray());
        }
    }
}