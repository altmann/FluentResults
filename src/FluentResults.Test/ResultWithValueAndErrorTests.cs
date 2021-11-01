using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluentResults.Test
{
    class NotFoundError : Error
    {
        public int Id { get; }

        public NotFoundError(int id) : base("Not Found")
        {
            Id = id;
        }
    }

    public class ResultWithValueAndErrorTests
    {
        [Fact]
        public void Ok_WithNoParams_ShouldReturnSuccessResult()
        {
            // Act
            var okResult = Result.Ok<int, NotFoundError>(default);

            // Assert
            okResult.Should().BeOfType<Result<int, NotFoundError>>();
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
            var okResult = Result.Ok<int, NotFoundError>(5);

            // Assert
            okResult.Should().BeOfType<Result<int, NotFoundError>>();
            okResult.IsSuccess.Should().BeTrue();
            okResult.Value.Should().Be(5);
            okResult.ValueOrDefault.Should().Be(5);
        }

        [Fact]
        public void WithValue_WithValidParam_ShouldReturnSuccessResult()
        {
            var okResult = Result.Ok<int, NotFoundError>(default);

            // Act
            okResult.WithValue(5);

            // Assert
            okResult.Should().BeOfType<Result<int, NotFoundError>>();
            okResult.Value.Should().Be(5);
            okResult.ValueOrDefault.Should().Be(5);
        }

        [Fact]
        public void Fail_WithValidError_ShouldReturnFailedResult()
        {
            // Act
            var result = Result.Fail<int,NotFoundError>(new NotFoundError(404));

            // Assert
            result.Should().BeOfType<Result<int, NotFoundError>>();
            result.IsFailed.Should().BeTrue();
            result.ValueOrDefault.Should().Be(0);
        }

        [Fact]
        public void Value_WithResultInFailedState_ShouldThrowException()
        {
            // Act
            var result = Result.Fail<int, NotFoundError>(new NotFoundError(404));

            // Assert

            Action action = () => { var v = result.Value; };

            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Result is in status failed. Value is not set.");
        }
    }
}
