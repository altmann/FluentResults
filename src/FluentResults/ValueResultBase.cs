using System.Collections.Generic;
using System.Linq;

namespace FluentResults
{
    public abstract class ResultBase
    {
        public bool IsFailed => Reasons.OfType<Error>().Any();
        public bool IsSuccess => !IsFailed;

        public List<Reason> Reasons { get; }
        public List<Error> Errors => Reasons.OfType<Error>().ToList();
        public List<Success> Successes => Reasons.OfType<Success>().ToList();

        protected ResultBase()
        {
            Reasons = new List<Reason>();
        }
    }

    public abstract class ResultBase<TResult> : ResultBase
        where TResult : ResultBase<TResult>
    {
        public TResult WithError(string errorMessage)
        {
            Reasons.Add(new Error(errorMessage));
            return (TResult)this;
        }

        public TResult WithError(Error error)
        {
            Reasons.Add(error);
            return (TResult)this;
        }

        public TResult WithError<TError>()
            where TError : Error, new()
        {
            Reasons.Add(new TError());
            return (TResult) this;
        }

        public TResult WithSuccess(string successMessage)
        {
            Reasons.Add(new Success(successMessage));
            return (TResult)this;
        }

        public TResult WithSuccess(Success success)
        {
            Reasons.Add(success);
            return (TResult)this;
        }

        public TResult WithSuccess<TSuccess>()
            where TSuccess : Success, new()
        {
            Reasons.Add(new TSuccess());
            return (TResult)this;
        }

        public Result<TNewValue> ConvertToResultWithValueType<TNewValue>()
        {
            return ResultHelper.Merge<Result<TNewValue>>(this);
        }

        public TNewResult ConvertToResultOfType<TNewResult>()
            where TNewResult : ResultBase<TNewResult>, new()
        {
            return ResultHelper.Merge<TNewResult>(this);
        }

        public TResult Log()
        {
            return Log(string.Empty);
        }

        public TResult Log(string context)
        {
            var logger = Results.Settings.Logger;

            logger.Log(context, this);

            return (TResult) this;
        }

        public override string ToString()
        {
            return $"Result: IsSuccess: {IsSuccess}, " + ReasonFormat.ReasonsToString(Reasons);
        }
    }
    
    public abstract class ValueResultBase<TResult, TValue> : ResultBase<TResult>
        where TResult : ValueResultBase<TResult, TValue>
    {
        public TValue Value { get; protected set; }

        public TResult WithValue(TValue value)
        {
            Value = value;
            return (TResult)this;
        }

        public Result ConvertTo()
        {
            return ResultHelper.Merge<Result>(this);
        }

        public override string ToString()
        {
            //todo reasons + value
            return "";
        }
    }
}
