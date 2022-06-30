using System;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public partial class Result : ResultBase<Result>
    {
        public Result()
        { }

        /// <summary>
        /// Map all errors of the result via errorMapper
        /// </summary>
        /// <param name="errorMapper"></param>
        /// <returns></returns>
        public Result MapErrors(Func<IError, IError> errorMapper)
        {
            if (IsSuccess)
                return this;
            
            return new Result()
                .WithErrors(Errors.Select(errorMapper))
                .WithSuccesses(Successes);
        }

        /// <summary>
        /// Map all successes of the result via successMapper
        /// </summary>
        /// <param name="successMapper"></param>
        /// <returns></returns>
        public Result MapSuccesses(Func<ISuccess, ISuccess> successMapper)
        {
            return new Result()
                .WithErrors(Errors)
                .WithSuccesses(Successes.Select(successMapper));
        }

        public Result<TNewValue> ToResult<TNewValue>(TNewValue newValue = default)
        {
            return new Result<TNewValue>()
                .WithValue(IsFailed ? default : newValue)
                .WithReasons(Reasons);
        }
    }

    public interface IResult<out TValue> : IResultBase
    {
        /// <summary>
        /// Get the Value. If result is failed then an Exception is thrown because a failed result has no value. Opposite see property ValueOrDefault.
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// Get the Value. If result is failed then a default value is returned. Opposite see property Value.
        /// </summary>
        TValue ValueOrDefault { get; }
    }

    public class Result<TValue> : ResultBase<Result<TValue>>, IResult<TValue>
    {
        public Result()
        { }

        private TValue _value;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public TValue ValueOrDefault => _value;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public TValue Value
        {
            get
            {
                ThrowIfFailed();

                return _value;
            }
            private set
            {
                ThrowIfFailed();

                _value = value;
            }
        }

        /// <summary>
        /// Set value
        /// </summary>
        public Result<TValue> WithValue(TValue value)
        {
            Value = value;
            return this;
        }

        /// <summary>
        /// Map all errors of the result via errorMapper
        /// </summary>
        /// <param name="errorMapper"></param>
        /// <returns></returns>
        public Result<TValue> MapErrors(Func<IError, IError> errorMapper)
        {
            if (IsSuccess)
                return this;

            return new Result<TValue>()
                .WithErrors(Errors.Select(errorMapper))
                .WithSuccesses(Successes);
        }

        /// <summary>
        /// Map all successes of the result via successMapper
        /// </summary>
        /// <param name="successMapper"></param>
        /// <returns></returns>
        public Result<TValue> MapSuccesses(Func<ISuccess, ISuccess> successMapper)
        {
            return new Result<TValue>()
                .WithValue(ValueOrDefault)
                .WithErrors(Errors)
                .WithSuccesses(Successes.Select(successMapper));
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
        public Result<TNewValue> ToResult<TNewValue>(Func<TValue, TNewValue> valueConverter = null)
        {
            if(IsSuccess && valueConverter == null)
                throw new ArgumentException("If result is success then valueConverter should not be null");

            return new Result<TNewValue>()
                .WithValue(IsFailed ? default : valueConverter(Value))
                .WithReasons(Reasons);
        }

        public override string ToString()
        {
            var baseString = base.ToString();
            var valueString = ValueOrDefault.ToLabelValueStringOrEmpty(nameof(Value));
            return $"{baseString}, {valueString}";
        }

        public static implicit operator Result<TValue>(Result result)
        {
            return result.ToResult<TValue>();
        }

        public static implicit operator Result<TValue>(TValue value)
        {
            return Result.Ok(value);
        }

        private void ThrowIfFailed()
        {
            if (IsFailed)
                throw new InvalidOperationException($"Result is in status failed. Value is not set. Having: {ReasonFormat.ErrorReasonsToString(Errors)}");
        }
    }
}