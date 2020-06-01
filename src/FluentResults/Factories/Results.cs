using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
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

        [Obsolete("Removed in next major version. Use Results.Ok<TValue>(TValue value) instead.")]
        public static Result<TValue> Ok<TValue>()
        {
            return new Result<TValue>();
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

        public static void Setup(Action<ResultSettingsBuilder> setupFunc)
        {
            var settingsBuilder = new ResultSettingsBuilder();
            setupFunc(settingsBuilder);

            Settings = settingsBuilder.Build();
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

        [Obsolete("Removed in next major version. Use Results.Ok<TValue>(TValue value) instead.")]
        public static Result<TValue> Ok<TValue>()
        {
            return new Result<TValue>();
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
}