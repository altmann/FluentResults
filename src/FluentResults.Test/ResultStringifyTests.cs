using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentResults.Test
{
    [TestClass]
    public class ResultStringifyTests
    {
        [TestMethod]
        public void OkResultToString_OkResult()
        {
            // Act
            var result = Results.Ok();

            // Assert
            result.ToString().Should().Be("Result: IsSuccess='True'");
        }

        [TestMethod]
        public void FailedResultToString_FailedResult()
        {
            // Act
            var result = Results.Fail("My error");

            // Assert
            result.ToString().Should().Be("Result: IsSuccess='False', Reasons='Error with Message='My error''");
        }
    }
}
