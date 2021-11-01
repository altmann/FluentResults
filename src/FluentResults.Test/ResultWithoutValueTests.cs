using System;
using System.Data.SqlTypes;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
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
            okResult.Should().BeOfType<Result>();
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
            okResult.Should().BeOfType<Result>();
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
        public void ToResult_WithOkResultAndValue_ReturnSuccessResult()
        {
            var valueResult = Result.Ok();

            // Act
            var result = valueResult.ToResult<float>(2.5f);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(2.5f);
        }

        [Fact]
        public void ToResult_WithOkResultWithoutValue_ReturnSuccessResult()
        {
            var valueResult = Result.Ok();

            // Act
            var result = valueResult.ToResult<bool>();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(default);
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


        [Fact]
        public void Try_execute_successfully_action_return_success_result()
        {
            void Action()
            {
            }

            var result = Result.Try(Action);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Try_execute_failed_action_return_failed_result()
        {
            var exception = new Exception("ex message");
            void Action() => throw exception;

            var result = Result.Try(Action);

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().HaveCount(1);

            var error = (ExceptionalError) result.Errors.First();
            error.Message.Should().Be(exception.Message);
            error.Exception.Should().Be(exception);
        }

        [Fact]
        public void Try_execute_failed_action_with_custom_catchHandler_return_failed_result()
        {
            var exception = new Exception("ex message");
            void Action() => throw exception;

            var result = Result.Try(Action, e => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }

        [Fact]
        public async Task Try_execute_successfully_async_action_return_success_result()
        {
            Task Action()
            {
                return Task.FromResult(0);
            }

            var result = await Result.Try(Action);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Try_execute_failed_async_action_return_failed_result()
        {
            var exception = new Exception("ex message");
            Task Action() => throw exception;

            var result = await Result.Try(Action);

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().HaveCount(1);

            var error = (ExceptionalError) result.Errors.First();
            error.Message.Should().Be(exception.Message);
            error.Exception.Should().Be(exception);
        }

        [Fact]
        public async Task Try_execute_failed_async_action_with_custom_catchHandler_return_failed_result()
        {
            var exception = new Exception("ex message");
            Task Action() => throw exception;

            var result = await Result.Try(Action, e => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }
    }
}