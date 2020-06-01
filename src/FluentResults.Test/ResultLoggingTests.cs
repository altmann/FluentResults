using FluentAssertions;
using FluentResults.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentResults.Test
{
    [TestClass]
    public class ResultLoggingTests
    {
        [TestMethod]
        public void LogOkResult_EmptySuccess()
        {
            // Arrange
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            new Result()
                .Log();

            // Assert
            logger.LoggedContext.Should().Be(string.Empty);
            logger.LoggedResult.Should().NotBeNull();
        }
    }
}
