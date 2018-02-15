using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System;

namespace FluentResults.Test
{
    [TestClass]
    public class ResultWithValueTests
    {
        [TestMethod]
        public void CreateOkResult_SuccessResult()
        {
            // Act
            var okResult = Results.Ok<int>();

            // Assert
            okResult.IsFailed.Should().BeFalse();
            okResult.IsSuccess.Should().BeTrue();

            okResult.Reasons.Should().BeEmpty();
            okResult.Errors.Should().BeEmpty();
            okResult.Successes.Should().BeEmpty();
            okResult.Value.Should().Be(0);
        }

        [TestMethod]
        public void CreateOkResultWithValue_SuccessResultWithValue()
        {
            // Act
            var okResult = Results.Ok(5);

            // Assert
            okResult.Value.Should().Be(5);
        }

        [TestMethod]
        public void CreateOkResultWithValue2_SuccessResultWithValue()
        {
            // Act
            var okResult = Results.Ok<int>()
                .WithValue(5);

            // Assert
            okResult.Value.Should().Be(5);
        }

        [TestMethod]
        public void CreateFailedResultWithNoExplicitValue_FailedResult()
        {
            // Act
            var result = Results.Fail<int>("First error message");

            // Assert
            result.ValueOrDefault.Should().Be(0);
        }

        [TestMethod]
        public void CreateFailedResult_FailedResult()
        {
            // Act
            var result = Results.Fail<int>("First error message");

            // Assert

            Action action = () => { var v = result.Value; };

            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Result is in status failed. Value is not set.");
        }

        [TestMethod]
        public void CreateFailedResultWithExplicitValue_ShouldThrowException()
        {
            var failedResult = Results.Fail<int>("First error message");

            // Act
            Action action = () => { failedResult.WithValue(5); };

            // Assert
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Result is in status failed. Value is not set.");
        }
    }
}