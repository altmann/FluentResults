using FluentAssertions;
using Xunit;

namespace FluentResults.Test
{
    public class ResultWithoutValueTests
    {
        [Fact]
        public void OkResult_EmptyReasons()
        {
            // Act
            var okResult = Results.Ok();

            // Assert
            okResult.Reasons
                .Should().BeEmpty();
        }
    }
}
