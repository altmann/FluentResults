using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
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

        public bool HasError<TError>() where TError : Error
        {
            return HasError<TError>(error => true);
        }

        public bool HasError<TError>(Func<TError, bool> predicate) where TError : Error
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasError(Errors, predicate);
        }

        public bool HasError(Func<Error, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasError(Errors, predicate);
        }

        public bool HasSuccess<TSuccess>() where TSuccess : Success
        {
            return HasSuccess<TSuccess>(success => true);
        }

        public bool HasSuccess<TSuccess>(Func<TSuccess, bool> predicate) where TSuccess : Success
        {
            return ResultHelper.HasSuccess(Successes, predicate);
        }

        public bool HasSuccess(Func<Success, bool> predicate)
        {
            return ResultHelper.HasSuccess(Successes, predicate);
        }
    }

    public abstract class ResultBase<TResult> : ResultBase
        where TResult : ResultBase<TResult>
    {
        public TResult WithReason(Reason reason)
        {
            Reasons.Add(reason);
            return (TResult)this;
        }

        public TResult WithReasons(IEnumerable<Reason> reasons)
        {
            foreach (var reason in reasons)
                WithReason(reason);

            return (TResult)this;
        }

        public TResult WithError(string errorMessage)
        {
            return WithError(new Error(errorMessage));
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

        public TResult Log()
        {
            return Log(string.Empty);
        }

        public TResult Log(string context)
        {
            var logger = Result.Settings.Logger;

            logger.Log(context, this);

            return (TResult)this;
        }

        public override string ToString()
        {
            var reasonsString = Reasons.Any()
                ? $", Reasons='{ReasonFormat.ReasonsToString(Reasons)}'"
                : string.Empty;

            return $"Result: IsSuccess='{IsSuccess}'{reasonsString}";
        }
    }
}
