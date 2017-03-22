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
            Results.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Results.Ok()
                .Log();

            // Assert
            logger.LoggedContext.Should().Be(string.Empty);
            logger.LoggedResult.Should().NotBeNull();
        }
    }
}
