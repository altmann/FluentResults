using FluentAssertions;
using Xunit;

namespace FluentResults.Test
{
    public class ResultStringifyTests
    {
        [Fact]
        public void OkResultWithoutValueToString_OkResult()
        {
            // Act
            var result = Result.Ok();

            // Assert
            result.ToString().Should().Be("Result { IsSuccess = True }");
        }

        [Fact]
        public void FailedResultWithoutValueToString_FailedResult()
        {
            // Act
            var result = Result.Fail("My error");

            // Assert
            result.ToString().Should().Be("Result { IsSuccess = False, Reasons = [ Error { Message = My error } ] }");
        }

        [Fact]
        public void OkResultWithValueToString_OkResult()
        {
            // Act
            var result = Result.Ok<int>(default);

            // Assert
            result.ToString().Should().Be("Result { IsSuccess = True, Value = 0 }");
        }

        [Fact]
        public void FailedResultWithValueToString_FailedResult()
        {
            // Act
            var result = Result.Fail<int>("My error");

            // Assert
            result.ToString().Should().Be("Result { IsSuccess = False, Reasons = [ Error { Message = My error } ] }");
        }
    }
}
