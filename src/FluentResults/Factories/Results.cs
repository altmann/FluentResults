using System;

namespace FluentResults
{
    public interface ILogger
    {
        void Log(string context, ResultBase result);
    }

    public class DefaultLogger : ILogger
    {
        public void Log(string context, ResultBase result)
        {
            
        }
    }

    public class ResultSettings
    {
        public ILogger Logger { get; set; }
    }

    public class ResultSettingsBuilder
    {
        public ILogger Logger { get; set; }

        public ResultSettingsBuilder()
        {
            // set defaults
            Logger = new DefaultLogger();
        }

        public ResultSettings Build()
        {
            return new ResultSettings
            {
                Logger = Logger
            };
        }
    }

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

    internal static class ResultHelper
    {
        public static TResult Merge<TResult>(params ResultBase[] results)
            where TResult : ResultBase<TResult>, new()
        {
            var finalResult = new TResult();

            foreach (var result in results)
            {
                foreach (var reason in result.Reasons)
                {
                    if (reason.GetType() == typeof(Error))
                    {
                        finalResult.WithError(reason as Error);
                    }
                    else if (reason.GetType() == typeof(Success))
                    {
                        finalResult.WithSuccess(reason as Success);
                    }
                }
            }

            return finalResult;
        }
    }
}