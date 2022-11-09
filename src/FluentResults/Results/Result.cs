using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        /// <summary>
        /// Convert result to result with value that may fail.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="bind">Transformation that may fail.</param>
        public Result<TNewValue> Bind<TNewValue>(Func<Result<TNewValue>> bind)
        {
            var result = new Result<TNewValue>();
            result.WithReasons(Reasons);
            
            if (IsSuccess)
            {
                var converted = bind();
                result.WithValue(converted.ValueOrDefault);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }
        
        /// <summary>
        /// Convert result to result with value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="bind">Transformation that may fail.</param>
        public async Task<Result<TNewValue>> Bind<TNewValue>(Func<Task<Result<TNewValue>>> bind)
        {
            var result = new Result<TNewValue>();
            result.WithReasons(Reasons);
            
            if (IsSuccess)
            {
                var converted = await bind();
                result.WithValue(converted.ValueOrDefault);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }
        
        /// <summary>
        /// Convert result to result with value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="bind">Transformation that may fail.</param>
        public async ValueTask<Result<TNewValue>> Bind<TNewValue>(Func<ValueTask<Result<TNewValue>>> bind)
        {
            var result = new Result<TNewValue>();
            result.WithReasons(Reasons);
            
            if (IsSuccess)
            {
                var converted = await bind();
                result.WithValue(converted.ValueOrDefault);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }
        
        /// <summary>
        /// Execute an action which returns a <see cref="Result"/>.
        /// </summary>
        /// <example>
        /// <code>
        ///  var done = result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="action">Action that may fail.</param>
        public Result Bind(Func<Result> action)
        {
            var result = new Result();
            result.WithReasons(Reasons);
            
            if (IsSuccess)
            {
                var converted = action();
                result.WithReasons(converted.Reasons);
            }

            return result;
        }
        
        /// <summary>
        /// Execute an action which returns a <see cref="Result"/> asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var done = result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="action">Action that may fail.</param>
        public async Task<Result> Bind(Func<Task<Result>> action)
        {
            var result = new Result();
            result.WithReasons(Reasons);
            
            if (IsSuccess)
            {
                var converted = await action();
                result.WithReasons(converted.Reasons);
            }

            return result;
        }
        
        /// <summary>
        /// Execute an action which returns a <see cref="Result"/> asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var done = result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="action">Action that may fail.</param>
        public async ValueTask<Result> Bind(Func<ValueTask<Result>> action)
        {
            var result = new Result();
            result.WithReasons(Reasons);
            
            if (IsSuccess)
            {
                var converted = await action();
                result.WithReasons(converted.Reasons);
            }

            return result;
        }
        
        public static implicit operator Result(Error error)
        {
            return Fail(error);
        }

        public static implicit operator Result(List<Error> errors)
        {
            return Fail(errors);
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
            return Map(valueConverter);
        }

        /// <summary>
        /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
        /// </summary>
        public Result<TNewValue> Map<TNewValue>(Func<TValue, TNewValue> mapLogic)
        {
            if (IsSuccess && mapLogic == null)
                throw new ArgumentException("If result is success then valueConverter should not be null");

            return new Result<TNewValue>()
                   .WithValue(IsFailed ? default : mapLogic(Value))
                   .WithReasons(Reasons);
        }

        /// <summary>
        /// Convert result with value to result with another value that may fail.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = result
        ///     .Bind(GetWhichMayFail)
        ///     .Bind(ProcessWhichMayFail)
        ///     .Bind(FormattingWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="bind">Transformation that may fail.</param>
        public Result<TNewValue> Bind<TNewValue>(Func<TValue, Result<TNewValue>> bind)
        {
            var result = new Result<TNewValue>();
            result.WithReasons(Reasons);
            
            if (IsSuccess)
            {
                var converted = bind(Value);
                result.WithValue(converted.ValueOrDefault);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }
        
        /// <summary>
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="bind">Transformation that may fail.</param>
        public async Task<Result<TNewValue>> Bind<TNewValue>(Func<TValue, Task<Result<TNewValue>>> bind)
        {
            var result = new Result<TNewValue>();
            result.WithReasons(Reasons);
            
            if (IsSuccess)
            {
                var converted = await bind(Value);
                result.WithValue(converted.ValueOrDefault);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }
        
        /// <summary>
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="bind">Transformation that may fail.</param>
        public async ValueTask<Result<TNewValue>> Bind<TNewValue>(Func<TValue, ValueTask<Result<TNewValue>>> bind)
        {
            var result = new Result<TNewValue>();
            result.WithReasons(Reasons);
            
            if (IsSuccess)
            {
                var converted = await bind(Value);
                result.WithValue(converted.ValueOrDefault);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }

        /// <summary>
        /// Execute an action which returns a <see cref="Result"/>.
        /// </summary>
        /// <example>
        /// <code>
        ///  var done = result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="action">Action that may fail.</param>
        public Result Bind(Func<TValue, Result> action)
        {
            var result = new Result();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = action(Value);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }
        
        /// <summary>
        /// Execute an action which returns a <see cref="Result"/> asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var done = await result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="action">Action that may fail.</param>
        public async Task<Result> Bind(Func<TValue, Task<Result>> action)
        {
            var result = new Result();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = await action(Value);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }
        
        /// <summary>
        /// Execute an action which returns a <see cref="Result"/> asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var done = await result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="action">Action that may fail.</param>
        public async ValueTask<Result> Bind(Func<TValue, ValueTask<Result>> action)
        {
            var result = new Result();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = await action(Value);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }

        public override string ToString()
        {
            var baseString = base.ToString();
            var valueString = ValueOrDefault.ToLabelValueStringOrEmpty(nameof(Value));
            return $"{baseString}, {valueString}";
        }

        public static implicit operator Result<TValue>(Result result)
        {
            return result.ToResult<TValue>(default);
        }

        public static implicit operator Result<TValue>(TValue value)
        {
            if (value is Result<TValue> r)
                return r;

            return Result.Ok(value);
        }
        
        public static implicit operator Result<TValue>(Error error)
        {
            return Result.Fail(error);
        }

        public static implicit operator Result<TValue>(List<Error> errors)
        {
            return Result.Fail(errors);
        }

        /// <summary>
        /// Deconstruct Result
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="isFailed"></param>
        /// <param name="value"></param>
        public void Deconstruct(out bool isSuccess, out bool isFailed, out TValue value)
        {
            isSuccess = IsSuccess;
            isFailed = IsFailed;
            value = IsSuccess ? Value : default;
        }

        /// <summary>
        /// Deconstruct Result
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="isFailed"></param>
        /// <param name="value"></param>
        /// <param name="errors"></param>
        public void Deconstruct(out bool isSuccess, out bool isFailed, out TValue value, out List<IError> errors)
        {
            isSuccess = IsSuccess;
            isFailed = IsFailed;
            value = IsSuccess ? Value : default;
            errors = IsFailed ? Errors : default;
        }

        private void ThrowIfFailed()
        {
            if (IsFailed)
                throw new InvalidOperationException($"Result is in status failed. Value is not set. Having: {ReasonFormat.ErrorReasonsToString(Errors)}");
        }
    }
}