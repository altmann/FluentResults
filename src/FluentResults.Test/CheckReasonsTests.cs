using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentResults.Test
{
    [TestClass]
    public class CheckReasonsTests
    {
        class NotFoundError : Error
        {
            public int Id { get; }

            public NotFoundError(int id)
            {
                Id = id;
            }
        }

        class FoundSuccess : Success
        {
            public int Id { get; }

            public FoundSuccess(int id) : base("")
            {
                Id = id;
            }
        }

        [TestMethod]
        public void HasError_WithSearchedError()
        {
            var result = Result.Fail(new NotFoundError(3));

            result.HasError<NotFoundError>().Should().BeTrue();
        }

        [TestMethod]
        public void HasErrorWithPredicate_WithSearchedError()
        {
            var result = Result.Fail(new NotFoundError(3));

            result.HasError<NotFoundError>(e => e.Id == 3).Should().BeTrue();
        }

        [TestMethod]
        public void HasError_WithoutSearchedError()
        {
            var result = Result.Ok();

            result.HasError<NotFoundError>().Should().BeFalse();
        }

        [TestMethod]
        public void HasErrorInNestedError_WithoutSearchedError()
        {
            var result = Result.Ok()
                .WithError(new Error("Main Error")
                    .CausedBy(new NotFoundError(2)));

            result.HasError<NotFoundError>().Should().BeTrue();
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void HasErrorWithMetadataKey_WithSearchedError()
        {
            var result = Result.Fail(new Error().WithMetadata("MetadataKey1", "MetadataValue1"));

            result.HasError(e => e.HasMetadataKey("MetadataKey1")).Should().BeTrue();
        }

        [TestMethod]
        public void HasErrorWithMetadataValueWithPredicate_WithSearchedError()
        {
            var result = Result.Fail(new Error().WithMetadata("MetadataKey1", "MetadataValue1"));

            result.HasError(e => e.HasMetadata("MetadataKey1", metadataValue => (string)metadataValue == "MetadataValue1")).Should().BeTrue();
        }

        [TestMethod]
        public void HasErrorWithNoMetadataValueWithPredicate_WithSearchedError()
        {
            var result = Result.Fail(new Error());

            result.HasError(e => e.HasMetadata("MetadataKey1", metadataValue => (string)metadataValue == "MetadataValue1")).Should().BeFalse();
        }

        [TestMethod]
        public void HasSuccess_WithSearchedSuccess()
        {
            var result = Result.Ok()
                .WithSuccess(new FoundSuccess(3));

            result.HasSuccess<FoundSuccess>().Should().BeTrue();
        }

        [TestMethod]
        public void HasSuccessWithPredicate_WithSearchedSuccess()
        {
            var result = Result.Ok()
                .WithSuccess(new FoundSuccess(3));

            result.HasSuccess<FoundSuccess>(e => e.Id == 3).Should().BeTrue();
        }

        [TestMethod]
        public void HasSuccess_WithoutSearchedSuccess()
        {
            var result = Result.Fail("error");

            result.HasSuccess<FoundSuccess>().Should().BeFalse();
        }
    }
}