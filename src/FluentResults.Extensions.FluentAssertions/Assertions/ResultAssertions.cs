using System;
using FluentAssertions;
using FluentAssertions.Primitives;

// ReSharper disable once CheckNamespace
namespace FluentResults.Extensions.FluentAssertions
{
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
            return new BeFailureAssertionOperator().Do(Subject, this, because, becauseArgs);
        }

        public AndWhichConstraint<ResultAssertions, Result> BeSuccess(string because = "", params object[] becauseArgs)
        {
            return new BeSuccessAssertionOperator().Do(Subject, this, because, becauseArgs);
        }

        public AndWhichThatConstraint<ResultAssertions, Result, ReasonAssertions> HaveReason(string message, Func<string, string, bool> messageComparison = null, string because = "", params object[] becauseArgs)
        {
            return new HaveReasonAssertionOperator().Do(Subject, this, message, messageComparison, because, becauseArgs);
        }
        
        public AndWhichThatConstraint<ResultAssertions, Result, ReasonAssertions> HaveReason<TReason>(string message, Func<string, string, bool> messageComparison = null, string because = "", params object[] becauseArgs) where TReason : IReason
        {
            return new HaveReasonTAssertionOperator().Do<ResultAssertions, Result, TReason>(Subject, this, message, messageComparison, because, becauseArgs);
        }

        public AndWhichThatConstraint<ResultAssertions, Result, ReasonAssertions> HaveError(string message, Func<string, string, bool> messageComparison = null, string because = "", params object[] becauseArgs)
        {
            return new HaveErrorAssertionOperator().Do(Subject, this, message, messageComparison, because, becauseArgs);
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
            return new BeFailureAssertionOperator().Do(Subject, this, because, becauseArgs);
        }

        public AndWhichConstraint<ResultAssertions<T>, Result<T>> BeSuccess(string because = "", params object[] becauseArgs)
        {
            return new BeSuccessAssertionOperator().Do(Subject, this, because, becauseArgs);
        }

        public AndWhichThatConstraint<ResultAssertions<T>, Result<T>, ReasonAssertions> HaveReason(string message, Func<string, string, bool> messageComparison = null, string because = "", params object[] becauseArgs)
        {
            return new HaveReasonAssertionOperator().Do(Subject, this, message, messageComparison, because, becauseArgs);
        }

        public AndWhichThatConstraint<ResultAssertions<T>, Result<T>, ReasonAssertions> HaveReason<TReason>(string message, Func<string, string, bool> messageComparison = null, string because = "", params object[] becauseArgs) where TReason : IReason
        {
            return new HaveReasonTAssertionOperator().Do<ResultAssertions<T>, Result<T>, TReason>(Subject, this, message, messageComparison, because, becauseArgs);
        }

        public AndWhichThatConstraint<ResultAssertions<T>, Result<T>, ReasonAssertions> HaveError(string message, Func<string, string, bool> messageComparison = null, string because = "", params object[] becauseArgs)
        {
            return new HaveErrorAssertionOperator().Do(Subject, this, message, messageComparison, because, becauseArgs);
        }

        public AndWhichConstraint<ResultAssertions<T>, Result<T>> HaveReason(IReason reason, string because = "", params object[] becauseArgs)
        {
            Subject.Reasons.Should().ContainEquivalentOf(reason, because, becauseArgs);

            return new AndWhichConstraint<ResultAssertions<T>, Result<T>>(this, Subject);
        }

        public AndConstraint<ResultAssertions<T>> HaveValue(T expectedValue, string because = "", params object[] becauseArgs)
        {
            return new HaveValueAssertionOperator().Do(Subject, this, expectedValue, because, becauseArgs);
        }

        public AndConstraint<ResultAssertions<T>> Satisfy(Action<Result<T>> action)
        {
            action(Subject);

            return new AndConstraint<ResultAssertions<T>>(this);
        }
    }
}