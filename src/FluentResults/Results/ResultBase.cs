using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public interface IResultBase
    {
        /// <summary>
        /// Is true if Reasons contains at least one error
        /// </summary>
        bool IsFailed { get; }

        /// <summary>
        /// Is true if Reasons contains no errors
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Get all reasons (errors and successes)
        /// </summary>
        List<IReason> Reasons { get; }

        /// <summary>
        /// Get all errors
        /// </summary>
        List<IError> Errors { get; }

        /// <summary>
        /// Get all successes
        /// </summary>
        List<ISuccess> Successes { get; }
    }

    public abstract class ResultBase : IResultBase
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsFailed => Reasons.OfType<IError>().Any();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsSuccess => !IsFailed;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public List<IReason> Reasons { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public List<IError> Errors => Reasons.OfType<IError>().ToList();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public List<ISuccess> Successes => Reasons.OfType<ISuccess>().ToList();

        protected ResultBase()
        {
            Reasons = new List<IReason>();
        }

        /// <summary>
        /// Check if the result object contains an error from a specific type
        /// </summary>
        public bool HasError<TError>() where TError : IError
        {
            return HasError<TError>(out _);
        }

        /// <summary>
        /// Check if the result object contains an error from a specific type
        /// </summary>
        public bool HasError<TError>(out IEnumerable<TError> result) where TError : IError
        {
            return HasError<TError>(e => true, out result);
        }

        /// <summary>
        /// Check if the result object contains an error from a specific type and with a specific condition
        /// </summary>
        public bool HasError<TError>(Func<TError, bool> predicate) where TError : IError
        {
            return HasError<TError>(predicate, out _);
        }

        /// <summary>
        /// Check if the result object contains an error from a specific type and with a specific condition
        /// </summary>
        public bool HasError<TError>(Func<TError, bool> predicate, out IEnumerable<TError> result) where TError : IError
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasError(Errors, predicate, out result);
        }

        /// <summary>
        /// Check if the result object contains an error with a specific condition
        /// </summary>
        public bool HasError(Func<IError, bool> predicate)
        {
            return HasError(predicate, out _);
        }

        /// <summary>
        /// Check if the result object contains an error with a specific condition
        /// </summary>
        public bool HasError(Func<IError, bool> predicate, out IEnumerable<IError> result)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasError(Errors, predicate, out result);
        }

        /// <summary>
        /// Check if the result object contains an exception from a specific type
        /// </summary>
        public bool HasException<TException>() where TException : Exception
        {
            return HasException<TException>(out _);
        }

        /// <summary>
        /// Check if the result object contains an exception from a specific type
        /// </summary>
        public bool HasException<TException>(out IEnumerable<IError> result) where TException : Exception
        {
            return HasException<TException>(error => true, out result);
        }

        /// <summary>
        /// Check if the result object contains an exception from a specific type and with a specific condition
        /// </summary>
        public bool HasException<TException>(Func<TException, bool> predicate) where TException : Exception
        {
            return HasException(predicate, out _);
        }

        /// <summary>
        /// Check if the result object contains an exception from a specific type and with a specific condition
        /// </summary>
        public bool HasException<TException>(Func<TException, bool> predicate, out IEnumerable<IError> result) where TException : Exception
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasException(Errors, predicate, out result);
        }

        

        /// <summary>
        /// Check if the result object contains a success from a specific type
        /// </summary>
        public bool HasSuccess<TSuccess>() where TSuccess : ISuccess
        {
            return HasSuccess<TSuccess>(success => true, out _);
        }

        /// <summary>
        /// Check if the result object contains a success from a specific type
        /// </summary>
        public bool HasSuccess<TSuccess>(out IEnumerable<TSuccess> result) where TSuccess : ISuccess
        {
            return HasSuccess<TSuccess>(success => true, out result);
        }

        /// <summary>
        /// Check if the result object contains a success from a specific type and with a specific condition
        /// </summary>
        public bool HasSuccess<TSuccess>(Func<TSuccess, bool> predicate) where TSuccess : ISuccess
        {
            return HasSuccess(predicate, out _);
        }

        /// <summary>
        /// Check if the result object contains a success from a specific type and with a specific condition
        /// </summary>
        public bool HasSuccess<TSuccess>(Func<TSuccess, bool> predicate, out IEnumerable<TSuccess> result) where TSuccess : ISuccess
        {
            return ResultHelper.HasSuccess(Successes, predicate, out result);
        }

        /// <summary>
        /// Check if the result object contains a success with a specific condition
        /// </summary>
        public bool HasSuccess(Func<ISuccess, bool> predicate, out IEnumerable<ISuccess> result)
        {
            return ResultHelper.HasSuccess(Successes, predicate, out result);
        }

        /// <summary>
        /// Check if the result object contains a success with a specific condition
        /// </summary>
        public bool HasSuccess(Func<ISuccess, bool> predicate)
        {
            return ResultHelper.HasSuccess(Successes, predicate, out _);
        }

        /// <summary>
        /// Deconstruct Result 
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="isFailed"></param>
        public void Deconstruct(out bool isSuccess, out bool isFailed)
        {
            isSuccess = IsSuccess;
            isFailed = IsFailed;
        }

        /// <summary>
        /// Deconstruct Result
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="isFailed"></param>
        /// <param name="errors"></param>
        public void Deconstruct(out bool isSuccess, out bool isFailed, out List<IError> errors)
        {
            isSuccess = IsSuccess;
            isFailed = IsFailed;
            errors = IsFailed ? Errors : default;
        }
    }

    public abstract class ResultBase<TResult> : ResultBase
        where TResult : ResultBase<TResult>

    {
        /// <summary>
        /// Add a reason (success or error)
        /// </summary>
        public TResult WithReason(IReason reason)
        {
            Reasons.Add(reason);
            return (TResult)this;
        }

        /// <summary>
        /// Add multiple reasons (success or error)
        /// </summary>
        public TResult WithReasons(IEnumerable<IReason> reasons)
        {
            Reasons.AddRange(reasons);
            return (TResult)this;
        }

        /// <summary>
        /// Add an error
        /// </summary>
        public TResult WithError(string errorMessage)
        {
            return WithError(Result.Settings.ErrorFactory(errorMessage));
        }

        /// <summary>
        /// Add an error
        /// </summary>
        public TResult WithError(IError error)
        {
            return WithReason(error);
        }

        /// <summary>
        /// Add multiple errors
        /// </summary>
        public TResult WithErrors(IEnumerable<IError> errors)
        {
            return WithReasons(errors);
        }

        /// <summary>
        /// Add multiple errors
        /// </summary>
        public TResult WithErrors(IEnumerable<string> errors)
        {
            return WithReasons(errors.Select(errorMessage => Result.Settings.ErrorFactory(errorMessage)));
        }

        /// <summary>
        /// Add an error
        /// </summary>
        public TResult WithError<TError>()
            where TError : IError, new()
        {
            return WithError(new TError());
        }

        /// <summary>
        /// Add a success
        /// </summary>
        public TResult WithSuccess(string successMessage)
        {
            return WithSuccess(Result.Settings.SuccessFactory(successMessage));
        }

        /// <summary>
        /// Add a success
        /// </summary>
        public TResult WithSuccess(ISuccess success)
        {
            return WithReason(success);
        }

        /// <summary>
        /// Add a success
        /// </summary>
        public TResult WithSuccess<TSuccess>()
            where TSuccess : Success, new()
        {
            return WithSuccess(new TSuccess());
        }

        public TResult WithSuccesses(IEnumerable<ISuccess> successes)
        {
            foreach (var success in successes)
            {
                WithSuccess(success);
            }

            return (TResult)this;
        }

        /// <summary>
        /// Log the result. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult Log(LogLevel logLevel = LogLevel.Information)
        {
            return Log(string.Empty, null, logLevel);
        }

        /// <summary>
        /// Log the result. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult Log(string context, LogLevel logLevel = LogLevel.Information)
        {
            return Log(context, null, logLevel);
        }

        /// <summary>
        /// Log the result with a specific logger context. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult Log(string context, string content, LogLevel logLevel = LogLevel.Information)
        {
            var logger = Result.Settings.Logger;

            logger.Log(context, content, this, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// Log the result with a typed context. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult Log<TContext>(LogLevel logLevel = LogLevel.Information)
        {
            return Log<TContext>(null, logLevel);
        }

        /// <summary>
        /// Log the result with a typed context. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult Log<TContext>(string content, LogLevel logLevel = LogLevel.Information)
        {
            var logger = Result.Settings.Logger;

            logger.Log<TContext>(content, this, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// Log the result only when it is successful. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfSuccess(LogLevel logLevel = LogLevel.Information)
        {
            if (IsSuccess)
                return Log(logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// Log the result with a specific logger context only when it is successful. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfSuccess(string context, string content = null, LogLevel logLevel = LogLevel.Information)
        {
            if (IsSuccess)
                return Log(context, content, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// Log the result with a typed context only when it is successful. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfSuccess<TContext>(string content = null, LogLevel logLevel = LogLevel.Information)
        {
            if (IsSuccess)
                return Log<TContext>(content, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// Log the result only when it is failed. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfFailed(LogLevel logLevel = LogLevel.Error)
        {
            if (IsFailed)
                return Log(logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// Log the result with a specific logger context only when it is failed. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfFailed(string context, string content = null, LogLevel logLevel = LogLevel.Error)
        {
            if (IsFailed)
                return Log(context, content, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// Log the result with a typed context only when it is failed. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfFailed<TContext>(string content = null, LogLevel logLevel = LogLevel.Error)
        {
            if (IsFailed)
                return Log<TContext>(content, logLevel);

            return (TResult)this;
        }

        public override string ToString()
        {
            var reasonsString = Reasons.Any()
                                    ? $", Reasons='{ReasonFormat.ReasonsToString(Reasons)}'"
                                    : string.Empty;

            return $"Result: IsSuccess='{IsSuccess}'{reasonsString}";
        }
    }
}