using System.Collections.Generic;
using System.Linq;

namespace FluentResults
{
    internal static class ResultHelper
    {
        public static TResult Merge<TResult>(params ResultBase[] results)
            where TResult : ResultBase<TResult>, new()
        {
            var finalResult = new TResult();

            foreach (var result in results)
            {
                foreach (var reason in result.Reasons)
                {
                    finalResult.WithReason(reason);
                }
            }

            return finalResult;
        }

        public static Result<IEnumerable<TValue>> Merge<TResult, TValue>(params ValueResultBase<TResult, TValue>[] results)
            where TResult : ValueResultBase<TResult, TValue>, new()
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