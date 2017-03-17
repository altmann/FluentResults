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

        [TestMethod]
        public void ErrorWithTagsToString_TypeWithMessage()
        {
            // Act
            var error = new Success("First success message")
                .WithTags("firstTag", "secondTag");

            // Assert
            error.ToString().Should().Be("Success with Message='First success message', Tags='firstTag; secondTag'");
        }
    }
}
