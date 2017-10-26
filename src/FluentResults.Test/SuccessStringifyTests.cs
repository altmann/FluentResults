using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentResults.Test
{
    [TestClass]
    public class SuccessStringifyTests
    {
        [TestMethod]
        public void EmptySuccessToString_OnlyType()
        {
            // Act
            var error = new Success("My first success");

            // Assert
            error.ToString().Should().Be("Success with Message='My first success'");
        }
    }
}
