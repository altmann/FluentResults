using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;
using FluentResults.Test.Mocks;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FluentResults.Test
{
    public class ResultTValueExtensionsTests
    {
        private void AssertEqual<TValue>(IResult<TValue> actual, IResult<TValue> expected)
        {
            // Assert
            actual.IsFailed.Should().Be(expected.IsFailed);
            actual.IsSuccess.Should().Be(expected.IsSuccess);
            actual.ValueOrDefault.Should().Be(expected.ValueOrDefault);

            actual.Reasons.Should().BeEquivalentTo(expected.Reasons);
            actual.Errors.Should().BeEquivalentTo(expected.Errors);
            actual.Successes.Should().BeEquivalentTo(expected.Successes);
        }

        [Fact]
        public void Ok_MapReason_with_IError_NoChange()
        {
            // Act
            IResult<string> result = Result.Ok("FooBar");
            IResult<string> actual = result.MapReasons((IError error) => new Error("\"" + error.Message + "\""));

            // Assert
            AssertEqual(actual, result);
        }

        [Fact]
        public void Fail_MapReason_with_IError_ShouldChange()
        {
            // Act
            IResult<string> result = Result.Fail<string>("FooBar");
            IResult<string> actual = result.MapReasons((IError error) => new Error("\"" + error.Message + "\""));

            // Assert
            var expected = result;
            actual.IsFailed.Should().Be(expected.IsFailed);
            actual.IsSuccess.Should().Be(expected.IsSuccess);
            actual.ValueOrDefault.Should().Be(expected.ValueOrDefault);

            actual.Successes.Should().BeEquivalentTo(expected.Successes);
            actual.Reasons.Should().NotBeEquivalentTo(expected.Reasons);
            actual.Errors.Should().NotBeEquivalentTo(expected.Errors);

            actual.Errors.Count.Should().Be(1);
            actual.Errors[0].Message.Should().Be("\"FooBar\"");
        }

        [Fact]
        public void Ok_Bind_Success()
        {
            // Act
            IResult<string> result = Result.Ok("4815162342");
            IResult<long> actual = result.Bind(s => Result.Try(() => long.Parse(s)));

            // Assert
            actual.IsSuccess.Should().Be(true);
            actual.IsFailed.Should().Be(false);
            actual.Errors.Should().BeEmpty();
            actual.Reasons.Should().BeEmpty();
            actual.Successes.Should().BeEmpty();
            actual.ValueOrDefault.Should().Be(4815162342L);
        }

        [Fact]
        public void Ok_Bind_Failure()
        {
            // Act
            bool delegateHasBeenCalled = false;
            IResult<string> result = Result.Ok("4815162342").WithError("FooBar");
            IResult<long> actual = result.Bind(s =>
            {
                delegateHasBeenCalled = true;
                return Result.Try(() => long.Parse(s));
            });

            // Assert
            delegateHasBeenCalled.Should().Be(false, "Bind of failed result should be called");
            actual.IsSuccess.Should().Be(false);
            actual.IsFailed.Should().Be(true);
            actual.Errors.Should().NotBeEmpty();
            actual.Errors.Count.Should().Be(1);
            actual.Reasons.Should().NotBeEmpty();
            actual.Reasons.Count.Should().Be(1);
            actual.Successes.Should().BeEmpty();
            actual.ValueOrDefault.Should().Be(default);
        }
    }
}
