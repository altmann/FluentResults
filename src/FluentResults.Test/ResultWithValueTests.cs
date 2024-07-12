using FluentAssertions;
using System;
using System.Collections.Generic;
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
        public void Fail_WithValidErrorMessages_ShouldReturnFailedResult()
        {
            // Act
            var errors = new List<string> { "First error message", "Second error message" };
            var result = Result.Fail<int>(errors);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Reasons.Should().HaveCount(2);
            result.Reasons[0].Should().BeOfType<Error>();
            result.Reasons[1].Should().BeOfType<Error>();
            result.Reasons[0].Message.Should().Be("First error message");
            result.Reasons[1].Message.Should().Be("Second error message");
            result.ValueOrDefault.Should().Be(0);
        }

        [Fact]
        public void Fail_WithNullEnumerableOfErrorMessages_ShouldThrow()
        {
            // Act
            Action act = () => Result.Fail<int>((IEnumerable<string>)null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Fail_WithValidErrors_ShouldReturnFailedResult()
        {
            // Act
            var errors = new List<IError> { new Error("First error message"), new Error("Second error message") };
            var result = Result.Fail<int>(errors);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Reasons.Should().HaveCount(2);
            result.Reasons[0].Should().BeOfType<Error>();
            result.Reasons[1].Should().BeOfType<Error>();
            result.Reasons[0].Message.Should().Be("First error message");
            result.Reasons[1].Message.Should().Be("Second error message");
            result.ValueOrDefault.Should().Be(0);
        }

        [Fact]
        public void Fail_WithNullEnumerableOfErrors_ShouldThrow()
        {
            // Act
            Action act = () => Result.Fail<int>((IEnumerable<IError>)null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
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
            valueOrDefault.Should().BeNull();
        }

        [Fact]
        public void Value_WithResultInFailedState_ShouldThrowException()
        {
            // Act
            var result = Result.Fail<int>("Error message");

            // Assert

            Action action = () => { var _ = result.Value; };

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

            Action action = () => { var _ = result.Value; };

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
        public void ToResult_ToAnotherValueType_ReturnFailedResult()
        {
            var valueResult = Result.Fail<int>("First error message");

            // Act
            var result = valueResult.ToResult<float>();

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void ToResult_ToAnotherValueTypeWithOkResultAndNoConverter_ReturnFailedResult()
        {
            var valueResult = Result.Fail<int>("Failed");

            // Act
            var result = valueResult.ToResult<float>();

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public void ToResult_ToAnotherValueTypeWithOkResultAndConverter_ReturnSuccessResult()
        {
            var valueResult = Result.Ok(4);

            // Act
            var result = valueResult.ToResult<float>(v => v);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(4);
        }

        public class BindMethod
        {
            [Fact]
            public void Bind_ToAnotherValueTypeWithFailedResult_ReturnFailedResult()
            {
                var valueResult = Result.Fail<int>("First error message");

                // Act
                var result = valueResult.Bind(Result.Ok);

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("First error message");
            }

            [Fact]
            public async Task Bind_ToAnotherValueTypeWithFailedResult_ReturnFailedResultTask()
            {
                var valueResult = Result.Fail<int>("First error message");

                // Act
                var result = await valueResult.Bind(x => Task.FromResult(Result.Ok(x)));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("First error message");
            }

            [Fact]
            public async Task Bind_ToAnotherValueTypeWithFailedResult_ReturnFailedResultValueTask()
            {
                var valueResult = Result.Fail<int>("First error message");

                // Act
                var result = await valueResult.Bind(x => new ValueTask<Result<int>>(Result.Ok(x)));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("First error message");
            }

            [Fact]
            public void Bind_ToResultWithFailedResult_ReturnFailedResult()
            {
                var valueResult = Result.Fail<int>("First error message");

                // Act
                var result = valueResult.Bind(_ => Result.Ok());

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("First error message");
            }

            [Fact]
            public async Task Bind_ToResultWithFailedResult_ReturnFailedResultTask()
            {
                var valueResult = Result.Fail<int>("First error message");

                // Act
                var result = await valueResult.Bind(_ => Task.FromResult(Result.Ok()));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("First error message");
            }

            [Fact]
            public async Task Bind_ToResultWithFailedResult_ReturnFailedResultValueTask()
            {
                var valueResult = Result.Fail<int>("First error message");

                // Act
                var result = await valueResult.Bind(_ => new ValueTask<Result>(Result.Ok()));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("First error message");
            }

            [Fact]
            public void Bind_ToAnotherValueTypeWithFailedResultAndFailedTransformation_ReturnFailedResult()
            {
                var valueResult = Result.Fail<int>("Original error message");

                // Act
                var result = valueResult.Bind(_ => Result.Fail<string>("Irrelevant error"));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("Original error message");
            }

            [Fact]
            public async Task Bind_ToAnotherValueTypeWithFailedResultAndFailedTransformation_ReturnFailedResultTask()
            {
                var valueResult = Result.Fail<int>("Original error message");

                // Act
                var result = await valueResult.Bind(_ => Task.FromResult(Result.Fail<string>("Irrelevant error")));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("Original error message");
            }

            [Fact]
            public async Task Bind_ToAnotherValueTypeWithFailedResultAndFailedTransformation_ReturnFailedResultValueTask()
            {
                var valueResult = Result.Fail<int>("Original error message");

                // Act
                var result = await valueResult.Bind(_ => Task.FromResult(Result.Fail<string>("Irrelevant error")));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("Original error message");
            }

            [Fact]
            public void Bind_ToResultWithFailedResultAndFailedTransformation_ReturnFailedResult()
            {
                var valueResult = Result.Fail<int>("Original error message");

                // Act
                var result = valueResult.Bind(_ => Result.Fail("Irrelevant error"));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("Original error message");
            }

            [Fact]
            public async Task Bind_ToResultWithFailedResultAndFailedTransformation_ReturnFailedResultTask()
            {
                var valueResult = Result.Fail<int>("Original error message");

                // Act
                var result = await valueResult.Bind(_ => Task.FromResult(Result.Fail("Irrelevant error")));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("Original error message");
            }

            [Fact]
            public async Task Bind_ToResultWithFailedResultAndFailedTransformation_ReturnFailedResultValueTask()
            {
                var valueResult = Result.Fail<int>("Original error message");

                // Act
                var result = await valueResult.Bind(_ => new ValueTask<Result>(Result.Fail("Irrelevant error")));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("Original error message");
            }

            [Fact]
            public void Bind_ToAnotherValueTypeWhichIsSuccessful_ReturnsSuccessResult()
            {
                var valueResult = Result.Ok(1).WithSuccess("An int");

                // Act
                var result = valueResult.Bind(n => n == 1
                    ? "One".ToResult().WithSuccess("It is one")
                    : Result.Fail<string>("Only one accepted"));

                // Assert
                result.IsSuccess.Should().BeTrue();
                result.Value.Should()
                    .Be("One");

                result.Successes.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("An int", "It is one");
            }

            [Fact]
            public async Task Bind_ToAnotherValueTypeWhichIsSuccessful_ReturnsSuccessResultTask()
            {
                var valueResult = Result.Ok(1).WithSuccess("An int");

                // Act
                var result = await valueResult.Bind(n => Task.FromResult(n == 1
                    ? "One".ToResult().WithSuccess("It is one")
                    : Result.Fail<string>("Only one accepted")));

                // Assert
                result.IsSuccess.Should().BeTrue();
                result.Value.Should()
                    .Be("One");

                result.Successes.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("An int", "It is one");
            }

            [Fact]
            public async Task Bind_ToAnotherValueTypeWhichIsSuccessful_ReturnsSuccessResultValueTask()
            {
                var valueResult = Result.Ok(1).WithSuccess("An int");

                // Act
                var result = await valueResult.Bind(n => new ValueTask<Result<string>>(n == 1
                    ? "One".ToResult().WithSuccess("It is one")
                    : Result.Fail<string>("Only one accepted")));

                // Assert
                result.IsSuccess.Should().BeTrue();
                result.Value.Should()
                    .Be("One");

                result.Successes.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("An int", "It is one");
            }

            [Fact]
            public void Bind_ToResultWhichIsSuccessful_ReturnsSuccessResult()
            {
                var valueResult = Result.Ok(1).WithSuccess("First number");

                // Act
                var result = valueResult.Bind(n => Result.OkIf(n == 1, "Irrelevant").WithSuccess("It is one"));

                // Assert
                result.IsSuccess.Should().BeTrue();
                result.Successes.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("First number", "It is one");
            }

            [Fact]
            public async Task Bind_ToResultWhichIsSuccessful_ReturnsSuccessResultTask()
            {
                var valueResult = Result.Ok(1).WithSuccess("First number");

                // Act
                var result = await valueResult.Bind(n => Task.FromResult(Result.OkIf(n == 1, "Irrelevant").WithSuccess("It is one")));

                // Assert
                result.IsSuccess.Should().BeTrue();
                result.Successes.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("First number", "It is one");
            }

            [Fact]
            public async Task Bind_ToResultWhichIsSuccessful_ReturnsSuccessResultValueTask()
            {
                var valueResult = Result.Ok(1).WithSuccess("First number");

                // Act
                var result = await valueResult.Bind(n => new ValueTask<Result<string>>(Result.OkIf(n == 1, "Irrelevant").WithSuccess("It is one")));

                // Assert
                result.IsSuccess.Should().BeTrue();
                result.Successes.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("First number", "It is one");
            }

            [Fact]
            public void Bind_ToAnotherValueTypeWhichFailedTransformation_ReturnsFailedResult()
            {
                var valueResult = Result.Ok(5);

                // Act
                var result = valueResult.Bind(n => Result.Fail<string>("Only one accepted"));

                // Assert
                result.IsFailed.Should().BeTrue();

                result.Errors.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("Only one accepted");
            }

            [Fact]
            public async Task Bind_ToAnotherValueTypeWhichFailedTransformation_ReturnsFailedResultTask()
            {
                var valueResult = Result.Ok(5);

                // Act
                var result = await valueResult.Bind(n => Task.FromResult(Result.Fail<string>("Only one accepted")));

                // Assert
                result.IsFailed.Should().BeTrue();

                result.Errors.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("Only one accepted");
            }

            [Fact]
            public async Task Bind_ToAnotherValueTypeWhichFailedTransformation_ReturnsFailedResultValueTask()
            {
                var valueResult = Result.Ok(5);

                // Act
                var result = await valueResult.Bind(n => new ValueTask<Result<string>>(Result.Fail<string>("Only one accepted")));

                // Assert
                result.IsFailed.Should().BeTrue();

                result.Errors.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("Only one accepted");
            }

            [Fact]
            public void Bind_ToResultWhichFailedTransformation_ReturnsFailedResult()
            {
                var valueResult = Result.Ok(5);

                // Act
                var result = valueResult.Bind(n => Result.Fail("Only one accepted"));

                // Assert
                result.IsFailed.Should().BeTrue();

                result.Errors.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("Only one accepted");
            }

            [Fact]
            public async Task Bind_ToResultWhichFailedTransformation_ReturnsFailedResultTask()
            {
                var valueResult = Result.Ok(5);

                // Act
                var result = await valueResult.Bind(n => Task.FromResult(Result.Fail("Only one accepted")));

                // Assert
                result.IsFailed.Should().BeTrue();

                result.Errors.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("Only one accepted");
            }

            [Fact]
            public async Task Bind_ToResultWhichFailedTransformation_ReturnsFailedResultValueTask()
            {
                var valueResult = Result.Ok(5);

                // Act
                var result = await valueResult.Bind(n => new ValueTask<Result<string>>(Result.Fail("Only one accepted")));

                // Assert
                result.IsFailed.Should().BeTrue();

                result.Errors.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("Only one accepted");
            }
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

            var result = Result.Try(Action, _ => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }

        [Fact]
        public async Task Try_execute_successfully_task_async_action_return_success_result()
        {
            Task<int> Action() => Task.FromResult(5);
            var result = await Result.Try(Action);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(5);
        }

        [Fact]
        public async Task Try_execute_successfully_valuetask_async_action_return_success_result()
        {
            ValueTask<int> Action() => new ValueTask<int>(5);
            var result = await Result.Try(Action);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(5);
        }

        [Fact]
        public async Task Try_execute_failed_task_async_action_return_failed_result()
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
        public async Task Try_execute_failed_valuetask_async_action_return_failed_result()
        {
            var exception = new Exception("ex message");
            ValueTask<int> Action() => throw exception;

            var result = await Result.Try(Action);

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().HaveCount(1);

            var error = (ExceptionalError)result.Errors.First();
            error.Message.Should().Be(exception.Message);
            error.Exception.Should().Be(exception);
        }

        [Fact]
        public async Task Try_execute_failed_task_async_action_with_custom_catchHandler_return_failed_result()
        {
            var exception = new Exception("ex message");
            Task<int> Action() => throw exception;

            var result = await Result.Try(Action, _ => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }

        [Fact]
        public async Task Try_execute_failed_valuetask_async_action_with_custom_catchHandler_return_failed_result()
        {
            var exception = new Exception("ex message");
            ValueTask<int> Action() => throw exception;

            var result = await Result.Try(Action, _ => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }

        [Fact]
        public void Try_execute_failed_func_return_failed_result()
        {
            var error = new Error("xy");
            Result<int> Action() => Result.Fail<int>(error);

            var result = Result.Try(Action);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().Be(error);
        }

        [Fact]
        public async Task Try_execute_failed_func_async_return_failed_result()
        {
            var error = new Error("xy");
            Task<Result<int>> Action() => Task.FromResult(Result.Fail<int>(error));

            var result = await Result.Try(Action);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().Be(error);
        }

        [Fact]
        public async Task Try_execute_failed_valuetask_func_async_return_failed_result()
        {
            var error = new Error("xy");
            ValueTask<Result<int>> Action() => new ValueTask<Result<int>>(Result.Fail<int>(error));

            var result = await Result.Try(Action);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().Be(error);
        }

        [Fact]
        public void Try_execute_success_func_return_success_result()
        {
            Result<int> Action() => Result.Ok(5);

            var result = Result.Try(Action);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(5);
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Try_execute_success_func_async_return_success_result()
        {
            Task<Result<int>> Action() => Task.FromResult(Result.Ok(5));

            var result = await Result.Try(Action);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(5);
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Try_execute_success_valuetask_func_async_return_success_result()
        {
            ValueTask<Result<int>> Action() => new ValueTask<Result<int>>(Result.Ok(5));

            var result = await Result.Try(Action);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(5);
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Try_execute_withresult_failed_task_action_with_custom_catchHandler_return_failed_result()
        {
            var exception = new Exception("ex message");
            Result<int> Action() => throw exception;

            var result = Result.Try(Action, _ => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }

        [Fact]
        public async Task Try_execute_withresult_failed_task_async_action_with_custom_catchHandler_return_failed_result()
        {
            var exception = new Exception("ex message");
            Task<Result<int>> Action() => throw exception;

            var result = await Result.Try(Action, _ => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }

        [Fact]
        public async Task Try_execute_withresult_failed_valuetask_async_action_with_custom_catchHandler_return_failed_result()
        {
            var exception = new Exception("ex message");
            ValueTask<Result<int>> Action() => throw exception;

            var result = await Result.Try(Action, _ => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }

        [Fact]
        public void Implicit_conversion_T_is_converted_to_Success_result_of_T()
        {
            string value = "result";

            Result<string> result = value;

            result.IsSuccess.Should().BeTrue();
            result.IsFailed.Should().BeFalse();
            result.Reasons.Should().BeEmpty();
            result.Errors.Should().BeEmpty();

            result.Value.Should().Be(value);
            result.Value.Should().BeOfType<string>();

            result.ValueOrDefault.Should().Be(value);
            result.ValueOrDefault.Should().BeOfType<string>();
        }

        [Fact]
        public void Implicit_conversion_Null_is_converted_to_Success_result_of_Null()
        {
            Result<object> result = (object)null;

            result.IsSuccess.Should().BeTrue();
            result.IsFailed.Should().BeFalse();
            result.Reasons.Should().BeEmpty();
            result.Errors.Should().BeEmpty();

            result.Value.Should().BeNull();
            result.ValueOrDefault.Should().BeNull();
        }

        [Fact]
        public void Implicit_conversion_Result_Value_is_converted_to_Result_object()
        {
            Result<object> result = new Result<int>().WithValue(42);

            result.IsSuccess.Should().BeTrue();
            result.IsFailed.Should().BeFalse();
            result.Reasons.Should().BeEmpty();
            result.Errors.Should().BeEmpty();

            result.Value.Should().NotBeNull();
            result.ValueOrDefault.Should().NotBeNull();
            result.Value.Should().Be(42);
        }

        [Fact]
        public void Implicit_conversion_Result_Value_is_converted_to_Result_object_with_Reasons()
        {
            Result<object> result = new Result<int>().WithValue(42).WithReason(new SuccessTests.CustomSuccess());

            result.IsSuccess.Should().BeTrue();
            result.IsFailed.Should().BeFalse();
            result.Errors.Should().BeEmpty();

            result.Value.Should().NotBeNull();
            result.ValueOrDefault.Should().NotBeNull();
            result.Value.Should().Be(42);

            result.Reasons.Should().ContainSingle();
            result.Reasons.Should().AllBeEquivalentTo(new SuccessTests.CustomSuccess());
        }

        [Fact]
        public void Implicit_conversion_Result_Value_is_converted_to_Result_object_with_Errors()
        {
            Result<object> result = new Result<int>().WithValue(42).WithError("foo");

            result.IsSuccess.Should().BeFalse();
            result.IsFailed.Should().BeTrue();

            result.Reasons.Should().ContainSingle();
            result.Reasons.Should().AllBeEquivalentTo(new Error("foo"));

            result.Errors.Should().ContainSingle();
            result.Errors.Should().AllBeEquivalentTo(new Error("foo"));
        }

        [Fact]
        public void Can_deconstruct_generic_Ok_to_isSuccess_and_isFailed()
        {
            var (isSuccess, isFailed) = Result.Ok(true);

            isSuccess.Should().Be(true);
            isFailed.Should().Be(false);
        }

        [Fact]
        public void Can_deconstruct_generic_Fail_to_isSuccess_and_isFailed()
        {
            var (isSuccess, isFailed) = Result.Fail<bool>("fail");

            isSuccess.Should().Be(false);
            isFailed.Should().Be(true);
        }

        [Fact]
        public void Can_deconstruct_generic_Ok_to_isSuccess_and_isFailed_and_value()
        {
            var (isSuccess, isFailed, value) = Result.Ok(100);

            isSuccess.Should().Be(true);
            isFailed.Should().Be(false);
            value.Should().Be(100);
        }

        [Fact]
        public void Can_deconstruct_generic_Fail_to_isSuccess_and_isFailed_and_value()
        {
            var (isSuccess, isFailed, value) = Result.Fail<int>("fail");

            isSuccess.Should().Be(false);
            isFailed.Should().Be(true);
            value.Should().Be(default);
        }

        [Fact]
        public void Can_deconstruct_generic_Ok_to_isSuccess_and_isFailed_and_value_with_errors()
        {
            var (isSuccess, isFailed, value, errors) = Result.Ok(100);

            isSuccess.Should().Be(true);
            isFailed.Should().Be(false);
            value.Should().Be(100);
            errors.Should().BeNull();
        }

        [Fact]
        public void Can_deconstruct_generic_Fail_to_isSuccess_and_isFailed_and_errors_with_value()
        {
            var error = new Error("fail");

            var (isSuccess, isFailed, value, errors) = Result.Fail<bool>(error);

            isSuccess.Should().Be(false);
            isFailed.Should().Be(true);
            value.Should().Be(default);

            errors.Count.Should().Be(1);
            errors.FirstOrDefault().Should().Be(error);
        }

        [Fact]
        public void Dynamic_value_is_implicit_converted_to_ValueResult()
        {
            var result = DynamicConvert("hello", typeof(string));

            ((object)result.Value).Should().Be("hello");
            ((object)result.Value).Should().BeOfType(typeof(string));
        }

        private static Result<dynamic> DynamicConvert(dynamic source, Type dest)
        {
            var result = new Result<dynamic>();
            var converted = Convert.ChangeType(source, dest);
            var x = result.WithValue(converted);
            return x;
        }
    }
}