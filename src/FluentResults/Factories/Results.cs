using System;
using System.Net;

namespace FluentResults
{
    public static class Results
    {
        internal static ResultSettings Settings { get; set; }

        static Results()
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
        
        public static Result Fail(string errorCode, string errorMessage)
        {
            var result = new Result();
            result.WithError(new Error(errorCode, errorMessage));
            return result;
        }

        public static Result Fail(string errorCode, string errorMessage, HttpStatusCode httpStatusCode)
        {
            var result = new Result();
            result.WithError(new Error(errorCode, errorMessage, httpStatusCode));
            return result;
        }
        
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
        
        public static Result<TValue> Fail<TValue>(string errorCode, string errorMessage)
        {
            var result = new Result<TValue>();
            result.WithError(new Error(errorCode, errorMessage));
            return result;
        }
        
        public static Result<TValue> Fail<TValue>(string errorCode, string errorMessage, HttpStatusCode httpStatusCode)
        {
            var result = new Result<TValue>();
            result.WithError(new Error(errorCode, errorMessage, httpStatusCode));
            return result;
        }
        
        public static Result Merge(params ResultBase[] results)
        {
            return ResultHelper.Merge<Result>(results);
        }

        public static Result<TValue> Merge<TValue>(params ResultBase[] results)
        {
            return ResultHelper.Merge<Result<TValue>>(results);
        }
    }

    public static class Results<TResult> where TResult : ResultBase<TResult>, new()
    {
        public static TResult Ok()
        {
            return new TResult();
        }
        
        public static TResult Fail(Error error)
        {
            var result = new TResult();
            result.WithError(error);
            return result;
        }

        public static TResult Fail(string errorMessage)
        {
            var result = new TResult();
            result.WithError(new Error(errorMessage));
            return result;
        }

        public static TResult Merge(params ResultBase[] results)
        {
            return ResultHelper.Merge<TResult>(results);
        }
    }
}