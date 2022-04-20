using FluentAssertions;
using Xunit;

namespace FluentResults.Test
{
    public class CheckReasonsTests
    {
        class CustomException : System.Exception
        {
            public int Id { get; }

            public CustomException(int id) : base("Custom exception")
            {
                Id = id;
            }
        }

        class NotFoundError : Error
        {
            public int Id { get; }

            public NotFoundError(int id) : base("Not Found")
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

        [Fact]
        public void HasException_WithSearchedError()
        {
            var result = Result.Fail(new NotFoundError(3).CausedBy(new CustomException(1)));

            result.HasException<CustomException>().Should().BeTrue();
        }

        [Fact]
        public void HasExceptionWithPredicate_WithSearchedError()
        {
            var result = Result.Fail(new NotFoundError(3).CausedBy(new CustomException(1)));

            result.HasException<CustomException>(e => e.Id == 1).Should().BeTrue();
        }

        [Fact]
        public void HasException_WithoutSearchedError()
        {
            var result = Result.Ok();

            result.HasException<CustomException>().Should().BeFalse();
        }

        [Fact]
        public void HasExceptionInNestedError_WithoutSearchedError()
        {
            var result = Result.Ok()
                .WithError(new Error("Main Error")
                    .CausedBy(new CustomException(1)));

            result.HasException<CustomException>().Should().BeTrue();
        }

        [Fact]
        public void HasExceptionInVeryDeepNestedError_WithoutSearchedError()
        {
            var result = Result.Ok()
                .WithError(new Error("Main Error")
                    .CausedBy(new Error("Another Error")
                        .CausedBy(new Error("Root Error"))
                        .CausedBy(new NotFoundError(2))
                        .CausedBy(new CustomException(1))
                    )
                );

            result.HasException<CustomException>().Should().BeTrue();
        }

        [Fact]
        public void HasExceptionInVeryDeepNestedErrorWithPredicate_WithoutSearchedError()
        {
            var result = Result.Ok()
                .WithError(new Error("Main Error")
                    .CausedBy(new Error("Another Error")
                        .CausedBy(new Error("Root Error"))
                        .CausedBy(new NotFoundError(2))
                        .CausedBy(new CustomException(1))
                    )
                );

            result.HasException<CustomException>(e => e.Id == 1).Should().BeTrue();
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
        public void HasErrorInNestedErrorWithPredicate_WithoutSearchedError()
        {
            var originalResult = Result.Fail(new NotFoundError(1));
            var result = Result.Fail(new NotFoundError(2)
                    .CausedBy(originalResult.Errors));

            result.HasError<NotFoundError>(e => e.Id == 1).Should().BeTrue();
            result.HasError<NotFoundError>(e => e.Id == 2).Should().BeTrue();
        }

        [Fact]
        public void HasErrorWithMetadataKey_WithSearchedError()
        {
            var result = Result.Fail(new Error("").WithMetadata("MetadataKey1", "MetadataValue1"));

            result.HasError(e => e.HasMetadataKey("MetadataKey1")).Should().BeTrue();
        }

        [Fact]
        public void HasErrorWithMetadataValueWithPredicate_WithSearchedError()
        {
            var result = Result.Fail(new Error("").WithMetadata("MetadataKey1", "MetadataValue1"));

            result.HasError(e => e.HasMetadata("MetadataKey1", metadataValue => (string) metadataValue == "MetadataValue1")).Should().BeTrue();
        }

        [Fact]
        public void HasErrorWithNoMetadataValueWithPredicate_WithSearchedError()
        {
            var result = Result.Fail(new Error(""));

            result.HasError(e => e.HasMetadata("MetadataKey1", metadataValue => (string) metadataValue == "MetadataValue1")).Should().BeFalse();
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