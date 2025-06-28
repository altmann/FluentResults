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

        private static Result<IEnumerable<TValue>> MergeWithValue<TValue, TInValue>(
            IEnumerable<Result<TInValue>> results,
            Func<List<Result<TInValue>>, IEnumerable<TValue>> createValue)
        {
            var resultList = results.ToList();

            var finalResult = Result.Ok<IEnumerable<TValue>>(new List<TValue>())
                .WithReasons(resultList.SelectMany(result => result.Reasons));

            if (finalResult.IsSuccess)
                finalResult.WithValue(createValue(resultList));

            return finalResult;
        }

        public static Result<IEnumerable<TValue>> MergeWithValue<TValue>(
            IEnumerable<Result<TValue>> results)
        {
            return MergeWithValue(
                results,
                resultList => resultList.Select(r => r.Value).ToList());
        }

        public static Result<IEnumerable<TValue>> MergeWithValue<TValue, TArray>(
            IEnumerable<Result<TArray>> results) where TArray : IEnumerable<TValue>
        {
            return MergeWithValue(
                results,
                resultList => resultList.SelectMany(r => r.Value).ToList());
        }

        public static bool HasError<TError>(
            IEnumerable<IError> errors,
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
                if (HasError(error.Reasons ?? new List<IError>(), predicate, out var fErrors))
                {
                    result = fErrors;
                    return true;
                }

            result = Array.Empty<TError>();
            return false;
        }

        public static bool HasException<TException>(
            IEnumerable<IError> errors,
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
                if (HasException(error.Reasons ?? new List<IError>(), predicate, out var fErrors))
                {
                    result = fErrors;
                    return true;
                }

            result = Array.Empty<IError>();
            return false;
        }

        public static bool HasSuccess<TSuccess>(
            IEnumerable<ISuccess> successes, 
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