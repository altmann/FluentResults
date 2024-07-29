using System;
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
    }
}
