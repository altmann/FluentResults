using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System;

namespace FluentResults.Test
{
    [TestClass]
    public class ResultWithValueTests
    {
        [TestMethod]
        public void Ok_WithNoParams_ShouldReturnSuccessResult()
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
            okResult.ValueOrDefault.Should().Be(0);
        }

        [TestMethod]
        public void Ok_WithValidValue_ShouldReturnSuccessResult()
        {
            // Act
            var okResult = Results.Ok(5);

            // Assert
            okResult.IsSuccess.Should().BeTrue();
            okResult.Value.Should().Be(5);
            okResult.ValueOrDefault.Should().Be(5);
        }

        [TestMethod]
        public void WithValue_WithValidParam_ShouldReturnSuccessResult()
        {
            var okResult = Results.Ok<int>();

            // Act
            okResult.WithValue(5);

            // Assert
            okResult.Value.Should().Be(5);
            okResult.ValueOrDefault.Should().Be(5);
        }

        [TestMethod]
        public void Fail_WithValidErrorMessage_ShouldReturnFailedResult()
        {
            // Act
            var result = Results.Fail<int>("Error message");

            // Assert
            result.IsFailed.Should().BeTrue();
            result.ValueOrDefault.Should().Be(0);
        }

        [TestMethod]
        public void ValueOrDefault_WithDateTime_ShouldReturnFailedResult()
        {
            var result = Results.Fail<DateTime>("Error message");

            // Act
            var valueOrDefault = result.ValueOrDefault;

            // Assert
            var defaultDateTime = default(DateTime);
            valueOrDefault.Should().Be(defaultDateTime);
        }

        class TestValue
        {

        }

        [TestMethod]
        public void ValueOrDefault_WithObject_ShouldReturnFailedResult()
        {
            var result = Results.Fail<TestValue>("Error message");

            // Act
            var valueOrDefault = result.ValueOrDefault;

            // Assert
            valueOrDefault.Should().Be(null);
        }

        [TestMethod]
        public void Value_WithResultInFailedState_ShouldThrowException()
        {
            // Act
            var result = Results.Fail<int>("Error message");

            // Assert

            Action action = () => { var v = result.Value; };

            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Result is in status failed. Value is not set.");
        }

        [TestMethod]
        public void WithValue_WithResultInFailedState_ShouldThrowException()
        {
            var failedResult = Results.Fail<int>("Error message");

            // Act
            Action action = () => { failedResult.WithValue(5); };

            // Assert
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Result is in status failed. Value is not set.");
        }

        [TestMethod]
        public void ToResult_ReturnFailedResult()
        {
            var valueResult = Results.Fail<int>("First error message");

            // Act
            var result = valueResult.ToResult();

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [TestMethod]
        public void ImplicitCastOperator_ReturnFailedResult()
        {
            var valueResult = Results.Fail<int>("First error message");

            // Act
            Result result = valueResult;

            // Assert
            result.IsFailed.Should().BeTrue();
        }
    }
}