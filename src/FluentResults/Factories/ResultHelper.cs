using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    internal static class ResultHelper
    {
        public static Result Merge(IEnumerable<ResultBase> results)
        {
            return Result.Ok().WithReasons(results.SelectMany(result => result.Reasons));
        }

        public static Result<IEnumerable<TValue>> MergeWithValue<TValue>(
            IEnumerable<Result<TValue>> results)
        {
            var resultList = results.ToList();

            var finalResult = Result.Ok<IEnumerable<TValue>>(new List<TValue>())
                                    .WithReasons(resultList.SelectMany(result => result.Reasons));

            if (finalResult.IsSuccess)
                finalResult.WithValue(resultList.Select(r => r.Value).ToList());

            return finalResult;
        }

        public static bool HasError<TError>(
            List<IError> errors,
            Func<TError, bool> predicate,
            out IEnumerable<TError> result)
            where TError : IError
        {
            var foundErrors = errors.OfType<TError>().Where(predicate).ToList();
            if (foundErrors.Any())
            {
                result = foundErrors;
                return true;
            }

            foreach (var error in errors)
            {
                if (HasError(error.Reasons, predicate, out var fErrors))
                {
                    result = fErrors;
                    return true;
                }
            }

            result = Array.Empty<TError>();
            return false;
        }

        public static bool HasException<TException>(
            List<IError> errors,
            Func<TException, bool> predicate,
            out IEnumerable<IError> result)
            where TException : Exception
        {
            var foundErrors = errors.OfType<ExceptionalError>()
                                    .Where(e => e.Exception is TException rootExceptionOfTException
                                                && predicate(rootExceptionOfTException))
                                    .ToList();

            if (foundErrors.Any())
            {
                result = foundErrors;
                return true;
            }

            foreach (var error in errors)
            {
                if (HasException(error.Reasons, predicate, out var fErrors))
                {
                    result = fErrors;
                    return true;
                }
            }

            result = Array.Empty<IError>();
            return false;
        }

        public static bool HasSuccess<TSuccess>(
            List<ISuccess> successes, 
            Func<TSuccess, bool> predicate,
            out IEnumerable<TSuccess> result) where TSuccess : ISuccess
        {
            var foundSuccesses = successes.OfType<TSuccess>()
                                          .Where(predicate)
                                          .ToList();
            if (foundSuccesses.Any())
            {
                result = foundSuccesses;
                return true;
            }

            result = Array.Empty<TSuccess>();
            return false;
        }
    }
}