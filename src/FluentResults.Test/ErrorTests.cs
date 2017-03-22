using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace FluentResults.Test
{
    [TestClass]
    public class ErrorTests
    {
        [TestMethod]
        public void CreateError_EmptyError()
        {
            // Act
            var error = new Error();

            // Assert
            error.ErrorCode.Should().BeEmpty();
            error.Reasons.Should().BeEmpty();
            error.Tags.Should().BeEmpty();
        }

        [TestMethod]
        public void CreateErrorCausedByErrorObject_ErrorWithReason()
        {
            // Act
            var error = new Error()
                .CausedBy(new Error("First error message"));

            // Assert
            error.ErrorCode.Should().BeEmpty();
            error.Reasons.Should().HaveCount(1);
            error.Reasons.First().Message.Should().Be("First error message");
        }

        [TestMethod]
        public void CreateErrorCausedBy2ErrorObjects_ErrorWithReason()
        {
            // Act
            var error = new Error()
                .CausedBy(new Error("First error message"))
                .CausedBy(new Error("Second error message"));

            // Assert
            error.ErrorCode.Should().BeEmpty();
            error.Reasons.Should().HaveCount(2);
            error.Reasons[0].Message.Should().Be("First error message");
            error.Reasons[1].Message.Should().Be("Second error message");
        }

        [TestMethod]
        public void CreateErrorCausedByErrorMessage_ErrorWithReason()
        {
            // Act
            var error = new Error()
                .CausedBy("First error message");

            // Assert
            error.ErrorCode.Should().BeEmpty();
            error.Reasons.Should().HaveCount(1);
            error.Reasons.First().Message.Should().Be("First error message");
        }

        [TestMethod]
        public void CreateErrorCausedByException_ErrorWithReason()
        {
            // Act
            var error = new Error()
                .CausedBy(new InvalidOperationException("Invalid Operation Exception"));

            // Assert
            error.ErrorCode.Should().BeEmpty();
            error.Reasons.Should().HaveCount(1);
            error.Reasons.First().Should().BeOfType<ExceptionalError>();
            error.Reasons.First().Message.Should().Be("Invalid Operation Exception");
        }

        [TestMethod]
        public void CreateErrorCausedByMessageAndException_ErrorWithReason()
        {
            // Act
            var error = new Error()
                .CausedBy("First error", new InvalidOperationException("Invalid Operation Exception"));

            // Assert
            error.ErrorCode.Should().BeEmpty();
            error.Reasons.Should().HaveCount(1);
            error.Reasons.First().Should().BeOfType<ExceptionalError>();
            error.Reasons.First().Message.Should().Be("First error");
        }

        [TestMethod]
        public void CreateErrorWithTag_ErrorWithTag()
        {
            // Act
            var error = new Error()
                .WithTag("MyTag");

            // Assert
            error.ErrorCode.Should().BeEmpty();
            error.Tags.Should().HaveCount(1);
            error.Tags[0].Should().Be("MyTag");
        }

        [TestMethod]
        public void CreateErrorWithMultipleTags_ErrorWithMultipleTags()
        {
            // Act
            var error = new Error()
                .WithTag("MyTag1")
                .WithTag("MyTag2");

            // Assert
            error.ErrorCode.Should().BeEmpty();
            error.Tags.Should().HaveCount(2);
            error.Tags[0].Should().Be("MyTag1");
            error.Tags[1].Should().Be("MyTag2");
        }

        [TestMethod]
        public void CreateErrorWith3Tags_ErrorWith3Tags()
        {
            // Act
            var error = new Error()
                .WithTags("MyTag1", "MyTag2", "MyTag3");

            // Assert
            error.ErrorCode.Should().BeEmpty();
            error.Tags.Should().HaveCount(3);
            error.Tags[0].Should().Be("MyTag1");
            error.Tags[1].Should().Be("MyTag2");
            error.Tags[2].Should().Be("MyTag3");
        }
    }
}
