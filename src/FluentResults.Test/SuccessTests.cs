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

#if NET5_0
    public record MyError(int id) : IError;

    [Fact]
        public void CreateSuccess_2()
        {
            // Act
            var r = Result.Fail(new MyError(1));
        }
#endif
    }
}
