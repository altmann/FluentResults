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

        [Fact]
        public void LogOkResultWithContext_EmptySuccess()
        {
            // Arrange
            var context = "context";
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Result.Ok()
                .Log(context);

            // Assert
            logger.LoggedContext.Should().Be(context);
            logger.LoggedResult.Should().NotBeNull();
        }

        [Fact]
        public void LogOkResultWithTypedContext_EmptySuccess()
        {
            // Arrange
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Result.Ok()
                .Log<Result>();

            // Assert
            logger.LoggedContext.Should().Be(typeof(Result).ToString());
            logger.LoggedResult.Should().NotBeNull();
        }
    }
}