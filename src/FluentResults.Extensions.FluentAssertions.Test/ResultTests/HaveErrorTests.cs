using System;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace FluentResults.Extensions.FluentAssertions.Test.ResultTests
{
    public class HaveErrorTests
    {
        [Theory]
        [InlineData("Error 1")]
        [InlineData("Error")]
        public void A_result_with_expected_reason_throw_no_exception(string expectedError)
        {
            var failedResult = Result.Fail("Error 1");

            Action action = () =>
                failedResult.Should().BeFailure().And
                    .HaveError(expectedError, MessageComparisonLogics.ActualContainsExpected);

            action.Should().NotThrow();
        }

        [Fact]
        public void A_result_without_expected_reason_throw_a_exception()
        {
            var successResult = Result.Fail("Error 2");

            Action action = () => successResult.Should().BeFailure().And.HaveError("Error 1");

            action.Should()
                .Throw<XunitException>()
                .WithMessage(
                    "Expected result to contain error with message containing \"Error 1\", but found error '{Error with Message='Error 2'}'");
        }
    }
}