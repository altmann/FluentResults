using System;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace FluentResults.Extensions.FluentAssertions.Test.ValueResultTests
{
    public class BeFailureTests
    {
        [Fact]
        public void A_failed_result_throw_no_exception()
        {
            var failedResult = Result.Fail<int>("Error 1");

            Action action = () => failedResult.Should().BeFailure();

            action.Should().NotThrow();
        }
        
        [Fact]
        public void A_success_result_throw_a_exception()
        {
            var successResult = Result.Ok(1);

            Action action = () => successResult.Should().BeFailure();
            
            action.Should()
                .Throw<XunitException>()
                .WithMessage("Expected result be failed, but is success");
        }
    }
}
