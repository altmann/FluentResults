﻿using FluentAssertions;
using Xunit;

namespace FluentResults.Test
{
    public class CheckReasonsTests
    {
        record NotFoundError(int Id) : Error
        {
        }

        record FoundSuccess(int Id) : Success
        {
        }

        [Fact]
        public void HasError_WithSearchedError()
        {
            var result = Result.Fail(new NotFoundError(3));

            result.HasError<NotFoundError>().Should().BeTrue();
        }

        [Fact]
        public void HasErrorWithPredicate_WithSearchedError()
        {
            var result = Result.Fail(new NotFoundError(3));

            result.HasError<NotFoundError>(e => e.Id == 3).Should().BeTrue();
        }

        [Fact]
        public void HasError_WithoutSearchedError()
        {
            var result = Result.Ok();

            result.HasError<NotFoundError>().Should().BeFalse();
        }

        [Fact]
        public void HasErrorInNestedError_WithoutSearchedError()
        {
            var result = Result.Ok()
                .WithError(new Error("Main Error")
                    .CausedBy(new NotFoundError(2)));

            result.HasError<NotFoundError>().Should().BeTrue();
        }

        [Fact]
        public void HasErrorInVeryDeepNestedError_WithoutSearchedError()
        {
            var result = Result.Ok()
                .WithError(new Error("Main Error")
                    .CausedBy(new Error("Another Error")
                        .CausedBy(new Error("Root Error"))
                        .CausedBy(new NotFoundError(2))
                    )
                );

            result.HasError<NotFoundError>().Should().BeTrue();
        }

        [Fact]
        public void HasErrorInVeryDeepNestedErrorWithPredicate_WithoutSearchedError()
        {
            var result = Result.Ok()
                .WithError(new Error("Main Error")
                    .CausedBy(new Error("Another Error")
                        .CausedBy(new Error("Root Error"))
                        .CausedBy(new NotFoundError(2))
                    )
                );

            result.HasError<NotFoundError>(e => e.Id == 2).Should().BeTrue();
        }

        [Fact]
        public void HasErrorWithMetadataKey_WithSearchedError()
        {
            var result = Result.Fail(new Error().WithMetadata("MetadataKey1", "MetadataValue1"));

            result.HasError(e => e.HasMetadataKey("MetadataKey1")).Should().BeTrue();
        }

        [Fact]
        public void HasErrorWithMetadataValueWithPredicate_WithSearchedError()
        {
            var result = Result.Fail(new Error().WithMetadata("MetadataKey1", "MetadataValue1"));

            result.HasError(e => e.HasMetadata("MetadataKey1", metadataValue => (string)metadataValue == "MetadataValue1")).Should().BeTrue();
        }

        [Fact]
        public void HasErrorWithNoMetadataValueWithPredicate_WithSearchedError()
        {
            var result = Result.Fail(new Error());

            result.HasError(e => e.HasMetadata("MetadataKey1", metadataValue => (string)metadataValue == "MetadataValue1")).Should().BeFalse();
        }

        [Fact]
        public void HasSuccess_WithSearchedSuccess()
        {
            var result = Result.Ok()
                .WithSuccess(new FoundSuccess(3));

            result.HasSuccess<FoundSuccess>().Should().BeTrue();
        }

        [Fact]
        public void HasSuccessWithPredicate_WithSearchedSuccess()
        {
            var result = Result.Ok()
                .WithSuccess(new FoundSuccess(3));

            result.HasSuccess<FoundSuccess>(e => e.Id == 3).Should().BeTrue();
        }

        [Fact]
        public void HasSuccess_WithoutSearchedSuccess()
        {
            var result = Result.Fail("error");

            result.HasSuccess<FoundSuccess>().Should().BeFalse();
        }
    }
}