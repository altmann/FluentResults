using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults.Settings;

namespace FluentResults
{
    [Obsolete("Removed in next major version. Use the class Result instead.")]
    public static class Results
    {
        public static void Setup(Action<ResultSettingsBuilder> setupFunc)
        {
            Result.Setup(setupFunc);
        }

        public static Result Ok()
        {
            return new Result();
        }

        public static Result Fail(Error error)
        {
            var result = new Result();
            result.WithError(error);
            return result;
        }

        public static Result Fail(string errorMessage)
        {
            var result = new Result();
            result.WithError(new Error(errorMessage));
            return result;
        }
        
        public static Result<TValue> Ok<TValue>(TValue value)
        {
            var result = new Result<TValue>();
            result.WithValue(value);
            return result;
        }

        public static Result<TValue> Fail<TValue>(Error error)
        {
            var result = new Result<TValue>();
            result.WithError(error);
            return result;
        }

        public static Result<TValue> Fail<TValue>(string errorMessage)
        {
            var result = new Result<TValue>();
            result.WithError(new Error(errorMessage));
            return result;
        }

        public static Result Merge(params ResultBase[] results)
        {
            return ResultHelper.Merge(results);
        }

        public static Result<IEnumerable<TValue>> Merge<TValue>(params Result<TValue>[] results)
        {
            return ResultHelper.MergeWithValue(results);
        }
    }

    public partial class Result
    {
        internal static ResultSettings Settings { get; private set; }

        static Result()
        {
            Settings = new ResultSettingsBuilder().Build();
        }

        /// <summary>
        /// Setup global settings like logging
        /// </summary>
        public static void Setup(Action<ResultSettingsBuilder> setupFunc)
        {
            var settingsBuilder = new ResultSettingsBuilder();
            setupFunc(settingsBuilder);

            Settings = settingsBuilder.Build();
        }

        /// <summary>
        /// Creates a success result
        /// </summary>
        public static Result Ok()
        {
            return new Result();
        }
        
        /// <summary>
        /// Creates a failed result with the given error
        /// </summary>
        public static Result Fail(Error error)
        {
            var result = new Result();
            result.WithError(error);
            return result;
        }

        /// <summary>
        /// Creates a failed result with the given error message. Internally an error object from type `Error` is created. 
        /// </summary>
        public static Result Fail(string errorMessage)
        {
            var result = new Result();
            result.WithError(new Error(errorMessage));
            return result;
        }
        
        /// <summary>
        /// Creates a success result with the given value
        /// </summary>
        public static Result<TValue> Ok<TValue>(TValue value)
        {
            var result = new Result<TValue>();
            result.WithValue(value);
            return result;
        }

        /// <summary>
        /// Creates a failed result with the given error
        /// </summary>
        public static Result<TValue> Fail<TValue>(Error error)
        {
            var result = new Result<TValue>();
            result.WithError(error);
            return result;
        }

        /// <summary>
        /// Creates a failed result with the given error message. Internally an error object from type Error is created. 
        /// </summary>
        public static Result<TValue> Fail<TValue>(string errorMessage)
        {
            var result = new Result<TValue>();
            result.WithError(new Error(errorMessage));
            return result;
        }

        /// <summary>
        /// Merge multiple result objects to one result object together
        /// </summary>
        public static Result Merge(params ResultBase[] results)
        {
            return ResultHelper.Merge(results);
        }

        /// <summary>
        /// Merge multiple result objects to one result object together. Return one result with a list of merged values.
        /// </summary>
        public static Result<IEnumerable<TValue>> Merge<TValue>(params Result<TValue>[] results)
        {
            return ResultHelper.MergeWithValue(results);
        }

        /// <summary>
        /// Create a success/failed result depending on the parameter isSuccess
        /// </summary>
        public static Result OkIf(bool isSuccess, Error error)
        {
            return isSuccess ? Ok() : Fail(error);
        }

        /// <summary>
        /// Create a success/failed result depending on the parameter isSuccess
        /// </summary>
        public static Result OkIf(bool isSuccess, string error)
        {
            return isSuccess ? Ok() : Fail(error);
        }

        /// <summary>
        /// Create a success/failed result depending on the parameter isFailure
        /// </summary>
        public static Result FailIf(bool isFailure, Error error)
        {
            return isFailure ? Fail(error) : Ok();
        }

        /// <summary>
        /// Create a success/failed result depending on the parameter isFailure
        /// </summary>
        public static Result FailIf(bool isFailure, string error)
        {
            return isFailure ? Fail(error) : Ok();
        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static Result Try(Action action, Func<Exception, Error> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                action();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static async Task<Result> Try(Func<Task> action, Func<Exception, Error> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                await action();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static Result<T> Try<T>(Func<T> action, Func<Exception, Error> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return Ok(action());
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static async Task<Result<T>> Try<T>(Func<Task<T>> action, Func<Exception, Error> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return Ok(await action());
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }
    }
}