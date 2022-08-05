using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentResults.Extensions.FluentAssertions
{
    public class ResultReasonAndWhichConstraint : AndWhichConstraint<ResultAssertions, Result>
    {
        private readonly ResultAssertions _parentConstraint;
        private readonly Result _matchedConstraint;
        private readonly IReason _reason;

        public ResultReasonAndWhichConstraint(ResultAssertions parentConstraint, Result matchedConstraint, IReason reason) 
            : base(parentConstraint, matchedConstraint)
        {
            _parentConstraint = parentConstraint;
            _matchedConstraint = matchedConstraint;
            _reason = reason;
        }

        public ResultReasonAndWhichConstraint(ResultAssertions parentConstraint, Result matchedConstraint, IEnumerable<IReason> reasons) 
            : base(parentConstraint, matchedConstraint)
        {
        }

        public ReasonAssertions That => new ReasonAssertions(_matchedConstraint, _reason);
    }

    public class ReasonAndWhichConstraint : AndWhichConstraint<ReasonAssertions, IReason>
    {
        private readonly ReasonAssertions _parentConstraint;
        private readonly IReason _matchedConstraint;
        private readonly Result _result;

        public ReasonAndWhichConstraint(ReasonAssertions parentConstraint, IReason matchedConstraint, Result result)
            : base(parentConstraint, matchedConstraint)
        {
            _parentConstraint = parentConstraint;
            _matchedConstraint = matchedConstraint;
            _result = result;
        }

        public ReasonAndWhichConstraint(ReasonAssertions parentConstraint, IReason matchedConstraint, IEnumerable<Result> result)
            : base(parentConstraint, matchedConstraint)
        {
        }
    }

    public class ReasonAssertions : ReferenceTypeAssertions<IReason, ReasonAssertions>
    {
        private readonly Result _result;

        public ReasonAssertions(Result result, IReason subject)
            : base(subject)
        {
            _result = result;
        }

        protected override string Identifier => nameof(IReason);

        public AndWhichConstraint<ReasonAssertions, IReason> HaveMetadata(string metadataKey, object metadataValue, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .Given(() => Subject.Metadata)
                   .ForCondition(metadata =>
                                 {
                                     metadata.TryGetValue(metadataKey, out var actualMetadataValue);
                                     return actualMetadataValue == metadataValue;
                                 })
                   .FailWith($"Reason should contain '{metadataKey}' with '{metadataValue}', but not contain it");

            return new AndWhichConstraint<ReasonAssertions, IReason>(this, Subject);
        }

        public AndWhichConstraint<ReasonAssertions, IReason> Satisfy<TReason>(Action<TReason> action) where TReason : class, IReason
        {
            var specificReason =  Subject as TReason;

            Execute.Assertion
                   .Given(() => Subject)
                   .ForCondition(reason => reason is TReason)
                   .FailWith($"Reason should be of type '{typeof(TReason)}', but is of type '{Subject.GetType()}'");

            action(specificReason);

            return new AndWhichConstraint<ReasonAssertions, IReason>(this, Subject);
        }
    }

    public class ResultAssertions : ReferenceTypeAssertions<Result, ResultAssertions>
    {
        static ResultAssertions()
        {
            ResultFormatters.Register();
        }

        public ResultAssertions(Result subject)
            : base(subject)
        {

        }

        protected override string Identifier => nameof(Result);

        public AndWhichConstraint<ResultAssertions, Result> BeFailure(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => Subject.IsFailed)
                .ForCondition(isFailed => isFailed)
                .FailWith("Expected result be failed, but is success");

            return new AndWhichConstraint<ResultAssertions, Result>(this, Subject);
        }

        public AndWhichConstraint<ResultAssertions, Result> BeSuccess(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => Subject.IsSuccess)
                .ForCondition(isSuccess => isSuccess)
                .FailWith("Expected result be success, but is failed because of '{0}'", Subject.Errors);

            return new AndWhichConstraint<ResultAssertions, Result>(this, Subject);
        }

        public ResultReasonAndWhichConstraint HaveReason(string message, Func<string, string, bool> errorMessageComparison = null, string because = "", params object[] becauseArgs)
        {
            errorMessageComparison = errorMessageComparison ?? FluentResultAssertionsConfig.ErrorMessageComparison;
            var actualReason = Subject.Reasons.SingleOrDefault(reason => errorMessageComparison(reason.Message, message));

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => Subject.Reasons)
                .ForCondition(reasons => actualReason != null)
                .FailWith("Expected result to contain reason with message containing {0}, but found reasons '{1}'", message, Subject.Reasons);

            return new ResultReasonAndWhichConstraint(this, Subject, actualReason);
        }

        public AndWhichConstraint<ResultAssertions, Result> HaveReason<TReason>(string message, Func<string, string, bool> errorMessageComparison = null, string because = "", params object[] becauseArgs) where TReason : IReason
        {
            errorMessageComparison = errorMessageComparison ?? FluentResultAssertionsConfig.ErrorMessageComparison;

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => Subject.Reasons.OfType<TReason>())
                .ForCondition(reasons => reasons.Any(reason => errorMessageComparison(reason.Message, message)))
                .FailWith("Expected result to contain reason of type {0} with message containing {1}, but found reasons '{2}'", typeof(TReason).Name, message, Subject.Reasons);

            return new AndWhichConstraint<ResultAssertions, Result>(this, Subject);
        }

        public AndWhichConstraint<ResultAssertions, Result> HaveReason(IReason reason, string because = "", params object[] becauseArgs)
        {
            Subject.Reasons.Should().ContainEquivalentOf(reason, because, becauseArgs);

            return new AndWhichConstraint<ResultAssertions, Result>(this, Subject);
        }

        public AndConstraint<ResultAssertions> Satisfy(Action<Result> action)
        {
            action(Subject);

            return new AndConstraint<ResultAssertions>(this);
        }
    }

    public class ResultAssertions<T> : ReferenceTypeAssertions<Result<T>, ResultAssertions<T>>
    {
        static ResultAssertions()
        {
            ResultFormatters.Register();
        }

        public ResultAssertions(Result<T> subject)
            : base(subject)
        {
        }

        protected override string Identifier => nameof(Result);

        public AndWhichConstraint<ResultAssertions<T>, Result<T>> BeFailure(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => Subject.IsFailed)
                .ForCondition(actualIsFailed => actualIsFailed)
                .FailWith("Expected result be failed, but is success");

            return new AndWhichConstraint<ResultAssertions<T>, Result<T>>(this, Subject);
        }

        public AndWhichConstraint<ResultAssertions<T>, Result<T>> BeSuccess(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => Subject.IsSuccess)
                .ForCondition(actualIsSuccess => actualIsSuccess)
                .FailWith("Expected result be success, but is failed because of '{0}'", Subject.Errors);

            return new AndWhichConstraint<ResultAssertions<T>, Result<T>>(this, Subject);
        }

        public AndWhichConstraint<ResultAssertions<T>, Result<T>> HaveReason(string message, Func<string, string, bool> errorMessageComparison = null, string because = "", params object[] becauseArgs)
        {
            errorMessageComparison = errorMessageComparison ?? FluentResultAssertionsConfig.ErrorMessageComparison;

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => Subject.Reasons)
                .ForCondition(reasons => reasons.Any(reason => errorMessageComparison(reason.Message, message)))
                .FailWith("Expected result to contain reason with message containing {0}, but found reasons '{1}'", message, Subject.Reasons);

            return new AndWhichConstraint<ResultAssertions<T>, Result<T>>(this, Subject);
        }

        public AndWhichConstraint<ResultAssertions<T>, Result<T>> HaveReason<TReason>(string message, Func<string, string, bool> errorMessageComparison = null, string because = "", params object[] becauseArgs) where TReason : IReason
        {
            errorMessageComparison = errorMessageComparison ?? FluentResultAssertionsConfig.ErrorMessageComparison;

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => Subject.Reasons.OfType<TReason>())
                .ForCondition(reasons => reasons.Any(reason => errorMessageComparison(reason.Message, message)))
                .FailWith("Expected result to contain reason of type {0} with message containing {1}, but found reasons '{2}'", typeof(TReason).Name, message, Subject.Reasons);

            return new AndWhichConstraint<ResultAssertions<T>, Result<T>>(this, Subject);
        }

        public AndWhichConstraint<ResultAssertions<T>, Result<T>> HaveReason(IReason reason, string because = "", params object[] becauseArgs)
        {
            Subject.Reasons.Should().ContainEquivalentOf(reason, because, becauseArgs);

            return new AndWhichConstraint<ResultAssertions<T>, Result<T>>(this, Subject);
        }

        public AndConstraint<ResultAssertions<T>> HaveValue(T expectedValue, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because)
                .ForCondition(Subject.IsSuccess)
                .FailWith("Value can not be asserted because result is failed because of '{0}'", Subject.Errors)
                .Then
                .Given(() => Subject.Value)
                .ForCondition(actualValue => actualValue.Equals(expectedValue))
                .FailWith("Expected value is '{0}', but is '{1}'", expectedValue, Subject.Value);

            return new AndConstraint<ResultAssertions<T>>(this);
        }

        public AndConstraint<ResultAssertions<T>> Satisfy(Action<Result<T>> action)
        {
            action(Subject);

            return new AndConstraint<ResultAssertions<T>>(this);
        }
    }
}