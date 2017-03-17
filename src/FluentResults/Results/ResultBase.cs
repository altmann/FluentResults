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
            return WithError(new Error(errorMessage));
        }

        internal TResult WithReason(Reason reason)
        {
            Reasons.Add(reason);
            return (TResult)this;
        }

        public TResult WithError(Error error)
        {
            return WithReason(error);
        }

        public TResult WithError<TError>()
            where TError : Error, new()
        {
            return WithError(new TError());
        }

        public TResult WithSuccess(string successMessage)
        {
            return WithSuccess(new Success(successMessage));
        }

        public TResult WithSuccess(Success success)
        {
            return WithReason(success);
        }

        public TResult WithSuccess<TSuccess>()
            where TSuccess : Success, new()
        {
            return WithSuccess(new TSuccess());
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

            return (TResult)this;
        }

        public override string ToString()
        {
            return $"Result: IsSuccess='{IsSuccess}', Reasons='{ReasonFormat.ReasonsToString(Reasons)}'";
        }
    }
}
