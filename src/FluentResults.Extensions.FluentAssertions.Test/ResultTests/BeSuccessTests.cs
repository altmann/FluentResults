using System;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

// ReSharper disable once CheckNamespace
namespace FluentResults.Extensions.FluentAssertions.Test.ResultTests
{
    public class BeSuccessTests
    {
        [Fact]
        public void A_success_result_throw_no_exception()
        {
            var successResult = Result.Ok();

            Action action = () => successResult.Should().BeSuccess();

            action.Should().NotThrow();
        }

        [Fact]
        public void A_failed_result_with_one_error_throw_a_exception()
        {
            var failedResult = Result.Fail("Error 1");

            Action action = () => failedResult.Should().BeSuccess();

            action.Should()
                .Throw<XunitException>()
                .WithMessage("Expected result be success, but is failed because of '{Error with Message='Error 1'}'");
        }

        [Fact]
        public void A_failed_result_with_multiple_errors_throw_a_exception()
        {
            var failedResult = Result.Fail("Error 1")
                .WithError("Error 2");

            Action action = () => failedResult.Should().BeSuccess();

            action.Should()
                .Throw<XunitException>()
                .WithMessage(
                    "Expected result be success, but is failed because of '{Error with Message='Error 1', Error with Message='Error 2'}'");
        }

        [Fact]
        public void A_failed_result_with_a_success_throw_a_exception()
        {
            var failedResult = Result.Fail("Error 1")
                .WithSuccess("Success 1");

            Action action = () => failedResult.Should().BeSuccess();

            action.Should()
                .Throw<XunitException>()
                .WithMessage("Expected result be success, but is failed because of '{Error with Message='Error 1'}'");
        }
    }
}