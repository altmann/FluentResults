using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentResults.Test
{
    [TestClass]
    public class SuccessTests
    {
        [TestMethod]
        public void CreateSuccess_EmptySuccess()
        {
            // Act
            var success = new Success("My success message");

            // Assert
            success.Message.Should().Be("My success message");
            success.Metadata.Should().BeEmpty();
        }
    }
}
