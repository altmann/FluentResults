using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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

            okResult.Successes.Should().HaveCount(1);
            okResult.Successes.First().Should().BeOfType<Success>();
            okResult.Successes.First().Message.Should().Be("First success message");

            okResult.Errors.Should().BeEmpty();
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

            okResult.Errors.Should().BeEmpty();
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
        public void CreateFailedResultWithListOfErrors_FailedResultWithErrors()
        {
            // Act
            var errors = new List<string> { "First error message", "Second error message" };
            var result = Result.Fail(errors);

            // Assert
            result.Reasons.Should().HaveCount(2);
            result.Reasons[0].Should().BeOfType<Error>();
            result.Reasons[1].Should().BeOfType<Error>();
            result.Reasons[0].Message.Should().Be("First error message");
            result.Reasons[1].Message.Should().Be("Second error message");
        }

        [Fact]
        public void Fail_WithNullEnumerableOfErrorMessages_ShouldThrow()
        {
            // Act
            Action act = () => Result.Fail((IEnumerable<string>)null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Fail_WithEmptyEnumerableOfErrorMessages_ShouldThrow()
        {
            // Act
            Action act = () => Result.Fail(Enumerable.Empty<string>());

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Fail_WithValidErrors_ShouldReturnFailedResult()
        {
            // Act
            var errors = new List<IError> { new Error("First error message"), new Error("Second error message") };
            var result = Result.Fail(errors);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Reasons.Should().HaveCount(2);
            result.Reasons[0].Should().BeOfType<Error>();
            result.Reasons[1].Should().BeOfType<Error>();
            result.Reasons[0].Message.Should().Be("First error message");
            result.Reasons[1].Message.Should().Be("Second error message");
        }

        [Fact]
        public void Fail_WithNullEnumerableOfErrors_ShouldThrow()
        {
            // Act
            Action act = () => Result.Fail((IEnumerable<IError>)null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Fail_WithEmptyEnumerableOfErrors_ShouldThrow()
        {
            // Act
            Action act = () => Result.Fail(Enumerable.Empty<IError>());

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ToResult_WithOkResultAndValue_ReturnSuccessResult()
        {
            var valueResult = Result.Ok();

            // Act
            var result = valueResult.ToResult(2.5f);

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
        public void FailIf_FailedConditionIsFalseAndWithObjectErrorFactory_CreateSuccessResult()
        {
            var result = Result.FailIf(false, LazyError);

            result.IsFailed.Should().BeFalse();

            Error LazyError()
            {
                throw new Exception("This should not be thrown!");
            }
        }

        [Fact]
        public void FailIf_FailedConditionIsFalseAndWithStringErrorMessageFactory_CreateSuccessResult()
        {
            var result = Result.FailIf(false, LazyError);

            result.IsFailed.Should().BeFalse();

            string LazyError()
            {
                throw new Exception("This should not be thrown!");
            }
        }

        [Fact]
        public void FailIf_FailedConditionIsTrueAndWithObjectErrorFactory_CreateFailedResult()
        {
            var result = Result.FailIf(true, () => "Error message");

            result.IsFailed.Should().BeTrue();
            result.Errors.Single().Message.Should().Be("Error message");
        }

        [Fact]
        public void FailIf_FailedConditionIsTrueAndWithStringErrorMessageFactory_CreateFailedResult()
        {
            var result = Result.FailIf(true, () => new Error("Error message"));

            result.IsFailed.Should().BeTrue();
            result.Errors.Single().Message.Should().Be("Error message");
        }

        [Fact]
        public void FailIf_WithErrors_IsFailure()
        {
            // Arrange
            var errorMessage = "Sample Error";
            var errors = new List<IError> { new Error(errorMessage) };

            // Act
            var result = Result.FailIf(true, errors);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Single().Message.Should().Be(errorMessage);
        }

        [Fact]
        public void FailIf_WithNoErrors_IsSuccess()
        {
            // Arrange
            var errors = new List<IError>();

            // Act
            var result = Result.FailIf(false, errors);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void FailIfNotEmpty_WithErrors_IsFailure()
        {
            // Arrange
            var errorMessage = "Sample Error";
            var errors = new List<IError> { new Error(errorMessage) };

            // Act
            var result = Result.FailIfNotEmpty(errors);

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Single().Message.Should().Be(errorMessage);
        }

        [Fact]
        public void FailIfNotEmpty_WithNoErrors_IsSuccess()
        {
            // Arrange
            var errors = new List<IError>();

            // Act
            var result = Result.FailIfNotEmpty(errors);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeEmpty();
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
        public void OkIf_SuccessConditionIsTrueAndWithStringErrorMessageFactory_CreateSuccessResult()
        {
            var result = Result.OkIf(true, LazyError);

            result.IsSuccess.Should().BeTrue();

            string LazyError()
            {
                throw new Exception("This should not be thrown!");
            }
        }

        [Fact]
        public void OkIf_SuccessConditionIsTrueAnWithObjectErrorMessageFactory_CreateSuccessResult()
        {
            var result = Result.OkIf(true, LazyError);

            result.IsSuccess.Should().BeTrue();

            Error LazyError()
            {
                throw new Exception("This should not be thrown!");
            }
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
        public void OkIf_SuccessConditionIsFalseAndWithStringErrorMessageFactory_CreateFailedResult()
        {
            const string errorMessage = "Error message";
            var result = Result.OkIf(false, () => errorMessage);

            result.IsFailed.Should().BeTrue();
            result.Errors.Single().Message.Should().Be(errorMessage);
        }

        [Fact]
        public void OkIf_SuccessConditionIsFalseAndWithObjectErrorMessageFactory_CreateFailedResult()
        {
            const string errorMessage = "Error message";
            var result = Result.OkIf(false, () => new Error(errorMessage));

            result.IsFailed.Should().BeTrue();
            result.Errors.Single().Message.Should().Be(errorMessage);
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

            void Action()
            {
                throw exception;
            }

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

            void Action()
            {
                throw exception;
            }

            var result = Result.Try(Action, e => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }

        [Fact]
        public async Task Try_execute_successfully_task_async_action_return_success_result()
        {
            Task Action()
            {
                return Task.FromResult(0);
            }

            var result = await Result.Try(Action);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Try_execute_successfully_valuetask_async_action_return_success_result()
        {
            ValueTask<int> Action()
            {
                return new ValueTask<int>(0);
            }

            var result = await Result.Try(Action);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Try_execute_failed_task_async_action_return_failed_result()
        {
            var exception = new Exception("ex message");

            Task Action()
            {
                throw exception;
            }

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

            ValueTask Action()
            {
                throw exception;
            }

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

            Task Action()
            {
                throw exception;
            }

            var result = await Result.Try(Action, e => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }

        [Fact]
        public async Task Try_execute_failed_valuetask_async_action_with_custom_catchHandler_return_failed_result()
        {
            var exception = new Exception("ex message");

            ValueTask Action()
            {
                throw exception;
            }

            var result = await Result.Try(Action, e => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }

        [Fact]
        public void Try_execute_failed_func_return_failed_result()
        {
            var error = new Error("xy");

            Result Action()
            {
                return Result.Fail(error);
            }

            var result = Result.Try(Action);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().Be(error);
        }

        [Fact]
        public async Task Try_execute_failed_func_async_return_failed_result()
        {
            var error = new Error("xy");

            Task<Result> Action()
            {
                return Task.FromResult(Result.Fail(error));
            }

            var result = await Result.Try(Action);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().Be(error);
        }

        [Fact]
        public async Task Try_execute_failed_valuetask_func_async_return_failed_result()
        {
            var error = new Error("xy");

            ValueTask<Result> Action()
            {
                return new(Result.Fail(error));
            }

            var result = await Result.Try(Action);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.First().Should().Be(error);
        }

        [Fact]
        public void Try_execute_success_func_return_success_result()
        {
            Result Action()
            {
                return Result.Ok();
            }

            var result = Result.Try(Action);

            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Try_execute_success_func_async_return_success_result()
        {
            Task<Result> Action()
            {
                return Task.FromResult(Result.Ok());
            }

            var result = await Result.Try(Action);

            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Try_execute_success_valuetask_func_async_return_success_result()
        {
            ValueTask<Result> Action()
            {
                return new(Result.Ok());
            }

            var result = await Result.Try(Action);

            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Try_execute_withresult_failed_task_action_with_custom_catchHandler_return_failed_result()
        {
            var exception = new Exception("ex message");

            Result Action()
            {
                throw exception;
            }

            var result = Result.Try(Action, _ => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }

        [Fact]
        public async Task
            Try_execute_withresult_failed_task_async_action_with_custom_catchHandler_return_failed_result()
        {
            var exception = new Exception("ex message");

            Task<Result> Action()
            {
                throw exception;
            }

            var result = await Result.Try(Action, _ => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }

        [Fact]
        public async Task
            Try_execute_withresult_failed_valuetask_async_action_with_custom_catchHandler_return_failed_result()
        {
            var exception = new Exception("ex message");

            ValueTask<Result> Action()
            {
                throw exception;
            }

            var result = await Result.Try(Action, _ => new Error("xy"));

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);

            var error = result.Errors.First();
            error.Message.Should().Be("xy");
        }

        [Fact]
        public void Can_deconstruct_non_generic_Ok_to_isSuccess_and_isFailed()
        {
            var (isSuccess, isFailed) = Result.Ok();

            isSuccess.Should().Be(true);
            isFailed.Should().Be(false);
        }

        [Fact]
        public void Can_deconstruct_non_generic_Fail_to_isSuccess_and_isFailed()
        {
            var (isSuccess, isFailed) = Result.Fail("fail");

            isSuccess.Should().Be(false);
            isFailed.Should().Be(true);
        }

        [Fact]
        public void Can_deconstruct_non_generic_Ok_to_isSuccess_and_isFailed_and_errors()
        {
            var (isSuccess, isFailed, errors) = Result.Ok();

            isSuccess.Should().Be(true);
            isFailed.Should().Be(false);
            errors.Should().BeNull();
        }

        [Fact]
        public void Can_deconstruct_non_generic_Fail_to_isSuccess_and_isFailed_and_errors()
        {
            var error = new Error("fail");
            var (isSuccess, isFailed, errors) = Result.Fail(error);

            isSuccess.Should().Be(false);
            isFailed.Should().Be(true);

            errors.Count.Should().Be(1);
            errors.FirstOrDefault().Should().Be(error);
        }

        public class BindMethod
        {
            [Fact]
            public void Bind_ToAnotherValueTypeWithFailedResult_ReturnFailedResult()
            {
                var valueResult = Result.Fail("First error message");

                // Act
                var result = valueResult.Bind(() => Result.Ok(1));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("First error message");
            }

            [Fact]
            public async Task Bind_ToAnotherValueTypeWithFailedResult_ReturnFailedResultTask()
            {
                var valueResult = Result.Fail("First error message");

                // Act
                var result = await valueResult.Bind(() => Task.FromResult(Result.Ok(1)));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("First error message");
            }

            [Fact]
            public async Task Bind_ToAnotherValueTypeWithFailedResult_ReturnFailedResultValueTask()
            {
                var valueResult = Result.Fail("First error message");

                // Act
                var result = await valueResult.Bind(() => new ValueTask<Result<int>>(Result.Ok(1)));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("First error message");
            }

            [Fact]
            public void Bind_ToResultWithFailedResult_ReturnFailedResult()
            {
                var valueResult = Result.Fail("First error message");

                // Act
                var result = valueResult.Bind(Result.Ok);

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("First error message");
            }

            [Fact]
            public async Task Bind_ToResultWithFailedResult_ReturnFailedResultTask()
            {
                var valueResult = Result.Fail("First error message");

                // Act
                var result = await valueResult.Bind(() => Task.FromResult(Result.Ok()));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("First error message");
            }

            [Fact]
            public async Task Bind_ToResultWithFailedResult_ReturnFailedResultValueTask()
            {
                var valueResult = Result.Fail("First error message");

                // Act
                var result = await valueResult.Bind(() => new ValueTask<Result>(Result.Ok()));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("First error message");
            }

            [Fact]
            public void Bind_ToAnotherValueTypeWithFailedResultAndFailedTransformation_ReturnFailedResult()
            {
                var valueResult = Result.Fail("Original error message");

                // Act
                var result = valueResult.Bind(() => Result.Fail<string>("Irrelevant error"));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("Original error message");
            }

            [Fact]
            public async Task Bind_ToAnotherValueTypeWithFailedResultAndFailedTransformation_ReturnFailedResultTask()
            {
                var valueResult = Result.Fail("Original error message");

                // Act
                var result = await valueResult.Bind(() => Task.FromResult(Result.Fail<string>("Irrelevant error")));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("Original error message");
            }

            [Fact]
            public async Task
                Bind_ToAnotherValueTypeWithFailedResultAndFailedTransformation_ReturnFailedResultValueTask()
            {
                var valueResult = Result.Fail("Original error message");

                // Act
                var result = await valueResult.Bind(() => Task.FromResult(Result.Fail<string>("Irrelevant error")));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("Original error message");
            }

            [Fact]
            public void Bind_ToResultWithFailedResultAndFailedTransformation_ReturnFailedResult()
            {
                var valueResult = Result.Fail("Original error message");

                // Act
                var result = valueResult.Bind(() => Result.Fail("Irrelevant error"));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("Original error message");
            }

            [Fact]
            public async Task Bind_ToResultWithFailedResultAndFailedTransformation_ReturnFailedResultTask()
            {
                var valueResult = Result.Fail("Original error message");

                // Act
                var result = await valueResult.Bind(() => Task.FromResult(Result.Fail("Irrelevant error")));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("Original error message");
            }

            [Fact]
            public async Task Bind_ToResultWithFailedResultAndFailedTransformation_ReturnFailedResultValueTask()
            {
                var valueResult = Result.Fail("Original error message");

                // Act
                var result = await valueResult.Bind(() => new ValueTask<Result>(Result.Fail("Irrelevant error")));

                // Assert
                result.IsFailed.Should().BeTrue();
                result.Errors.Select(e => e.Message)
                    .Should()
                    .BeEquivalentTo("Original error message");
            }

            [Fact]
            public void Bind_ToAnotherValueTypeWhichIsSuccessful_ReturnsSuccessResult()
            {
                var valueResult = Result.Ok().WithSuccess("An int");

                // Act
                var result = valueResult.Bind(() => "One".ToResult().WithSuccess("It is one"));

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
                var valueResult = Result.Ok().WithSuccess("An int");

                // Act
                var result = await valueResult.Bind(() => Task.FromResult("One".ToResult().WithSuccess("It is one")));

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
                var valueResult = Result.Ok().WithSuccess("An int");

                // Act
                var result = await valueResult.Bind(() =>
                    new ValueTask<Result<string>>("One".ToResult().WithSuccess("It is one")));

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
                var valueResult = Result.Ok().WithSuccess("First number");

                // Act
                var result = valueResult.Bind(() => Result.Ok().WithSuccess("It is one"));

                // Assert
                result.IsSuccess.Should().BeTrue();
                result.Successes.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("First number", "It is one");
            }

            [Fact]
            public async Task Bind_ToResultWhichIsSuccessful_ReturnsSuccessResultTask()
            {
                var valueResult = Result.Ok().WithSuccess("First number");

                // Act
                var result = await valueResult.Bind(() => Task.FromResult(Result.Ok().WithSuccess("It is one")));

                // Assert
                result.IsSuccess.Should().BeTrue();
                result.Successes.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("First number", "It is one");
            }

            [Fact]
            public async Task Bind_ToResultWhichIsSuccessful_ReturnsSuccessResultValueTask()
            {
                var valueResult = Result.Ok().WithSuccess("First number");

                // Act
                var result = await valueResult.Bind(() =>
                    new ValueTask<Result<string>>(Result.Ok().WithSuccess("It is one")));

                // Assert
                result.IsSuccess.Should().BeTrue();
                result.Successes.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("First number", "It is one");
            }

            [Fact]
            public void Bind_ToAnotherValueTypeWhichFailedTransformation_ReturnsFailedResult()
            {
                var valueResult = Result.Ok();

                // Act
                var result = valueResult.Bind(() => Result.Fail<string>("Only one accepted"));

                // Assert
                result.IsFailed.Should().BeTrue();

                result.Errors.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("Only one accepted");
            }

            [Fact]
            public async Task Bind_ToAnotherValueTypeWhichFailedTransformation_ReturnsFailedResultTask()
            {
                var valueResult = Result.Ok();

                // Act
                var result = await valueResult.Bind(() => Task.FromResult(Result.Fail<string>("Only one accepted")));

                // Assert
                result.IsFailed.Should().BeTrue();

                result.Errors.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("Only one accepted");
            }

            [Fact]
            public async Task Bind_ToAnotherValueTypeWhichFailedTransformation_ReturnsFailedResultValueTask()
            {
                var valueResult = Result.Ok();

                // Act
                var result = await valueResult.Bind(() =>
                    new ValueTask<Result<string>>(Result.Fail<string>("Only one accepted")));

                // Assert
                result.IsFailed.Should().BeTrue();

                result.Errors.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("Only one accepted");
            }

            [Fact]
            public void Bind_ToResultWhichFailedTransformation_ReturnsFailedResult()
            {
                var valueResult = Result.Ok();

                // Act
                var result = valueResult.Bind(() => Result.Fail("Only one accepted"));

                // Assert
                result.IsFailed.Should().BeTrue();

                result.Errors.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("Only one accepted");
            }

            [Fact]
            public async Task Bind_ToResultWhichFailedTransformation_ReturnsFailedResultTask()
            {
                var valueResult = Result.Ok();

                // Act
                var result = await valueResult.Bind(() => Task.FromResult(Result.Fail("Only one accepted")));

                // Assert
                result.IsFailed.Should().BeTrue();

                result.Errors.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("Only one accepted");
            }

            [Fact]
            public async Task Bind_ToResultWhichFailedTransformation_ReturnsFailedResultValueTask()
            {
                var valueResult = Result.Ok();

                // Act
                var result =
                    await valueResult.Bind(() => new ValueTask<Result<string>>(Result.Fail("Only one accepted")));

                // Assert
                result.IsFailed.Should().BeTrue();

                result.Errors.Select(s => s.Message)
                    .Should()
                    .BeEquivalentTo("Only one accepted");
            }
        }
    }
}