using System;
using System.Linq;
using System.Threading.Tasks;

namespace FluentResults.Extensions
{
    /// <summary>
    /// Extension methods for Result
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Map all errors of the result via errorMapper
        /// </summary>
        /// <param name="resultTask">The current result</param>
        /// <param name="errorMapper">Function to transform the errors</param>
        public static async Task<Result> MapErrors(this Task<Result> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.MapErrors(errorMapper);
        }

        /// <summary>
        /// Map all errors of the result via errorMapper
        /// </summary>
        /// <param name="resultTask">The current result</param>
        /// <param name="errorMapper">Function to transform the errors</param>
        public static async ValueTask<Result> MapErrors(this ValueTask<Result> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.MapErrors(errorMapper);
        }

        /// <summary>
        /// Map all errors of the result via errorMapper
        /// </summary>
        /// <param name="resultTask">The current result</param>
        /// <param name="errorMapper">Function to transform the errors</param>
        public static async Task<Result<T>> MapErrors<T>(this Task<Result<T>> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.MapErrors(errorMapper);
        }

        /// <summary>
        /// Map all errors of the result via errorMapper
        /// </summary>
        /// <param name="resultTask">The current result</param>
        /// <param name="errorMapper">Function to transform the errors</param>
        public static async ValueTask<Result<T>> MapErrors<T>(this ValueTask<Result<T>> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.MapErrors(errorMapper);
        }

        /// <summary>
        /// Map all successes of the result via successMapper
        /// </summary>
        /// <param name="resultTask">The current result</param>
        /// <param name="errorMapper">Function to transform the successes</param>
        public static async Task<Result> MapSuccesses(this Task<Result> resultTask, Func<ISuccess, ISuccess> errorMapper)
        {
            var result = await resultTask;
            return result.MapSuccesses(errorMapper);
        }

        /// <summary>
        /// Map all successes of the result via successMapper
        /// </summary>
        /// <param name="resultTask">The current result</param>
        /// <param name="errorMapper">Function to transform the successes</param>
        public static async ValueTask<Result> MapSuccesses(this ValueTask<Result> resultTask, Func<ISuccess, ISuccess> errorMapper)
        {
            var result = await resultTask;
            return result.MapSuccesses(errorMapper);
        }

        /// <summary>
        /// Map all successes of the result via successMapper
        /// <param name="resultTask">The current result</param>
        /// <param name="errorMapper">Function to transform the successes</param>
        /// </summary>
        public static async Task<Result<T>> MapSuccesses<T>(this Task<Result<T>> resultTask, Func<ISuccess, ISuccess> errorMapper)
        {
            var result = await resultTask;
            return result.MapSuccesses(errorMapper);
        }

        /// <summary>
        /// Map all successes of the result via successMapper
        /// </summary>
        /// <param name="resultTask">The current result</param>
        /// <param name="errorMapper">Function to transform the successes</param>
        public static async ValueTask<Result<T>> MapSuccesses<T>(this ValueTask<Result<T>> resultTask, Func<ISuccess, ISuccess> errorMapper)
        {
            var result = await resultTask;
            return result.MapSuccesses(errorMapper);
        }

        /// <summary>
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">The current result</param>
        /// <param name="bind">Transformation that may fail.</param>
        public static async Task<Result<TNew>> Bind<TOld, TNew>(this Task<Result<TOld>> resultTask, Func<TOld, Task<Result<TNew>>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">The current result</param>
        /// <param name="bind">Transformation that may fail.</param>
        public static async ValueTask<Result<TNew>> Bind<TOld, TNew>(this ValueTask<Result<TOld>> resultTask, Func<TOld, ValueTask<Result<TNew>>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">The current result</param>
        /// <param name="bind">Transformation that may fail.</param>
        public static async Task<Result<TNew>> Bind<TOld, TNew>(this Task<Result<TOld>> resultTask, Func<TOld, Result<TNew>> bind)
        {
            var result = await resultTask;
            return result.Bind(bind);
        }
        /// <summary>
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">The current result</param>
        /// <param name="bind">Transformation that may fail.</param>

        public static async ValueTask<Result<TNew>> Bind<TOld, TNew>(this ValueTask<Result<TOld>> resultTask, Func<TOld, Result<TNew>> bind)
        {
            var result = await resultTask;
            return result.Bind(bind);
        }

        /// <summary>
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">The current result</param>
        /// <param name="bind">Transformation that may fail.</param>
        public static async Task<Result> Bind<TOld>(this Task<Result<TOld>> resultTask, Func<TOld, Task<Result>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">The current result</param>
        /// <param name="bind">Transformation that may fail.</param>
        public static async Task<Result> Bind<TOld>(this Task<Result<TOld>> resultTask, Func<TOld, Result> bind)
        {
            var result = await resultTask;
            return result.Bind(bind);
        }

        /// <summary>
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">The current result</param>
        /// <param name="bind">Transformation that may fail.</param>
        public static async ValueTask<Result> Bind<TOld>(this ValueTask<Result<TOld>> resultTask, Func<TOld, Result> bind)
        {
            var result = await resultTask;
            return result.Bind(bind);
        }

        /// <summary>
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">The current result</param>
        /// <param name="bind">Transformation that may fail.</param>
        public static async ValueTask<Result> Bind<TOld>(this ValueTask<Result<TOld>> resultTask, Func<TOld, ValueTask<Result>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">The current result</param>
        /// <param name="bind">Transformation that may fail.</param>
        public static async Task<Result<TNew>> Bind<TNew>(this Task<Result> resultTask, Func<Task<Result<TNew>>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">The current result</param>
        /// <param name="bind">Transformation that may fail.</param>
        public static async ValueTask<Result<TNew>> Bind<TNew>(this ValueTask<Result> resultTask, Func<ValueTask<Result<TNew>>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">The current result</param>
        /// <param name="bind">Transformation that may fail.</param>
        public static async Task<Result> Bind(this Task<Result> resultTask, Func<Task<Result>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">The current result</param>
        /// <param name="bind">Transformation that may fail.</param>
        public static async ValueTask<Result> Bind(this ValueTask<Result> resultTask, Func<ValueTask<Result>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
        /// </summary>
        public static async Task<Result<TNewValue>> Map<TOldValue, TNewValue>(this Task<Result<TOldValue>> resultTask, Func<TOldValue, TNewValue> valueConverter)
        {
            var result = await resultTask;
            return result.Map(valueConverter);
        }

        /// <summary>
        /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
        /// </summary>
        public static async Task<Result<TNewValue>> Map<TOldValue, TNewValue>(this ValueTask<Result<TOldValue>> resultTask, Func<TOldValue, TNewValue> valueConverter)
        {
            var result = await resultTask;
            return result.Map(valueConverter);
        }

        /// <summary>
        /// Convert result without value to a result containing a value
        /// </summary>
        /// <typeparam name="TValue">Type of the value</typeparam>
        /// <param name="resultTask">The current result</param>
        /// <param name="value">Value to add to the new result</param>
        public static async Task<Result<TValue>> ToResult<TValue>(this Task<Result> resultTask, TValue value)
        {
            var result = await resultTask;
            return result.ToResult(value);
        }

        /// <summary>
        /// Convert result without value to a result containing a value
        /// </summary>
        /// <typeparam name="TValue">Type of the value</typeparam>
        /// <param name="resultTask">The current result</param>
        /// <param name="value">Value to add to the new result</param>
        public static async Task<Result<TValue>> ToResult<TValue>(this ValueTask<Result> resultTask, TValue value)
        {
            var result = await resultTask;
            return result.ToResult(value);
        }

        #region Interfaces

        public static async Task<IResultBase> MapErrors(this Task<IResultBase> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.MapErrors(errorMapper);
        }

        public static async ValueTask<IResultBase> MapErrors(this ValueTask<IResultBase> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.MapErrors(errorMapper);
        }

        public static async Task<IResult<T>> MapErrors<T>(this Task<IResult<T>> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.MapErrors(errorMapper);
        }

        public static async ValueTask<IResult<T>> MapErrors<T>(this ValueTask<IResult<T>> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.MapErrors(errorMapper);
        }

        public static async Task<IResultBase> MapSuccesses(this Task<IResultBase> resultTask, Func<ISuccess, ISuccess> errorMapper)
        {
            var result = await resultTask;
            return result.MapSuccesses(errorMapper);
        }

        public static async ValueTask<IResultBase> MapSuccesses(this ValueTask<IResultBase> resultTask, Func<ISuccess, ISuccess> errorMapper)
        {
            var result = await resultTask;
            return result.MapSuccesses(errorMapper);
        }

        public static async Task<IResult<T>> MapSuccesses<T>(this Task<IResult<T>> resultTask, Func<ISuccess, ISuccess> errorMapper)
        {
            var result = await resultTask;
            return result.MapSuccesses(errorMapper);
        }

        public static async ValueTask<IResult<T>> MapSuccesses<T>(this ValueTask<IResult<T>> resultTask, Func<ISuccess, ISuccess> errorMapper)
        {
            var result = await resultTask;
            return result.MapSuccesses(errorMapper);
        }

        public static async Task<IResult<TNew>> Bind<TOld, TNew>(this Task<IResult<TOld>> resultTask, Func<TOld, Task<IResult<TNew>>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        public static async ValueTask<IResult<TNew>> Bind<TOld, TNew>(this ValueTask<IResult<TOld>> resultTask, Func<TOld, ValueTask<IResult<TNew>>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        public static async Task<IResult<TNew>> Bind<TOld, TNew>(this Task<IResult<TOld>> resultTask, Func<TOld, IResult<TNew>> bind)
        {
            var result = await resultTask;
            return result.Bind(bind);
        }

        public static async ValueTask<IResult<TNew>> Bind<TOld, TNew>(this ValueTask<IResult<TOld>> resultTask, Func<TOld, IResult<TNew>> bind)
        {
            var result = await resultTask;
            return result.Bind(bind);
        }

        public static async Task<IResultBase> Bind<TOld>(this Task<IResult<TOld>> resultTask, Func<TOld, Task<IResultBase>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        public static async Task<IResultBase> Bind<TOld>(this Task<IResult<TOld>> resultTask, Func<TOld, IResultBase> bind)
        {
            var result = await resultTask;
            return result.Bind(bind);
        }

        public static async ValueTask<IResultBase> Bind<TOld>(this ValueTask<IResult<TOld>> resultTask, Func<TOld, IResultBase> bind)
        {
            var result = await resultTask;
            return result.Bind(bind);
        }

        public static async ValueTask<IResultBase> Bind<TOld>(this ValueTask<IResult<TOld>> resultTask, Func<TOld, ValueTask<IResultBase>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        public static async Task<IResult<TNew>> Bind<TNew>(this Task<IResultBase> resultTask, Func<Task<IResult<TNew>>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        public static async ValueTask<IResult<TNew>> Bind<TNew>(this ValueTask<IResultBase> resultTask, Func<ValueTask<IResult<TNew>>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        public static async Task<IResultBase> Bind(this Task<IResultBase> resultTask, Func<Task<IResultBase>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        public static async ValueTask<IResultBase> Bind(this ValueTask<IResultBase> resultTask, Func<ValueTask<IResultBase>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        public static async Task<IResult<TNewValue>> Map<TOldValue, TNewValue>(this Task<IResult<TOldValue>> resultTask, Func<TOldValue, TNewValue> valueConverter)
        {
            var result = await resultTask;
            return result.Map(valueConverter);
        }

        public static async Task<IResult<TNewValue>> Map<TOldValue, TNewValue>(this ValueTask<IResult<TOldValue>> resultTask, Func<TOldValue, TNewValue> valueConverter)
        {
            var result = await resultTask;
            return result.Map(valueConverter);
        }

        public static async Task<IResult<TValue>> ToResult<TValue>(this Task<IResultBase> resultTask, TValue value)
        {
            var result = await resultTask;
            return result.ToResult(value);
        }

        public static async Task<IResult<TValue>> ToResult<TValue>(this ValueTask<IResultBase> resultTask, TValue value)
        {
            var result = await resultTask;
            return result.ToResult(value);
        }

        #endregion


        #region Erweiterung: Member von Result für Interface IResultBase

        /// <inheritdoc cref="Result.MapErrors(Func{IError, IError})"/>
        public static IResultBase MapErrors(this IResultBase result, Func<IError, IError> errorMapper) =>
            result.IsSuccess 
                ? result
                : new Result()
                    .WithErrors(result.Errors.Select(errorMapper))
                    .WithSuccesses(result.Successes);

        /// <inheritdoc cref="Result.MapSuccesses(Func{ISuccess, ISuccess})"/>
        public static IResultBase MapSuccesses(this IResultBase result, Func<ISuccess, ISuccess> successMapper) =>
            new Result()
                .WithErrors(result.Errors)
                .WithSuccesses(result.Successes.Select(successMapper));

        /// <inheritdoc cref="Result.ToResult{TNewValue}(TNewValue)"/>
        public static IResult<TNewValue> ToResult<TNewValue>(this IResultBase result, TNewValue newValue = default) =>
            new Result<TNewValue>()
                .WithValue(result.IsFailed ? default : newValue)
                .WithReasons(result.Reasons);

        /// <inheritdoc cref="Result.Bind{TNewValue}(Func{Result{TNewValue}})"/>
        public static IResult<TNewValue> Bind<TNewValue>(this IResultBase result, Func<IResult<TNewValue>> bind)
        {
            var out_result = new Result<TNewValue>();
            out_result.WithReasons(result.Reasons);

            if(result.IsSuccess)
            {
                var converted = bind();
                out_result.WithValue(converted.ValueOrDefault);
                out_result.WithReasons(converted.Reasons);
            }

            return out_result;
        }

        /// <inheritdoc cref="Result.Bind{TNewValue}(Func{Task{Result{TNewValue}}})"/>
        public static async Task<IResult<TNewValue>> Bind<TNewValue>(this IResultBase result, Func<Task<IResult<TNewValue>>> bind)
        {
            var out_result = new Result<TNewValue>();
            out_result.WithReasons(result.Reasons);

            if(result.IsSuccess)
            {
                var converted = await bind();
                out_result.WithValue(converted.ValueOrDefault);
                out_result.WithReasons(converted.Reasons);
            }

            return out_result;
        }

        /// <inheritdoc cref="Result.Bind{TNewValue}(Func{ValueTask{Result{TNewValue}}})" />
        public static async ValueTask<IResult<TNewValue>> Bind<TNewValue>(this IResultBase result, Func<ValueTask<IResult<TNewValue>>> bind)
        {
            var out_result = new Result<TNewValue>();
            out_result.WithReasons(result.Reasons);

            if(result.IsSuccess)
            {
                var converted = await bind();
                out_result.WithValue(converted.ValueOrDefault);
                out_result.WithReasons(converted.Reasons);
            }

            return out_result;
        }

        /// <inheritdoc cref="Result.Bind(Func{Result})"/>
        public static IResultBase Bind(this IResultBase result, Func<IResultBase> action)
        {
            var out_result = new Result();
            out_result.WithReasons(result.Reasons);

            if(result.IsSuccess)
            {
                var converted = action();
                out_result.WithReasons(converted.Reasons);
            }

            return result;
        }

        /// <inheritdoc cref="Result.Bind(Func{Task{Result}})"/>
        public static async Task<IResultBase> Bind(this IResultBase result, Func<Task<IResultBase>> action)
        {
            var out_result = new Result();
            out_result.WithReasons(result.Reasons);

            if(result.IsSuccess)
            {
                var converted = await action();
                out_result.WithReasons(converted.Reasons);
            }

            return out_result;
        }

        /// <inheritdoc cref="Result.Bind(Func{ValueTask{Result}})"/>
        public static async ValueTask<IResultBase> Bind(this IResultBase result, Func<ValueTask<IResultBase>> action)
        {
            var out_result = new Result();
            out_result.WithReasons(result.Reasons);

            if(result.IsSuccess)
            {
                var converted = await action();
                out_result.WithReasons(converted.Reasons);
            }

            return out_result;
        }

        #endregion


        #region Erweiterung: Member von Result für Interface IResultBase

        /// <inheritdoc cref="Result{TValue}.WithValue(TValue)"/>
        public static IResult<TValue> WithValue<TValue>(this IResult<TValue> result, TValue value) =>
            // ℹ️ You would assume immutability on results,
            //     therefor we first create a copy!
            new Result<TValue>()
                .WithReasons(result.Reasons)
                .WithValue(value);


        /// <inheritdoc cref="Result{TValue}.MapErrors(Func{IError, IError})"/>
        public static IResult<TValue> MapErrors<TValue>(this IResult<TValue> result, Func<IError, IError> errorMapper) =>
            result.IsSuccess
                ? result
                : new Result<TValue>()
                    .WithErrors(result.Errors.Select(errorMapper))
                    .WithSuccesses(result.Successes);

        /// <inheritdoc cref="Result{TValue}.MapSuccesses(Func{ISuccess, ISuccess})"/>
        public static IResult<TValue> MapSuccesses<TValue>(this IResult<TValue> result, Func<ISuccess, ISuccess> successMapper) =>
            new Result<TValue>()
                .WithValue(result.ValueOrDefault)
                .WithErrors(result.Errors)
                .WithSuccesses(result.Successes.Select(successMapper));

        /// <inheritdoc cref="Result{TValue}.ToResult()"/>
        public static IResultBase ToResult<TValue>(this IResult<TValue> result) =>
            new Result()
                .WithReasons(result.Reasons);

        /// <inheritdoc cref="Result{TValue}.ToResult{TNewValue}(Func{TValue, TNewValue})"/>
        public static Result<TNewValue> ToResult<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, TNewValue> valueConverter = null) =>
            Map(result, valueConverter);

        /// <inheritdoc cref="Result{TValue}.Map{TNewValue}(Func{TValue, TNewValue})"/>
        public static Result<TNewValue> Map<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, TNewValue> mapLogic)
        {
            if(result.IsSuccess && mapLogic == null)
                throw new ArgumentException("If result is success then valueConverter should not be null");

            return new Result<TNewValue>()
                   .WithValue(result.IsFailed ? default : mapLogic(result.Value))
                   .WithReasons(result.Reasons);
        }

        /// <inheritdoc cref="Result{TValue}.Bind{TNewValue}(Func{TValue, Result{TNewValue}})"/>
        public static Result<TNewValue> Bind<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, IResult<TNewValue>> bind)
        {
            var out_result = new Result<TNewValue>();
            out_result.WithReasons(result.Reasons);

            if(result.IsSuccess)
            {
                var converted = bind(result.Value);
                out_result.WithValue(converted.ValueOrDefault);
                out_result.WithReasons(converted.Reasons);
            }

            return out_result;
        }

        /// <inheritdoc cref="Result{TValue}.Bind{TNewValue}(Func{TValue, Task{Result{TNewValue}}})"/>
        public static async Task<Result<TNewValue>> Bind<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, Task<IResult<TNewValue>>> bind)
        {
            var out_result = new Result<TNewValue>();
            out_result.WithReasons(result.Reasons);

            if(result.IsSuccess)
            {
                var converted = await bind(result.Value);
                out_result.WithValue(converted.ValueOrDefault);
                out_result.WithReasons(converted.Reasons);
            }

            return out_result;
        }

        /// <inheritdoc cref="Result{TValue}.Bind{TNewValue}(Func{TValue, ValueTask{Result{TNewValue}}})"/>
        public static async ValueTask<Result<TNewValue>> Bind<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, ValueTask<IResult<TNewValue>>> bind)
        {
            var out_result = new Result<TNewValue>();
            out_result.WithReasons(result.Reasons);

            if(result.IsSuccess)
            {
                var converted = await bind(result.Value);
                out_result.WithValue(converted.ValueOrDefault);
                out_result.WithReasons(converted.Reasons);
            }

            return out_result;
        }

        /// <inheritdoc cref="Result{TValue}.Bind(Func{TValue, Result})"/>
        public static IResultBase Bind<TValue>(this IResult<TValue> result, Func<TValue, IResultBase> action)
        {
            var out_result = new Result();
            out_result.WithReasons(result.Reasons);

            if(result.IsSuccess)
            {
                var converted = action(result.Value);
                out_result.WithReasons(converted.Reasons);
            }

            return out_result;
        }

        /// <inheritdoc cref="Result{TValue}.Bind(Func{TValue, Task{Result}})"/>
        public static async Task<IResultBase> Bind<TValue>(this IResult<TValue> result, Func<TValue, Task<IResultBase>> action)
        {
            var out_result = new Result();
            out_result.WithReasons(result.Reasons);

            if(result.IsSuccess)
            {
                var converted = await action(result.Value);
                out_result.WithReasons(converted.Reasons);
            }

            return out_result;
        }

        /// <inheritdoc cref="Result{TValue}.Bind(Func{TValue, ValueTask{Result}})"/>
        public static async ValueTask<IResultBase> Bind<TValue>(this IResult<TValue> result, Func<TValue, ValueTask<IResultBase>> action)
        {
            var out_result = new Result();
            out_result.WithReasons(result.Reasons);

            if(result.IsSuccess)
            {
                var converted = await action(result.Value);
                out_result.WithReasons(converted.Reasons);
            }

            return out_result;
        }

        #endregion
    }
}
