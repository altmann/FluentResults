using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace FluentResults.Test
{
    public class SuccessTests
    {
        [Fact]
        public void CreateSuccess_EmptySuccess()
        {
            // Act
            var success = new Success("My success message");

            // Assert
            success.Message.Should().Be("My success message");
            success.Metadata.Should().BeEmpty();
        }


        public record RecordError : IError
        {
            public RecordError()
            {
            }

            public RecordError(string message)
            {
                Message = message;
            }

            public string Message { get; init; }

            public Dictionary<string, object> Metadata { get; init; }

            public List<IError> Reasons { get; init; }
        }

        public record MyError(int Id, string Message) : RecordError(Message);
        public record MyError2(int Id) : RecordError;

        [Fact]
        public void CreateSuccess_2()
        {
            var e1 = new MyError(1, "see")
            {
                Metadata = new Dictionary<string, object>()
            };
            var e2 = new MyError(1, "see")
            {
                Metadata = new Dictionary<string, object>()
            };

            if (e1 == e2)
            {
                var x = 32;
            }

            // Act
            var r = Result.Fail(e1);

            var isFailed = r.IsFailed;
        }
    }
}