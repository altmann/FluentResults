using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    internal static class ResultHelper
    {
        public static Result Merge(IEnumerable<ResultBase<IError>> results)
        {
            return Result.Ok().WithReasons(results.SelectMany(result => result.Reasons));
        }

        public static Result<IEnumerable<TValue>, IError> MergeWithValue<TValue>(IEnumerable<Result<TValue>> results)
        {
            var finalResult = Result.Ok<IEnumerable<TValue>>(new List<TValue>())
                .WithReasons(results.SelectMany(result => result.Reasons));

            if (finalResult.IsSuccess)
                finalResult.WithValue(results.Select(r => r.Value).ToList());

            return finalResult;
        }

        public static bool HasError<TError>(List<IError> errors, Func<TError, bool> predicate) where TError : IError
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

        public static bool HasSuccess<TSuccess>(List<ISuccess> successes, Func<TSuccess, bool> predicate) where TSuccess : ISuccess
        {
            return successes.Any(success => success is TSuccess successOfTSuccess && predicate(successOfTSuccess));
        }
    }
}