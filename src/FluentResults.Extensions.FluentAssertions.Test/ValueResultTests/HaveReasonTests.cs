using FluentAssertions;
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentResults.Extensions.FluentAssertions.Test.ValueResultTests
{
    public class HaveReasonTests
    {
        [Theory]
        [InlineData("Error 1")]
        [InlineData("Error")]
        public void A_result_with_expected_reason_throw_no_exception(string expectedError)
        {
            var failedResult = Result.Fail<int>("Error 1");

            Action action = () => failedResult.Should().BeFailure().And.HaveReason(expectedError, MessageComparisonLogics.ActualContainsExpected);

            action.Should().NotThrow();
        }

        [Fact]
        public void A_result_without_expected_reason_throw_a_exception()
        {
            var successResult = Result.Fail<int>("Error 2");

            Action action = () => successResult.Should().BeFailure().And.HaveReason("Error 1");

            action.Should()
                .Throw<XunitException>()
                .WithMessage("Expected result to contain reason with message containing \"Error 1\", but found reasons '{Error with Message='Error 2'}'");
        }

        [Theory]
        [InlineData("Error 1")]
        [InlineData("Error")]
        public void A_result_with_expected_reason_of_type_throw_no_exception(string expectedError)
        {
            var successResult = Result.Fail<int>(new SomeReason("Error 1"));

            Action action = () => successResult.Should().BeFailure().And.HaveReason<SomeReason>(expectedError, MessageComparisonLogics.ActualContainsExpected);

            action.Should().NotThrow();
        }

        [Fact]
        public void A_result_without_expected_reason_of_type_throw_a_exception()
        {
            var successResult = Result.Fail<int>("Error 1");

            Action action = () => successResult.Should().BeFailure().And.HaveReason<SomeReason>("Error 1");

            action.Should()
                .Throw<XunitException>()
                .WithMessage("Expected result to contain reason of type \"SomeReason\" with message containing \"Error 1\", but found reasons '{Error with Message='Error 1'}'");
        }

        [Fact]
        public void A_result_with_expected_reason_object_throw_no_exception()
        {
            var successResult = Result.Fail<int>("Error 1");

            Action action = () => successResult.Should().BeFailure().And.HaveReason(new Error("Error 1"));

            action.Should().NotThrow();
        }

        [Fact]
        public void A_result_without_expected_reason_object_throw_a_exception()
        {
            var successResult = Result.Fail<int>("Error 1");

            Action action = () => successResult.Should().BeFailure().And.HaveReason(new Error("Error 2"));

            action.Should()
                .Throw<XunitException>()
                .WithMessage("Expected Subject.Reasons {Error with Message='Error 1'} to contain equivalent of Error with Message='Error 2'*");
        }
    }
}
