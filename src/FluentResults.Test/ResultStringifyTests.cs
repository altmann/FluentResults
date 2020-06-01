using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentResults.Test
{
    [TestClass]
    public class ResultStringifyTests
    {
        [TestMethod]
        public void OkResultWithoutValueToString_OkResult()
        {
            // Act
            var result = Result.Ok();

            // Assert
            result.ToString().Should().Be("Result: IsSuccess='True'");
        }

        [TestMethod]
        public void FailedResultWithoutValueToString_FailedResult()
        {
            // Act
            var result = Result.Fail("My error");

            // Assert
            result.ToString().Should().Be("Result: IsSuccess='False', Reasons='Error with Message='My error''");
        }

        [TestMethod]
        public void OkResultWithValueToString_OkResult()
        {
            // Act
            var result = Result.Ok<int>(default);

            // Assert
            result.ToString().Should().Be("Result: IsSuccess='True', Value='0'");
        }

        [TestMethod]
        public void FailedResultWithValueToString_FailedResult()
        {
            // Act
            var result = Result.Fail<int>("My error");

            // Assert
            result.ToString().Should().Be("Result: IsSuccess='False', Reasons='Error with Message='My error'', Value='0'");
        }
    }
}
