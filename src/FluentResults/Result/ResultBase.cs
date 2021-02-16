using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public abstract class ResultBase
    {
        /// <summary>
        /// Is true if Reasons contains at least one error
        /// </summary>
        public bool IsFailed => Reasons.OfType<Error>().Any();

        /// <summary>
        /// Is true if Reasons contains no errors
        /// </summary>
        public bool IsSuccess => !IsFailed;

        /// <summary>
        /// Get all reasons (errors and successes)
        /// </summary>
        public List<Reason> Reasons { get; }

        /// <summary>
        /// Get all errors
        /// </summary>
        public List<Error> Errors => Reasons.OfType<Error>().ToList();

        /// <summary>
        /// Get all successes
        /// </summary>
        public List<Success> Successes => Reasons.OfType<Success>().ToList();

        protected ResultBase()
        {
            Reasons = new List<Reason>();
        }

        /// <summary>
        /// Check if the result object contains an error from a specific type
        /// </summary>
        public bool HasError<TError>() where TError : Error
        {
            return HasError<TError>(error => true);
        }

        /// <summary>
        /// Check if the result object contains an error from a specific type and with a specific condition
        /// </summary>
        public bool HasError<TError>(Func<TError, bool> predicate) where TError : Error
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasError(Errors, predicate);
        }

        /// <summary>
        /// Check if the result object contains an error with a specific condition
        /// </summary>
        public bool HasError(Func<Error, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasError(Errors, predicate);
        }

        /// <summary>
        /// Check if the result object contains a success from a specific type
        /// </summary>
        public bool HasSuccess<TSuccess>() where TSuccess : Success
        {
            return HasSuccess<TSuccess>(success => true);
        }

        /// <summary>
        /// Check if the result object contains a success from a specific type and with a specific condition
        /// </summary>
        public bool HasSuccess<TSuccess>(Func<TSuccess, bool> predicate) where TSuccess : Success
        {
            return ResultHelper.HasSuccess(Successes, predicate);
        }

        /// <summary>
        /// Check if the result object contains a success with a specific condition
        /// </summary>
        public bool HasSuccess(Func<Success, bool> predicate)
        {
            return ResultHelper.HasSuccess(Successes, predicate);
        }
    }

    public abstract class ResultBase<TResult> : ResultBase
        where TResult : ResultBase<TResult>
    {
        /// <summary>
        /// Add a reason (success or error)
        /// </summary>
        public TResult WithReason(Reason reason)
        {
            Reasons.Add(reason);
            return (TResult)this;
        }

        /// <summary>
        /// Add multiple reasons (success or error)
        /// </summary>
        public TResult WithReasons(IEnumerable<Reason> reasons)
        {
            Reasons.AddRange(reasons);
            return (TResult)this;
        }

        /// <summary>
        /// Add an error
        /// </summary>
        public TResult WithError(string errorMessage)
        {
            return WithError(new Error(errorMessage));
        }

        /// <summary>
        /// Add an error
        /// </summary>
        public TResult WithError(Error error)
        {
            return WithReason(error);
        }

        /// <summary>
        /// Add multiple errors
        /// </summary>
        public TResult WithErrors(IEnumerable<Error> errors)
        {
            return WithReasons(errors);
        }

        /// <summary>
        /// Add multiple errors
        /// </summary>
        public TResult WithErrors(IEnumerable<string> errors)
        {
            return WithReasons(errors.Select(errorMessage => new Error(errorMessage)));
        }

        /// <summary>
        /// Add an error
        /// </summary>
        public TResult WithError<TError>()
            where TError : Error, new()
        {
            return WithError(new TError());
        }

        /// <summary>
        /// Add a success
        /// </summary>
        public TResult WithSuccess(string successMessage)
        {
            return WithSuccess(new Success(successMessage));
        }

        /// <summary>
        /// Add a success
        /// </summary>
        public TResult WithSuccess(Success success)
        {
            return WithReason(success);
        }

        /// <summary>
        /// Add a success
        /// </summary>
        public TResult WithSuccess<TSuccess>()
            where TSuccess : Success, new()
        {
            return WithSuccess(new TSuccess());
        }

        /// <summary>
        /// Log the result. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult Log()
        {
            return Log(string.Empty);
        }

        /// <summary>
        /// Log the result with a specific logger context
        /// </summary>
        public TResult Log(string context)
        {
            var logger = Result.Settings.Logger;

            logger.Log(context, this);

            return (TResult)this;
        }

        protected virtual bool PrintMembers(StringBuilder builder)
        {
            builder.Append(nameof(IsSuccess));
            builder.Append(" = ");
            builder.Append(IsSuccess);

            if (Reasons.Count != 0)
            {
                builder.Append(", ");

                builder.Append(nameof(Reasons));
                builder.Append(" = [ ");

                foreach (var reason in Reasons)
                {
                    builder.Append(reason);
                    builder.Append(", ");
                }

                builder.Remove(builder.Length - 2, 2);
                builder.Append(" ]");
            }

            return true;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(nameof(ResultBase));
            builder.Append(" { ");

            if (PrintMembers(builder))
                builder.Append(" ");

            builder.Append("}");
            return builder.ToString();
        }
    }
}
