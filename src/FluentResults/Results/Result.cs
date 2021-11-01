using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public partial class Result : ResultBase<Result, IError>
    {
        public Result()
        { }

        public Result<TNewValue> ToResult<TNewValue>(TNewValue newValue = default)
        {
            return (Result<TNewValue>)new Result<TNewValue>()
                .WithValue(IsFailed ? default : newValue)
                .WithReasons(Reasons);
        }

        public Result<TNewValue, TError> ToResult<TNewValue, TError>(TNewValue newValue = default)
            where TError : IError
        {
            return new Result<TNewValue, TError>()
                .WithValue(IsFailed ? default : newValue)
                .WithReasons(Reasons);
        }

        /// <summary>
        /// Add an error
        /// </summary>
        public Result WithError(string errorMessage)
        {
            return WithError(new Error(errorMessage));
        }

        /// <summary>
        /// Add multiple errors
        /// </summary>
        public Result WithErrors(IEnumerable<string> errors)
        {
            return WithReasons(errors.Select(errorMessage => new Error(errorMessage)));
        }

        /// <summary>
        /// Check if the result object contains an error from a specific type
        /// </summary>
        public bool HasError<TError>() where TError : IError
        {
            return HasError<TError>(error => true);
        }

        /// <summary>
        /// Check if the result object contains an error from a specific type and with a specific condition
        /// </summary>
        public bool HasError<TError>(Func<TError, bool> predicate) where TError : IError
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasError(Errors as List<IError>, predicate);
        }
    }

    public class Result<TValue> : Result<TValue, IError>
    {
        /// <summary>
        /// Add an error
        /// </summary>
        public Result<TValue> WithError(string errorMessage)
        {
            return (Result<TValue>)WithError(new Error(errorMessage));
        }

        public static implicit operator Result<TValue>(Result result)
        {
            return result.ToResult<TValue>();
        }
    }

    public class Result<TValue, TError> : ResultBase<Result<TValue, TError>, TError>
        where TError : IError
    {
        public Result()
        { }

        private TValue _value;

        /// <summary>
        /// Get the Value. If result is failed then a default value is returned. Opposite see property Value.
        /// </summary>
        public TValue ValueOrDefault => _value;

        /// <summary>
        /// Get the Value. If result is failed then an Exception is thrown because a failed result has no value. Opposite see property ValueOrDefault.
        /// </summary>
        public TValue Value
        {
            get
            {
                if (IsFailed)
                    throw new InvalidOperationException("Result is in status failed. Value is not set.");

                return _value;
            }
            private set
            {
                if (IsFailed)
                    throw new InvalidOperationException("Result is in status failed. Value is not set.");

                _value = value;
            }
        }

        /// <summary>
        /// Set value
        /// </summary>
        public Result<TValue, TError> WithValue(TValue value)
        {
            Value = value;
            return this;
        }

        /// <summary>
        /// Convert result with value to result without value
        /// </summary>
        public Result ToResult()
        {
            return new Result()
                .WithReasons(Reasons);
        }

        /// <summary>
        /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
        /// </summary>
        public Result<TNewValue, TError> ToResult<TNewValue>(Func<TValue, TNewValue> valueConverter = null)
        {
            if(IsSuccess && valueConverter == null)
                throw new ArgumentException("If result is success then valueConverter should not be null");

            return new Result<TNewValue, TError>()
                .WithValue(IsFailed ? default : valueConverter(Value))
                .WithReasons(Reasons);
        }

        public override string ToString()
        {
            var baseString = base.ToString();
            var valueString = ValueOrDefault.ToLabelValueStringOrEmpty(nameof(Value));
            return $"{baseString}, {valueString}";
        }

        public static implicit operator Result<TValue, TError>(Result result)
        {
            return result.ToResult<TValue, TError>();
        }
    }
}