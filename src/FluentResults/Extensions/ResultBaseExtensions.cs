using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentResults
{
    /// <summary>
    /// Extensions methods for IResultBase
    /// </summary>
    /// <remarks>
    /// Adds methods, that are else defined in ResultBase, so that 
    /// they are also available for IResultBase.
    /// </remarks>
    public static class ResultBaseExtensions
    {
        #region Methods from ResultBase

        /// <inheritdoc cref="ResultBase.HasError{TError}()"/>
        public static bool HasError<TError>(this IResultBase resultBase) where TError : IError
        {
            return HasError<TError>(resultBase, out _);
        }

        /// <inheritdoc cref="ResultBase.HasError{TError}(out IEnumerable{TError})"/>
        public static bool HasError<TError>(this IResultBase resultBase, out IEnumerable<TError> result) where TError : IError
        {
            return HasError<TError>(resultBase, e => true, out result);
        }

        /// <inheritdoc cref="ResultBase.HasError{TError}(Func{TError, bool})"/>
        public static bool HasError<TError>(this IResultBase resultBase, Func<TError, bool> predicate) where TError : IError
        {
            return HasError<TError>(resultBase, predicate, out _);
        }

        /// <inheritdoc cref="ResultBase.HasError{TError}(Func{TError, bool}, out IEnumerable{TError})"/>
        public static bool HasError<TError>(this IResultBase resultBase, Func<TError, bool> predicate, out IEnumerable<TError> result) where TError : IError
        {
            if(predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasError(resultBase.Errors, predicate, out result);
        }

        /// <inheritdoc cref="ResultBase.HasError(Func{IError, bool})"/>
        public static bool HasError(this IResultBase resultBase, Func<IError, bool> predicate)
        {
            return HasError(resultBase, predicate, out _);
        }

        /// <inheritdoc cref="ResultBase.HasError(Func{IError, bool}, out IEnumerable{IError})"/>
        public static bool HasError(this IResultBase resultBase, Func<IError, bool> predicate, out IEnumerable<IError> result)
        {
            if(predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasError(resultBase.Errors, predicate, out result);
        }

        /// <inheritdoc cref="ResultBase.HasException{TException}()"/>
        public static bool HasException<TException>(this IResultBase resultBase) where TException : Exception
        {
            return HasException<TException>(resultBase, out _);
        }

        /// <inheritdoc cref="ResultBase.HasException{TException}(out IEnumerable{IError})"/>
        public static bool HasException<TException>(this IResultBase resultBase, out IEnumerable<IError> result) where TException : Exception
        {
            return HasException<TException>(resultBase, error => true, out result);
        }

        /// <inheritdoc cref="ResultBase.HasException{TException}(Func{TException, bool})"/>
        public static bool HasException<TException>(this IResultBase resultBase, Func<TException, bool> predicate) where TException : Exception
        {
            return HasException(resultBase, predicate, out _);
        }

        /// <inheritdoc cref="ResultBase.HasException{TException}(Func{TException, bool}, out IEnumerable{IError})"/>
        public static bool HasException<TException>(this IResultBase resultBase, Func<TException, bool> predicate, out IEnumerable<IError> result) where TException : Exception
        {
            if(predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasException(resultBase.Errors, predicate, out result);
        }

        /// <inheritdoc cref="ResultBase.HasSuccess{TSuccess}()" />
        public static bool HasSuccess<TSuccess>(this IResultBase resultBase) where TSuccess : ISuccess
        {
            return HasSuccess<TSuccess>(resultBase, success => true, out _);
        }

        /// <inheritdoc cref="ResultBase.HasSuccess{TSuccess}(out IEnumerable{TSuccess})"/>
        public static bool HasSuccess<TSuccess>(this IResultBase resultBase, out IEnumerable<TSuccess> result) where TSuccess : ISuccess
        {
            return HasSuccess<TSuccess>(resultBase, success => true, out result);
        }

        /// <inheritdoc cref="ResultBase.HasSuccess{TSuccess}(Func{TSuccess, bool})"/>
        public static bool HasSuccess<TSuccess>(this IResultBase resultBase, Func<TSuccess, bool> predicate) where TSuccess : ISuccess
        {
            return HasSuccess(resultBase, predicate, out _);
        }

        /// <inheritdoc cref="ResultBase.HasSuccess{TSuccess}(Func{TSuccess, bool}, out IEnumerable{TSuccess})"/>
        public static bool HasSuccess<TSuccess>(this IResultBase resultBase, Func<TSuccess, bool> predicate, out IEnumerable<TSuccess> result) where TSuccess : ISuccess
        {
            return ResultHelper.HasSuccess(resultBase.Successes, predicate, out result);
        }

        /// <inheritdoc cref="ResultBase.HasSuccess(Func{ISuccess, bool}, out IEnumerable{ISuccess})"/>
        public static bool HasSuccess(this IResultBase resultBase, Func<ISuccess, bool> predicate, out IEnumerable<ISuccess> result)
        {
            return ResultHelper.HasSuccess(resultBase.Successes, predicate, out result);
        }

        /// <inheritdoc cref="ResultBase.HasSuccess{TSuccess}()"/>
        public static bool HasSuccess(this IResultBase resultBase, Func<ISuccess, bool> predicate)
        {
            return ResultHelper.HasSuccess(resultBase.Successes, predicate, out _);
        }

        /// <inheritdoc cref="ResultBase.Deconstruct(out bool, out bool)"/>
        public static void Deconstruct(this IResultBase resultBase, out bool isSuccess, out bool isFailed)
        {
            isSuccess = resultBase.IsSuccess;
            isFailed = resultBase.IsFailed;
        }

        /// <inheritdoc cref="ResultBase.Deconstruct(out bool, out bool, out List{IError})"/>
        public static void Deconstruct(this IResultBase resultBase, out bool isSuccess, out bool isFailed, out IReadOnlyList<IError> errors)
        {
            isSuccess = resultBase.IsSuccess;
            isFailed = resultBase.IsFailed;
            errors = isFailed ? resultBase.Errors : (IReadOnlyList<IError>)Array.Empty<IError>();
        }

        #endregion


        #region Methods from ResultBase<TResult> (return the same type, for fluent syntax)

        /// <inheritdoc cref="ResultBase{TResult}.WithReason(IReason)"/>
        public static TResult WithReason<TResult>(this TResult result, IReason reason)
            where TResult : IResultBase
        {
            result.Reasons.Add(reason);
            return result;
        }

        /// <inheritdoc cref="ResultBase{TResult}.WithReasons(IEnumerable{IReason})"/>
        public static TResult WithReasons<TResult>(this TResult result, IEnumerable<IReason> reasons)
            where TResult : IResultBase
        {
            result.Reasons.AddRange(reasons);
            return result;
        }

        /// <inheritdoc cref="ResultBase{TResult}.WithError(string)"/>
        public static TResult WithError<TResult>(this TResult result, string errorMessage)
            where TResult : IResultBase
        {
            return result.WithError(Result.Settings.ErrorFactory(errorMessage));
        }

        /// <inheritdoc cref="ResultBase{TResult}.WithError(IError)"/>
        public static TResult WithError<TResult>(this TResult result, IError error)
            where TResult : IResultBase
        {
            return result.WithReason(error);
        }

        /// <inheritdoc cref="ResultBase{TResult}.WithErrors(IEnumerable{IError})"/>
        public static TResult WithErrors<TResult>(this TResult result, IEnumerable<IError> errors)
            where TResult : IResultBase
        {
            return result.WithReasons(errors);
        }

        /// <inheritdoc cref="ResultBase{TResult}.WithErrors(IEnumerable{string})"/>
        public static TResult WithErrors<TResult>(this TResult result, IEnumerable<string> errors)
            where TResult : IResultBase
        {
            return result.WithReasons(errors.Select(errorMessage => Result.Settings.ErrorFactory(errorMessage)));
        }

        /// <inheritdoc cref="ResultBase{TResult}.WithSuccess(string)"/>
        public static TResult WithSuccess<TResult>(this TResult result, string successMessage)
            where TResult : IResultBase
        {
            return result.WithSuccess(Result.Settings.SuccessFactory(successMessage));
        }

        /// <inheritdoc cref="ResultBase{TResult}.WithSuccess(ISuccess)"/>
        public static TResult WithSuccess<TResult>(this TResult result, ISuccess success)
            where TResult : IResultBase
        {
            return result.WithReason(success);
        }

        /// <inheritdoc cref="ResultBase{TResult}.WithSuccesses(IEnumerable{ISuccess})"/>
        public static TResult WithSuccesses<TResult>(this TResult result, IEnumerable<ISuccess> successes)
            where TResult : IResultBase
        {
            foreach(var success in successes)
            {
                result.WithSuccess(success);
            }

            return result;
        }


        #endregion

    }

}