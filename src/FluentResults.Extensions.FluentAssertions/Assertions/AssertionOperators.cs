using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;

// ReSharper disable once CheckNamespace
namespace FluentResults.Extensions.FluentAssertions
{
    internal class BeFailureAssertionOperator
    {
        public AndWhichConstraint<TResultAssertion, TResult> Do<TResultAssertion, TResult>(TResult subject, TResultAssertion parentConstraint, string because, params object[] becauseArgs)
            where TResult : ResultBase
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .Given(() => subject.IsFailed)
                   .ForCondition(isFailed => isFailed)
                   .FailWith("Expected result be failed, but is success");

            return new AndWhichConstraint<TResultAssertion, TResult>(parentConstraint, subject);
        }
    }

    internal class BeSuccessAssertionOperator
    {
        public AndWhichConstraint<TResultAssertion, TResult> Do<TResultAssertion, TResult>(TResult subject, TResultAssertion parentConstraint, string because, params object[] becauseArgs)
            where TResult : ResultBase
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .Given(() => subject.IsSuccess)
                   .ForCondition(isSuccess => isSuccess)
                   .FailWith("Expected result be success, but is failed because of '{0}'", subject.Errors);

            return new AndWhichConstraint<TResultAssertion, TResult>(parentConstraint, subject);
        }
    }

    internal class HaveReasonAssertionOperator
    {
        public AndWhichThatConstraint<TResultAssertion, TResult, ReasonAssertions> Do<TResultAssertion, TResult>(TResult subject, TResultAssertion parentConstraint, string expectedMessage, Func<string, string, bool> messageComparison, string because, params object[] becauseArgs)
            where TResult : ResultBase
        {
            messageComparison = messageComparison ?? FluentResultAssertionsConfig.MessageComparison;

            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .Given(() => subject.Reasons)
                   .ForCondition(reasons => reasons.Any(reason => messageComparison(reason.Message, expectedMessage)))
                   .FailWith("Expected result to contain reason with message containing {0}, but found reasons '{1}'", expectedMessage, subject.Reasons);

            return new AndWhichThatConstraint<TResultAssertion, TResult, ReasonAssertions>(parentConstraint, subject, new ReasonAssertions(subject.Reasons.SingleOrDefault(reason => messageComparison(reason.Message, expectedMessage))));
        }
    }

    internal class HaveErrorAssertionOperator
    {
        public AndWhichThatConstraint<TResultAssertion, TResult, ReasonAssertions> Do<TResultAssertion, TResult>(TResult subject, TResultAssertion parentConstraint, string expectedMessage, Func<string, string, bool> messageComparison, string because, params object[] becauseArgs)
            where TResult : ResultBase
        {
            messageComparison = messageComparison ?? FluentResultAssertionsConfig.MessageComparison;

            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .Given(() => subject.Errors)
                   .ForCondition(errors => errors.Any(reason => messageComparison(reason.Message, expectedMessage)))
                   .FailWith("Expected result to contain error with message containing {0}, but found error '{1}'", expectedMessage, subject.Errors);

            return new AndWhichThatConstraint<TResultAssertion, TResult, ReasonAssertions>(parentConstraint, subject, new ReasonAssertions(subject.Reasons.SingleOrDefault(reason => messageComparison(reason.Message, expectedMessage))));
        }
    }

    internal class HaveReasonTAssertionOperator
    {
        public AndWhichThatConstraint<TResultAssertion, TResult, ReasonAssertions> Do<TResultAssertion, TResult, TReason>(TResult subject, TResultAssertion parentConstraint, string expectedMessage, Func<string, string, bool> messageComparison, string because, params object[] becauseArgs)
            where TResult : ResultBase
            where TReason : IReason
        {
            messageComparison = messageComparison ?? FluentResultAssertionsConfig.MessageComparison;

            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .Given(() => subject.Reasons.OfType<TReason>())
                   .ForCondition(reasons => reasons.Any(reason => messageComparison(reason.Message, expectedMessage)))
                   .FailWith("Expected result to contain reason of type {0} with message containing {1}, but found reasons '{2}'", typeof(TReason).Name, expectedMessage, subject.Reasons);

            return new AndWhichThatConstraint<TResultAssertion, TResult, ReasonAssertions>(parentConstraint, subject, new ReasonAssertions(subject.Reasons.SingleOrDefault(reason => messageComparison(reason.Message, expectedMessage))));
        }
    }

    internal class HaveValueAssertionOperator
    {
        public AndConstraint<TResultAssertion> Do<T, TResultAssertion>(Result<T> subject, TResultAssertion parentConstraint, T expectedValue, string because, params object[] becauseArgs)
        {
            Execute.Assertion
                   .BecauseOf(because)
                   .ForCondition(subject.IsSuccess)
                   .FailWith("Value can not be asserted because result is failed because of '{0}'", subject.Errors)
                   .Then
                   .Given(() => subject.Value)
                   .ForCondition(actualValue => (actualValue == null && expectedValue == null) || actualValue.Equals(expectedValue))
                   .FailWith("Expected value is '{0}', but is '{1}'", expectedValue, subject.Value);

            return new AndConstraint<TResultAssertion>(parentConstraint);
        }
    }
}
