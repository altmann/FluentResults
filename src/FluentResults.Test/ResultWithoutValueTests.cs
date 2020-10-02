using FluentAssertions;
using System.Linq;
using Xunit;

namespace FluentResults.Test
{
    public class ResultWithoutValueTests
    {
        [Fact]
        public void CreateOkResult_SuccessResult()
        {
            // Act
            var okResult = Result.Ok();

            // Assert
            okResult.IsFailed.Should().BeFalse();
            okResult.IsSuccess.Should().BeTrue();

            okResult.Reasons.Should().BeEmpty();
            okResult.Errors.Should().BeEmpty();
            okResult.Successes.Should().BeEmpty();
        }

        [Fact]
        public void CreateOkResultWithSuccess_SuccessResultWithSuccess()
        {
            // Act
            var okResult = Result.Ok()
                .WithSuccess("First success message");

            // Assert
            okResult.Reasons.Should().HaveCount(1);
            okResult.Reasons.First().Should().BeOfType<Success>();
            okResult.Reasons.First().Message.Should().Be("First success message");
        }

        [Fact]
        public void CreateOkResultWith2Successes_SuccessResultWith2Successes()
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

        [Fact]
        public void FailIf_FailedConditionIsTrueAndWithStringErrorMessage_CreateFailedResult()
        {
            var result = Result.FailIf(true, "Error message");

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Single().Message.Should().Be("Error message");
        }

        [Fact]
        public void FailIf_FailedConditionIsTrueAndWithObjectErrorMessage_CreateFailedResult()
        {
            var result = Result.FailIf(true, new Error("Error message"));

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Single().Message.Should().Be("Error message");
        }

        [Fact]
        public void FailIf_FailedConditionIsFalseAndWithStringErrorMessage_CreateFailedResult()
        {
            var result = Result.FailIf(false, "Error message");

            // Assert
            result.IsFailed.Should().BeFalse();
        }

        [Fact]
        public void FailIf_FailedConditionIsFalseAndWithObjectErrorMessage_CreateFailedResult()
        {
            var result = Result.FailIf(false, new Error("Error message"));

            // Assert
            result.IsFailed.Should().BeFalse();
        }

        [Fact]
        public void OkIf_SuccessConditionIsTrueAndWithStringErrorMessage_CreateFailedResult()
        {
            var result = Result.OkIf(true, "Error message");

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void OkIf_SuccessConditionIsTrueAndWithObjectErrorMessage_CreateFailedResult()
        {
            var result = Result.OkIf(true, new Error("Error message"));

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void OkIf_SuccessConditionIsFalseAndWithStringErrorMessage_CreateFailedResult()
        {
            var result = Result.OkIf(false, "Error message");

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Single().Message.Should().Be("Error message");
        }

        [Fact]
        public void OkIf_SuccessConditionIsFalseAndWithObjectErrorMessage_CreateFailedResult()
        {
            var result = Result.OkIf(false, new Error("Error message"));

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Single().Message.Should().Be("Error message");
        }
    }
}