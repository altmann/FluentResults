using FluentAssertions;
using Xunit;

namespace FluentResults.Test
{
    public class SuccessTests
    {
        [Fact]
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
