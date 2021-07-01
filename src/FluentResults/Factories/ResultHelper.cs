using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentResults
{
    internal static class ResultHelper
    {
        public static Result Merge(IEnumerable<ResultBase> results)
        {
            return Result.Ok().WithReasons(results.SelectMany(result => result.Reasons));
        }

        public static Result<IEnumerable<TValue>> MergeWithValue<TValue>(IEnumerable<Result<TValue>> results)
        {
            var finalResult = Result.Ok<IEnumerable<TValue>>(new List<TValue>())
                .WithReasons(results.SelectMany(result => result.Reasons));

            if (finalResult.IsSuccess)
                finalResult.WithValue(results.Select(r => r.Value).ToList());

            return finalResult;
        }

        public static bool HasError<TError>(List<Error> errors, Func<TError, bool> predicate) where TError : Error
        {
            var anyErrors = errors.Any(error => error is TError errorOfTError && predicate(errorOfTError));
            if (anyErrors)
                return true;

            foreach (var error in errors)
            {
                var anyError = HasError(error.Reasons, predicate);
                if (anyError)
                    return true;
            }

            return false;
        }

        public static bool HasSuccess<TSuccess>(List<Success> successes, Func<TSuccess, bool> predicate) where TSuccess : Success
        {
            return successes.Any(success => success is TSuccess successOfTSuccess && predicate(successOfTSuccess));
        }
    }
}