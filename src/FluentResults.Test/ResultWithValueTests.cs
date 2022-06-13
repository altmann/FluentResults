using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FluentResults.Test
{
    public class ResultWithValueTests
    {
        [Fact]
        public void Ok_WithNoParams_ShouldReturnSuccessResult()
        {
            // Act
            var okResult = Result.Ok<int>(default);

            // Assert
            okResult.IsFailed.Should().BeFalse();
            okResult.IsSuccess.Should().BeTrue();

            okResult.Reasons.Should().BeEmpty();
            okResult.Errors.Should().BeEmpty();
            okResult.Successes.Should().BeEmpty();
            okResult.Value.Should().Be(0);
            okResult.ValueOrDefault.Should().Be(0);
        }

        [Fact]
        public void Ok_WithValidValue_ShouldReturnSuccessResult()
        {
            // Act
            var okResult = Result.Ok(5);

            // Assert
            okResult.IsSuccess.Should().BeTrue();
            okResult.Value.Should().Be(5);
            okResult.ValueOrDefault.Should().Be(5);
        }

        [Fact]
        public void WithValue_WithValidParam_ShouldReturnSuccessResult()
        {
            var okResult = Result.Ok<int>(default);

            // Act
            okResult.WithValue(5);

            // Assert
            okResult.Value.Should().Be(5);
            okResult.ValueOrDefault.Should().Be(5);
        }

        [Fact]
        public void CreateOkResultWithSuccess_SuccessResultWithSuccess()
        {
            // Act
            var okResult = Result.Ok<int>(default)
                .WithSuccess("First success message");

            // Assert
            okResult.Reasons.Should().HaveCount(1);
            okResult.Reasons.First().Should().BeOfType<Success>();
            okResult.Reasons.First().Message.Should().Be("First success message");

            okResult.Successes.Should().HaveCount(1);
            okResult.Successes.First().Should().BeOfType<Success>();
            okResult.Successes.First().Message.Should().Be("First success message");

            okResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void CreateOkResultWith2Successes_SuccessResultWith2Successes()
        {
            // Act
            var okResult = Result.Ok<int>(default)
                .WithSuccess("First success message")
                .WithSuccess("Second success message");

            // Assert
            okResult.Reasons.Should().HaveCount(2);
            okResult.Reasons[0].Should().BeOfType<Success>();
            okResult.Reasons[1].Should().BeOfType<Success>();
            okResult.Reasons[0].Message.Should().Be("First success message");
            okResult.Reasons[1].Message.Should().Be("Second success message");

            okResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Fail_WithValidErrorMessage_ShouldReturnFailedResult()
        {
            // Act
            var result = Result.Fail<int>("Error message");

            // Assert
            result.IsFailed.Should().BeTrue();
            result.ValueOrDefault.Should().Be(0);
        }

        [Fact]
        public void ValueOrDefault_WithDateTime_ShouldReturnFailedResult()
        {
            var result = Result.Fail<DateTime>("Error message");

            // Act
            var valueOrDefault = result.ValueOrDefault;

            // Assert
            var defaultDateTime = default(DateTime);
            valueOrDefault.Should().Be(defaultDateTime);
        }

        class TestValue
        {

        }

        [Fact]
        public void ValueOrDefault_WithObject_ShouldReturnFailedResult()
        {
            var result = Result.Fail<TestValue>("Error message");

            // Act
            var valueOrDefault = result.ValueOrDefault;

            // Assert
            valueOrDefault.Should().Be(null);
        }

        [Fact]
        public void Value_WithResultInFailedState_ShouldThrowException()
        {
            // Act
            var result = Result.Fail<int>("Error message");

            // Assert

            Action action = () => { var v = result.Value; };

            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Result is in status failed. Value is not set. Having: Error with Message='Error message'");
        }

        [Fact]
        public void Value_WithResultInFailedStateWithMultipleErrors_ShouldThrowException()
        {
            // Act
            var result = Result.Fail<int>("Error message")
                               .WithError("Actual error");

            // Assert

            Action action = () => { var v = result.Value; };

            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Result is in status failed. Value is not set. Having: Error with Message='Error message'; Error with Message='Actual error'");
        }

        [Fact]
        public void WithValue_WithResultInFailedState_ShouldThrowException()
        {
            var failedResult = Result.Fail<int>("Error message");

            // Act
            Action action = () => { failedResult.WithValue(5); };

            // Assert
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Result is in status failed. Value is not set. Having: Error with Message='Error message'");
        }

        [Fact]
        public void ToResult_ReturnFailedResult()
        {
            var valueResult = Result.Fail<int>("First error message");

            // Act
            var result = valueResult.ToResult();

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void ToResult_ToAntotherValueType_ReturnFailedResult()
        {
            var valueResult = Result.Fail<int>("First error message");

            // Act
            var result = valueResult.ToResult<float>();

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void ToResult_ToAntotherValueTypeWithOkResultAndNoConverter_ReturnFailedResult()
        {
            var valueResult = Result.Fail<int>("Failed");

            // Act
            var result = valueResult.ToResult<float>();

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void ToResult_ToAntotherValueTypeWithOkResultAndConverter_ReturnSuccessResult()
        {
            var valueResult = Result.Ok(4);

            // Act
            var result = valueResult.ToResult<float>(v => v);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(4);
        }

        [Fact]
        public void ImplicitCastOperator_ReturnFailedResult()
        {
            var valueResult = Result.Fail<int>("First error message");

            // Act
            Result result = valueResult.ToResult();

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void Try_execute_successfully_action_return_success_result()
        {
            int Action() => 5;
            var result = Result.Try(Action);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(5);
        }

        [Fact]
        public void Try_execute_failed_action_return_failed_result()
        {
            var exception = new Exception("ex message");
            int Action() => throw exception;

            var result = Result.Try(Action);

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().HaveCount(1);

            var error = (ExceptionalError)result.Errors.First();
            error.Message.Should().Be(exception.Message);
            error.Exception.Should().Be(exception);
        }

        [Fact]
        public void Try_execute_failed_action_with_custom_catchHandler_return_failed_result()
        {
            var exception = new Exception("ex message");
            int Action() => throw exception;

            var result = Result.Try(Action, e => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }


        [Fact]
        public async Task Try_execute_successfully_async_action_return_success_result()
        {
            Task<int> Action() => Task.FromResult(5);
            var result = await Result.Try(Action);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(5);
        }

        [Fact]
        public async Task Try_execute_failed_async_action_return_failed_result()
        {
            var exception = new Exception("ex message");
            Task<int> Action() => throw exception;

            var result = await Result.Try(Action);

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().HaveCount(1);

            var error = (ExceptionalError)result.Errors.First();
            error.Message.Should().Be(exception.Message);
            error.Exception.Should().Be(exception);
        }

        [Fact]
        public async Task Try_execute_failed_async_action_with_custom_catchHandler_return_failed_result()
        {
            var exception = new Exception("ex message");
            Task<int> Action() => throw exception;

            var result = await Result.Try(Action, e => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }
    }
}