using System.Linq;
using FluentAssertions;
using Xunit;

namespace FluentResults.Tests.ResultWithoutValue
{
    public class OkTests
    {
        [Fact]
        public void A_newly_created_success_result_is_empty()
        {
            var okResult = Result.Ok();

            okResult.IsFailed.Should().BeFalse();
            okResult.IsSuccess.Should().BeTrue();
            okResult.Reasons.Should().BeEmpty();
            okResult.Errors.Should().BeEmpty();
            okResult.Successes.Should().BeEmpty();
        }
    }

    public class WithSuccessTests
    {
        [Fact]
        public void Add_a_success_to_an_existing_success_result()
        {
            var successResult = Result.Ok()
                .WithSuccess("Sample success message");

            successResult.Reasons.Should().HaveCount(1);

            var firstReason = successResult.Reasons.Single();
            AssertSuccessReason(new Success("Sample success message"), firstReason);
        }

        private static void AssertSuccessReason(Success expectedSuccess, Reason actualSuccess)
        {
            actualSuccess.Should().BeOfType<Success>();
            actualSuccess.Message.Should().Be("Sample success message");
            actualSuccess.Metadata.Should().BeEmpty();
        }

        [Fact]
        public void Add()
        {
            // Act
            var okResult = Result.Ok()
                .WithSuccess("First success message")
                .WithSuccess("Second success message");

            // Assert
            okResult.Reasons.Should().HaveCount(2);
            okResult.Reasons[0].Should().BeOfType<Success>();
            okResult.Reasons[1].Should().BeOfType<Success>();
            okResult.Reasons[0].Message.Should().Be("First success message");
            okResult.Reasons[1].Message.Should().Be("Second success message");
        }

        [Fact]
        public void CreateFailedResult_FailedResult()
        {
            // Act
            var result = Result.Fail("First error message");

            // Assert
            result.Reasons.Should().HaveCount(1);
            result.Reasons[0].Should().BeOfType<Error>();
            result.Reasons[0].Message.Should().Be("First error message");
        }

        [Fact]
        public void CreateFailedResultWith2Errors_FailedResultWith2Errors()
        {
            // Act
            var result = Result.Fail("First error message")
                .WithError("Second error message");

            // Assert
            result.Reasons.Should().HaveCount(2);
            result.Reasons[0].Should().BeOfType<Error>();
            result.Reasons[1].Should().BeOfType<Error>();
            result.Reasons[0].Message.Should().Be("First error message");
            result.Reasons[1].Message.Should().Be("Second error message");
        }

        [Fact]
        public void ToResult_ReturnFailedResult()
        {
            var result = Result.Fail("First error message");

            // Act
            var valueResult = result.ToResult<int>();

            // Assert
            valueResult.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void ImplicitCastOperator_ReturnFailedValueResult()
        {
            var result = Result.Fail("First error message");

            // Act
            Result<bool> valueResult = result;

            // Assert
            valueResult.IsFailed.Should().BeTrue();
        }
    }
}
