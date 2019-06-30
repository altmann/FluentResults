using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Linq;

namespace FluentResults.Test
{
    [TestClass]
    public class ResultWithoutValueTests
    {
        [TestMethod]
        public void CreateOkResult_SuccessResult()
        {
            // Act
            var okResult = Results.Ok();

            // Assert
            okResult.IsFailed.Should().BeFalse();
            okResult.IsSuccess.Should().BeTrue();

            okResult.Reasons.Should().BeEmpty();
            okResult.Errors.Should().BeEmpty();
            okResult.Successes.Should().BeEmpty();
        }

        [TestMethod]
        public void CreateOkResultWithSuccess_SuccessResultWithSuccess()
        {
            // Act
            var okResult = Results.Ok()
                .WithSuccess("First success message");

            // Assert
            okResult.Reasons.Should().HaveCount(1);
            okResult.Reasons.First().Should().BeOfType<Success>();
            okResult.Reasons.First().Message.Should().Be("First success message");
        }

        [TestMethod]
        public void CreateOkResultWith2Successes_SuccessResultWith2Successes()
        {
            // Act
            var okResult = Results.Ok()
                .WithSuccess("First success message")
                .WithSuccess("Second success message");

            // Assert
            okResult.Reasons.Should().HaveCount(2);
            okResult.Reasons[0].Should().BeOfType<Success>();
            okResult.Reasons[1].Should().BeOfType<Success>();
            okResult.Reasons[0].Message.Should().Be("First success message");
            okResult.Reasons[1].Message.Should().Be("Second success message");
        }

        [TestMethod]
        public void CreateFailedResult_FailedResult()
        {
            // Act
            var result = Results.Fail("First error message");

            // Assert
            result.Reasons.Should().HaveCount(1);
            result.Reasons[0].Should().BeOfType<Error>();
            result.Reasons[0].Message.Should().Be("First error message");
        }

        [TestMethod]
        public void CreateFailedResultWith2Errors_FailedResultWith2Errors()
        {
            // Act
            var result = Results.Fail("First error message")
                .WithError("Second error message");

            // Assert
            result.Reasons.Should().HaveCount(2);
            result.Reasons[0].Should().BeOfType<Error>();
            result.Reasons[1].Should().BeOfType<Error>();
            result.Reasons[0].Message.Should().Be("First error message");
            result.Reasons[1].Message.Should().Be("Second error message");
        }

        [TestMethod]
        public void ToResult_ReturnFailedResult()
        {
            var result = Results.Fail("First error message");

            // Act
            var valueResult = result.ToResult<int>();

            // Assert
            valueResult.IsFailed.Should().BeTrue();
        }

        [TestMethod]
        public void ImplicitCastOperator_ReturnFailedValueResult()
        {
            var result = Results.Fail("First error message");

            // Act
            Result<bool> valueResult = result;

            // Assert
            valueResult.IsFailed.Should().BeTrue();
        }
    }
}