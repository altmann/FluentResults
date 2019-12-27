using System.Collections.Generic;
using System.Linq;

namespace FluentResults
{
    internal static class ResultHelper
    {
        public static Result Merge(params ResultBase[] results)
        {
            var finalResult = Results.Ok();

            foreach (var result in results)
            {
                foreach (var reason in result.Reasons)
                {
                    finalResult.WithReason(reason);
                }
            }

            return finalResult;
        }

        public static Result<IEnumerable<TValue>> MergeWithValue<TValue>(params Result<TValue>[] results)
        {
            var finalResult = Results.Ok<IEnumerable<TValue>>();

            foreach (var result in results)
            {
                foreach (var reason in result.Reasons)
                {
                    finalResult.WithReason(reason);
                }
            }

            if (finalResult.IsSuccess)
                finalResult.WithValue(results.Select(r => r.Value).ToList());

            return finalResult;
        }
    }
}