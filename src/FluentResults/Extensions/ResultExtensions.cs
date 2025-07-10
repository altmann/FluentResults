﻿using System;
using System.Linq;
using System.Threading.Tasks;

namespace FluentResults.Extensions
{
    /// <summary>
    /// Extension methods for Result
    /// </summary>
    public static class ResultExtensions
    {
        #region Extensions for Task<Result>, ValueTask<Result>, Task<Result<T>>, and ValueTask<Result<T>>

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
        #endregion

        #region Extensions for Task<IResultBase>, ValueTask<IResultBase>, Task<IResult<T>>, and ValueTask<IResult<T>>

        /// <inheritdoc cref="MapErrors(Task{Result}, Func{IError, IError})"/>
        public static async Task<IResultBase> MapErrors(this Task<IResultBase> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.MapErrors(errorMapper);
        }
        /// <inheritdoc cref="MapErrors(ValueTask{Result}, Func{IError, IError})"/>
        public static async ValueTask<IResultBase> MapErrors(this ValueTask<IResultBase> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.MapErrors(errorMapper);
        }

        /// <inheritdoc cref="MapErrors{T}(Task{Result{T}}, Func{IError, IError})"/>
        public static async Task<IResult<T>> MapErrors<T>(this Task<IResult<T>> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.IsSuccess ? result :
                new Result<T>()
                    .WithErrors(result.Errors.Select(errorMapper))
                    .WithSuccesses(result.Successes);
        }

        /// <inheritdoc cref="MapErrors{T}(ValueTask{Result{T}}, Func{IError, IError})"/>
        public static async ValueTask<IResult<T>> MapErrors<T>(this ValueTask<IResult<T>> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.IsSuccess ? result : result.MapReasons(errorMapper);
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

        #endregion

        #region Convert (Value)Task of IResultBase into Task of Result

        public static async Task<Result> ToResult(this Task<IResultBase> resultTask)
        {
            var result = await resultTask;
            return new Result()
                .WithReasons(result.Reasons);
        }

        public static async ValueTask<Result> ToResult(this ValueTask<IResultBase> resultTask)
        {
            var result = await resultTask;
            return new Result()
                .WithReasons(result.Reasons);
        }

        public static async Task<Result<TValue>> ToResult<TValue>(this Task<IResultBase> resultTask, TValue value)
        {
            var result = await resultTask;
            return new Result<TValue>()
                .WithValue(value)
                .WithReasons(result.Reasons);
        }

        public static async ValueTask<Result<TValue>> ToResult<TValue>(this ValueTask<IResultBase> resultTask, TValue value)
        {
            var result = await resultTask;
            return new Result<TValue>()
                .WithValue(value)
                .WithReasons(result.Reasons);
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
          
        /// <summary>
        /// Create a success/failed result depending on the parameter isFailure
        /// </summary>
        /// <param name="source">The previous result</param>
        /// <param name="isFailure">The condition to check if the result should fail</param>
        /// <param name="error">The error message</param>
        /// <returns>The previous result if it is already failed or a new result</returns>
        public static Result OrFailIf(this Result source, bool isFailure, string error)
        {
            if (source.IsFailed)
            {
                return source;
            }
            
            return isFailure ? Result.Fail(error) : source;
        }
          
        /// <summary>
        /// Create a success/failed result depending on the parameter isFailure
        /// </summary>
        /// <param name="source">The previous result</param>
        /// <param name="isFailure">The condition to check if the result should fail</param>
        /// <param name="error">The error message</param>
        /// <returns>The previous result if it is already failed or a new result</returns>
        public static IResult OrFailIf(this IResult source, bool isFailure, string error)
        {
            if (source.IsFailed)
            {
                return source;
            }
            
            return isFailure ? Result.Fail(error) : source;
        }
    }
}
