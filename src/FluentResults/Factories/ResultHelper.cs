using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    internal static class ResultHelper
    {
        public static Result Merge(params ResultBase[] results)
        {
            return Merge(results.AsEnumerable());
        }

        public static Result Merge(IEnumerable<ResultBase> results)
        {
            var finalResult = Result.Ok();

            foreach (var result in results.SelectMany(r => r.Reasons))
            {
                finalResult.WithReason(result);
            }

            return finalResult;
        }

        public static Result<IEnumerable<TValue>> Merge<TValue>(params Result<TValue>[] results)
        {
            return Merge(results.AsEnumerable());
        }

        public static Result<IEnumerable<TValue>> Merge<TValue>(IEnumerable<Result<TValue>> results)
        {
            var finalResult = Result.Ok<IEnumerable<TValue>>(new List<TValue>());

            foreach (var reason in results.SelectMany(r => r.Reasons))
            {
                finalResult.WithReason(reason);
            }

            if (finalResult.IsSuccess)
                finalResult.WithValue(results.Select(r => r.Value).ToList());

            return finalResult;
        }

        public static bool HasError<TError>(IReadOnlyList<Error> errors, Func<TError, bool> predicate) where TError : Error
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