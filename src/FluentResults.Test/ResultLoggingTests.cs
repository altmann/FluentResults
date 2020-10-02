using FluentAssertions;
using FluentResults.Test.Mocks;
using Xunit;

namespace FluentResults.Test
{
    public class ResultLoggingTests
    {
        [Fact]
        public void LogOkResult_EmptySuccess()
        {
            // Arrange
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Result.Ok()
                .Log();

            // Assert
            logger.LoggedContext.Should().Be(string.Empty);
            logger.LoggedResult.Should().NotBeNull();
        }
    }
}
