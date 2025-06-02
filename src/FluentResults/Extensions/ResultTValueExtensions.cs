using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace FluentResults.Extensions
{
    /// <summary>
    /// Extensions methods for <see cref="IResult{TValue}"/>
    /// </summary>
    /// <remarks>
    /// Adds methods that are defined in 
    /// <see cref="Result{TValue}"/> so that they are also
    /// available in <see cref="IResult{TValue}"/>.
    /// </remarks>
    public static class ResultTValueExtensions
    {
        /// <summary>
        /// Creates a new <see cref="IResult{TValue}"/> where the value is p<paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="result"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IResult<TValue> WithValue<TValue>(this IResult<TValue> result, TValue value) =>
            CovarianceResultFactory.Create<TValue>(value).WithReasons(result.Reasons);

        /// <summary>
        /// Map all reasons of the result via reasonMapper
        /// </summary>
        /// <remarks>
        /// If TReason is different from IReason the mapper is only to applied to 
        /// reasons of the appropriate type.
        /// </remarks>
        public static IResult<TValue> MapReasons<TValue, TReason>(this IResult<TValue> result, Func<TReason, TReason> reasonMapper)
            where TReason : IReason =>
            // Create new Result object
            CovarianceResultFactory.Create<TValue>(result.ValueOrDefault)
            // Set and map all reasons
            .WithReasons(
                result.Reasons.Select(reason =>
                    reason is TReason tReason
                    ? reasonMapper(tReason)
                    : reason));


        /// <inheritdoc cref="Result{TValue}.MapErrors(Func{IError, IError})"/>
        public static IResult<TValue> MapErrors<TValue>(this IResult<TValue> result, Func<IError, IError> errorMapper) =>
            result.IsSuccess
                ? result
                : result.MapReasons(errorMapper);

        /// <inheritdoc cref="Result{TValue}.MapSuccesses(Func{ISuccess, ISuccess})"/>
        public static IResult<TValue> MapSuccesses<TValue>(this IResult<TValue> result, Func<ISuccess, ISuccess> successMapper) =>
            result.MapReasons(successMapper);

        /// <inheritdoc cref="Result{TValue}.Map{TNewValue}(Func{TValue, TNewValue})"/>
        public static IResult<TNewValue> Map<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, TNewValue> mapLogic)
        {
            if(result.IsSuccess && mapLogic == null)
                throw new ArgumentException("If result is success then valueConverter should not be null");

            return result.WithValue(result.IsFailed ? default : mapLogic(result.Value));
        }

        /// <summary>
        /// Unwraps the value of the result.
        /// </summary>
        /// <remarks>
        /// if <paramref name="result"/> is successful, it returns its value, otherwise
        /// <paramref name="onError"/> is called with the erroneous result.
        /// </remarks>
        /// <typeparam name="TValue">
        /// The underlying type of the result.
        /// </typeparam>
        /// <param name="result">
        /// The result, that is to be unwrapped.
        /// </param>
        /// <param name="onError">
        /// Mapper for the result in case of error. 
        /// (Could also throw an exception instead.)
        /// </param>
        /// <returns>
        /// Return <c>result.Value</c> if result is successful 
        /// and the result of the call to <paramref name="onError"/>
        /// if the result is not successful.
        /// </returns>
        static public TValue Unwrap<TValue>(this IResult<TValue> result, Func<IResult<TValue>, TValue> onError)
        {
            if(result?.IsSuccess is true)
            {
                return result.Value;
            }
            else
            {
                return onError(result);
            }
        }

        #region Variants of Bind(), that return result with value

        /// <inheritdoc cref="Result{TValue}.Bind{TNewValue}(Func{TValue, Result{TNewValue}})"/>
        public static Result<TNewValue> Bind<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, IResult<TNewValue>> bind) =>
            Bind(result, new Func<TValue, ValueTask<IResult<TNewValue>>>(value => new ValueTask<IResult<TNewValue>>(bind(value))))
            .GetAwaiter().GetResult();

        /// <inheritdoc cref="Result{TValue}.Bind{TNewValue}(Func{TValue, Task{Result{TNewValue}}})"/>
        public static Task<Result<TNewValue>> Bind<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, Task<Result<TNewValue>>> bind) =>
            Bind(result, new Func<TValue, Task<IResult<TNewValue>>>(async (value) => await bind(value)));

        /// <inheritdoc cref="Result{TValue}.Bind{TNewValue}(Func{TValue, Task{Result{TNewValue}}})"/>
        public static Task<Result<TNewValue>> Bind<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, Task<IResult<TNewValue>>> bind) =>
            Bind(result, new Func<TValue, ValueTask<IResult<TNewValue>>>(async (value) => await bind(value)))
            .AsTask();
        
        /// <inheritdoc cref="Result{TValue}.Bind{TNewValue}(Func{TValue, ValueTask{Result{TNewValue}}})"/>
        public static ValueTask<Result<TNewValue>> Bind<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, ValueTask<Result<TNewValue>>> bind) =>
            Bind(result, new Func<TValue, ValueTask<IResult<TNewValue>>>(async (value) => await bind(value)));

        /// <inheritdoc cref="Result{TValue}.Bind{TNewValue}(Func{TValue, ValueTask{Result{TNewValue}}})"/>
        public static async ValueTask<Result<TNewValue>> Bind<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, ValueTask<IResult<TNewValue>>> bind)
        {
            var outResult = new Result<TNewValue>();
            outResult.WithReasons(result.Reasons);

            if(result.IsSuccess)
            {
                var converted = await bind(result.Value);
                outResult.WithValue(converted.ValueOrDefault);
                outResult.WithReasons(converted.Reasons);
            }

            return outResult;
        }

        #endregion


        #region Variants of Bind(), that return result without value

        /// <inheritdoc cref="Result{TValue}.Bind(Func{TValue, Result})"/>
        public static Result Bind<TValue>(this IResult<TValue> result, Func<TValue, IResultBase> action) =>
            Bind(result, new Func<TValue, ValueTask<IResultBase>>(value => new ValueTask<IResultBase>(action(value))))
            .GetAwaiter().GetResult();

        /// <inheritdoc cref="Result{TValue}.Bind(Func{TValue, Task{Result}})"/>
        public static Task<Result> Bind<TValue>(this IResult<TValue> result, Func<TValue, Task<Result>> action) =>
            Bind(result, new Func<TValue, Task<IResultBase>>(async (value) => await action(value)));

        /// <inheritdoc cref="Result{TValue}.Bind(Func{TValue, Task{Result}})"/>
        public static Task<Result> Bind<TValue>(this IResult<TValue> result, Func<TValue, Task<IResultBase>> action) =>
            Bind(result, new Func<TValue, ValueTask<IResultBase>>(async (value) => await action(value)))
            .AsTask();

        /// <inheritdoc cref="Result{TValue}.Bind(Func{TValue, ValueTask{Result}})"/>
        public static ValueTask<Result> Bind<TValue>(this IResult<TValue> result, Func<TValue, ValueTask<Result>> action) =>
            Bind(result, new Func<TValue, ValueTask<IResultBase>>(async (value) => await action(value)));

        /// <inheritdoc cref="Result{TValue}.Bind(Func{TValue, ValueTask{Result}})"/>
        public static async ValueTask<Result> Bind<TValue>(this IResult<TValue> result, Func<TValue, ValueTask<IResultBase>> action)
        {
            var outResult = new Result();
            outResult.WithReasons(result.Reasons);

            if(result.IsSuccess)
            {
                var converted = await action(result.Value);
                outResult.WithReasons(converted.Reasons);
            }

            return outResult;
        }

        #endregion
    }
}
