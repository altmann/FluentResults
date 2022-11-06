using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FluentResults.Test
{
    public class ErrorTests
    {
        [Fact]
        public void CreateError_EmptyError()
        {
            // Act
            var error = new Error("");

            // Assert
            error.Reasons.Should().BeEmpty();
            error.Metadata.Keys.Should().BeEmpty();
        }

        [Fact]
        public void CreateErrorCausedByErrorObject_ErrorWithReason()
        {
            // Act
            var error = new Error("")
                .CausedBy(new Error("First error message"));

            // Assert
            error.Reasons.Should().HaveCount(1);
            error.Reasons.First().Message.Should().Be("First error message");
        }

        [Fact]
        public void CreateErrorCausedBy2ErrorObjects_ErrorWithReason()
        {
            // Act
            var error = new Error("")
                .CausedBy(new Error("First error message"))
                .CausedBy(new Error("Second error message"));

            // Assert
            error.Reasons.Should().HaveCount(2);
            error.Reasons[0].Message.Should().Be("First error message");
            error.Reasons[1].Message.Should().Be("Second error message");
        }

        [Fact]
        public void CreateErrorCausedByErrorMessage_ErrorWithReason()
        {
            // Act
            var error = new Error("")
                .CausedBy("First error message");

            // Assert
            error.Reasons.Should().HaveCount(1);
            error.Reasons.First().Message.Should().Be("First error message");
        }

        [Fact]
        public void CreateErrorCausedByException_ErrorWithReason()
        {
            // Act
            var error = new Error("")
                .CausedBy(new InvalidOperationException("Invalid Operation Exception"));

            // Assert
            error.Reasons.Should().HaveCount(1);
            error.Reasons.First().Should().BeOfType<ExceptionalError>();
            error.Reasons.First().Message.Should().Be("Invalid Operation Exception");
        }

        [Fact]
        public void CreateErrorCausedByMessageAndException_ErrorWithReason()
        {
            // Act
            var error = new Error("")
                .CausedBy("First error", new InvalidOperationException("Invalid Operation Exception"));

            // Assert
            error.Reasons.Should().HaveCount(1);
            error.Reasons.First().Should().BeOfType<ExceptionalError>();
            error.Reasons.First().Message.Should().Be("First error");
        }

        [Fact]
        public void CreateErrorWithMetadata_ErrorWithMetadata()
        {
            // Act
            var error = new Error("")
                .WithMetadata("Field", "CustomerName");

            // Assert
            error.Metadata.Should().HaveCount(1);
            error.Metadata.Keys.First().Should().Be("Field");
            error.Metadata.Values.First().Should().Be("CustomerName");
        }

        [Fact]
        public void CreateErrorWithMultipleMetadata_ErrorWithMultipleMetadata()
        {
            // Act
            var error = new Error("")
                .WithMetadata("Field", "CustomerName")
                .WithMetadata("ErrorCode", "1.1");

            // Assert
            error.Metadata.Should().HaveCount(2);
            error.Metadata.Keys.First().Should().Be("Field");
            error.Metadata.Keys.Skip(1).Take(1).First().Should().Be("ErrorCode");
        }

        [Fact]
        public void CreateCustomErrorWithNoMessage_CustomErrorWithMessage()
        {
            // Act
            var error = new CustomError();

            // Assert
            error.Message.Should().BeNull();
            error.Reasons.Should().BeEmpty();
            error.Metadata.Keys.Should().BeEmpty();
        }

        public class CustomError : Error
        {
            
        }

        [Fact]
        public void CreateError_FromErrorImplicitConversion()
        {
            var error = new Error("")
                .CausedBy("First error message");

            Result result = error;

            result.IsFailed.Should().Be(true);
            result.Reasons.Should().HaveCount(1);
            error.Reasons.First().Message.Should().Be("First error message");
        }

        [Fact]
        public void CreateErrors_FromListOfErrorsImplicitConversion()
        {
            var errors = new List<Error>
            {
                new Error("First error message"),
                new Error("Second error message"),
            };

            Result result = errors;

            result.IsFailed.Should().Be(true);
            result.Reasons.Should().HaveCount(2);
            result.Reasons[0].Message.Should().Be("First error message");
            result.Reasons[1].Message.Should().Be("Second error message");
        }
    }
}