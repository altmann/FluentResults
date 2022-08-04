using System;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace FluentResults.Extensions.FluentAssertions.Test.ResultTests
{
    public class HaveReason2Tests
    {
        [Fact]
        public void A_result_with_reason_with_specific_metadata_throw_no_exception()
        {
            var failedResult = Result.Fail(new SomeReason("Error 1")
                                               .WithMetadata("key", "value"));

            Action action = () => failedResult
                                  .Should()
                                  .BeFailure()
                                  .And.HaveReason("Error 1")
                                  .That.HaveMetadata("key", "value");

            action.Should().NotThrow();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("value1")]
        [InlineData(1)]
        public void A_result_with_reason_with_another_metadata_because_of_value_throw_exception(object expectedMetadataValue)
        {
            var failedResult = Result.Fail(new SomeReason("Error 1")
                                               .WithMetadata("key", "value"));

            Action action = () => failedResult
                                  .Should()
                                  .BeFailure()
                                  .And.HaveReason("Error 1")
                                  .That.HaveMetadata("key", expectedMetadataValue);

            action.Should()
                  .Throw<XunitException>()
                  .WithMessage($"Reason should contain 'key' with '{expectedMetadataValue}', but not contain it");
        }

        [Theory]
        [InlineData("ke")]
        [InlineData("key1")]
        public void A_result_with_reason_with_another_metadata_because_of_key_throw_exception(string expectedMetadataKey)
        {
            var failedResult = Result.Fail(new SomeReason("Error 1")
                                               .WithMetadata("key", "value"));

            Action action = () => failedResult
                                  .Should()
                                  .BeFailure()
                                  .And.HaveReason("Error 1")
                                  .That.HaveMetadata(expectedMetadataKey, "value");

            action.Should()
                  .Throw<XunitException>()
                  .WithMessage($"Reason should contain '{expectedMetadataKey}' with 'value', but not contain it");
        }
    }
}