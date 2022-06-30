using FluentAssertions;
using FluentResults.Test.Mocks;
using Microsoft.Extensions.Logging;
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
            logger.LoggedContent.Should().BeNullOrEmpty();
            logger.LoggedResult.Should().NotBeNull();
            logger.LoggedLevel.Should().Be(LogLevel.Information);
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
            logger.LoggedContent.Should().BeNullOrEmpty();
            logger.LoggedResult.Should().NotBeNull();
            logger.LoggedLevel.Should().Be(LogLevel.Information);
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
            logger.LoggedContent.Should().BeNullOrEmpty();
            logger.LoggedResult.Should().NotBeNull();
            logger.LoggedLevel.Should().Be(LogLevel.Information);
        }

        [Fact]
        public void LogOkResultWithContent_EmptySuccess()
        {
            // Arrange
            var context = "context";
            var content = "content";
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Result.Ok()
                .Log(context, content);

            // Assert
            logger.LoggedContext.Should().Be(context);
            logger.LoggedContent.Should().Be(content);
            logger.LoggedResult.Should().NotBeNull();
            logger.LoggedLevel.Should().Be(LogLevel.Information);
        }

        [Fact]
        public void LogOkResultWithContentAndTypedContext_EmptySuccess()
        {
            // Arrange
            var content = "content";
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Result.Ok()
                .Log<Result>(content);

            // Assert
            logger.LoggedContext.Should().Be(typeof(Result).ToString());
            logger.LoggedContent.Should().Be(content);
            logger.LoggedResult.Should().NotBeNull();
            logger.LoggedLevel.Should().Be(LogLevel.Information);
        }

        [Fact]
        public void LogOkResultLevel_EmptySuccess()
        {
            // Arrange
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Result.Ok()
                .Log(LogLevel.Critical);

            // Assert
            logger.LoggedContext.Should().BeNullOrEmpty();
            logger.LoggedContent.Should().BeNullOrEmpty();
            logger.LoggedResult.Should().NotBeNull();
            logger.LoggedLevel.Should().Be(LogLevel.Critical);
        }

        [Fact]
        public void LogOkResultWithContextAndLevel_EmptySuccess()
        {
            // Arrange
            var context = "context";
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Result.Ok()
                .Log(context, LogLevel.Critical);

            // Assert
            logger.LoggedContext.Should().Be(context);
            logger.LoggedContent.Should().BeNullOrEmpty();
            logger.LoggedResult.Should().NotBeNull();
            logger.LoggedLevel.Should().Be(LogLevel.Critical);
        }

        [Fact]
        public void LogOkResultWithTypedContextAndLevel_EmptySuccess()
        {
            // Arrange
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Result.Ok()
                .Log<Result>(LogLevel.Critical);

            // Assert
            logger.LoggedContext.Should().Be(typeof(Result).ToString());
            logger.LoggedContent.Should().BeNullOrEmpty();
            logger.LoggedResult.Should().NotBeNull();
            logger.LoggedLevel.Should().Be(LogLevel.Critical);
        }

        [Fact]
        public void LogOkResultWithContentAndLevel_EmptySuccess()
        {
            // Arrange
            var context = "context";
            var content = "content";
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Result.Ok()
                .Log(context, content, LogLevel.Critical);

            // Assert
            logger.LoggedContext.Should().Be(context);
            logger.LoggedContent.Should().Be(content);
            logger.LoggedResult.Should().NotBeNull();
            logger.LoggedLevel.Should().Be(LogLevel.Critical);
        }

        [Fact]
        public void LogOkResultWithContentAndTypedContextAndLevel_EmptySuccess()
        {
            // Arrange
            var content = "content";
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Result.Ok()
                .Log<Result>(content, LogLevel.Critical);

            // Assert
            logger.LoggedContext.Should().Be(typeof(Result).ToString());
            logger.LoggedContent.Should().Be(content);
            logger.LoggedResult.Should().NotBeNull();
            logger.LoggedLevel.Should().Be(LogLevel.Critical);
        }

        [Fact]
        public void OkResultLogWhenSuccess_EmptySuccess()
        {
            // Arrange
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Result.Ok()
                .LogIfSuccess();

            // Assert
            logger.LoggedContext.Should().BeNullOrEmpty();
            logger.LoggedContent.Should().BeNull();
            logger.LoggedResult.Should().NotBeNull();
        }

        [Fact]
        public void FailedResultLogWhenFailed_Empty()
        {
            // Arrange
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Result.Fail("")
                .LogIfFailed();

            // Assert
            logger.LoggedContext.Should().BeNullOrEmpty();
            logger.LoggedContent.Should().BeNull();
            logger.LoggedResult.Should().NotBeNull();
        }

        [Fact]
        public void FailedResultLogWhenSuccess_EmptySuccess()
        {
            // Arrange
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Result.Fail("")
                .LogIfSuccess();

            // Assert
            logger.LoggedContext.Should().BeNullOrEmpty();
            logger.LoggedContent.Should().BeNull();
            logger.LoggedResult.Should().BeNull();
        }

        [Fact]
        public void OkResultLogWhenFailed_Empty()
        {
            // Arrange
            var logger = new LoggingMock();
            Result.Setup(cfg => {
                cfg.Logger = logger;
            });

            // Act
            Result.Ok()
                .LogIfFailed();

            // Assert
            logger.LoggedContext.Should().BeNullOrEmpty();
            logger.LoggedContent.Should().BeNull();
            logger.LoggedResult.Should().BeNull();
        }

        [Fact]
        public void FailedResult_MapErrors()
        {
            var result = Result.Fail("Failure 1")
                .WithSuccess("Success 1");

            result = result.MapErrors(e => new CustomError("Prefix: " + e.Message));

            result.Should().BeOfType<Result>();

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Message.Should().Be("Prefix: Failure 1");

            result.Successes.Should().HaveCount(1);
            result.Successes[0].Message.Should().Be("Success 1");
        }

        [Fact]
        public void FailedValueResult_MapErrors()
        {
            var result = Result.Fail<int>("Failure 1")
                .WithSuccess("Success 1");

            result = result.MapErrors(e => new CustomError("Prefix: " + e.Message));

            result.Should().BeOfType<Result<int>>();

            result.Errors.Should().HaveCount(1);
            result.Errors[0].Message.Should().Be("Prefix: Failure 1");

            result.Successes.Should().HaveCount(1);
            result.Successes[0].Message.Should().Be("Success 1");
        }

        [Fact]
        public void SuccessValueResult_MapSuccesses()
        {
            var result = Result.Ok(5)
                .WithSuccess("Success 1");

            result = result.MapSuccesses(e => new Success("Prefix: " + e.Message));

            result.Should().BeOfType<Result<int>>();
            result.Successes.Should().HaveCount(1);
            result.Successes[0].Message.Should().Be("Prefix: Success 1");

            result.Errors.Should().BeEmpty();

            result.Value.Should().Be(5);
        }

        [Fact]
        public void SuccessResult_MapSuccesses()
        {
            var result = Result.Ok()
                .WithSuccess("Success 1");

            result = result.MapSuccesses(e => new Success("Prefix: " + e.Message));

            result.Should().BeOfType<Result>();

            result.Successes.Should().HaveCount(1);
            result.Successes[0].Message.Should().Be("Prefix: Success 1");

            result.Errors.Should().BeEmpty();
        }
    }
}