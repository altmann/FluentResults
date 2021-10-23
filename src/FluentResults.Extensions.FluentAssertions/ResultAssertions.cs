using System;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

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

        public AndConstraint<ResultAssertions<T>> Satisfy(Action<Result> action)
        {
            action(Subject);

            return new AndConstraint<ResultAssertions<T>>(this);
        }
    }
}