using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace FluentResults.Test
{
    public class MyCustomResult : ValueResultBase<MyCustomResult, int>
    {
        public int MyNumber { get; set; }
    }

    [TestClass]
    public class CustomResultWithValueTests
    {
        [TestMethod]
        public void CreateOkResult_SuccessResult()
        {
            // Act
            var okResult = Results<MyCustomResult>.Ok();

            // Assert
            okResult.IsFailed.Should().BeFalse();
            okResult.IsSuccess.Should().BeTrue();

            okResult.Reasons.Should().BeEmpty();
            okResult.Errors.Should().BeEmpty();
            okResult.Successes.Should().BeEmpty();
            okResult.Value.Should().Be(0);
        }

        [TestMethod]
        public void CreateOkResultWithValue_SuccessResultWithValue()
        {
            // Act
            var okResult = Results<MyCustomResult>.Ok()
                .WithValue(5);

            // Assert
            okResult.Value.Should().Be(5);
        }
        
        [TestMethod]
        public void CreateFailedResultWithNoExplicitValue_FailedResult()
        {
            // Act
            var result = Results<MyCustomResult>.Fail("First error message");

            // Assert
            result.Value.Should().Be(0);
        }

        [TestMethod]
        public void CreateFailedResultWithExplicitValue_FailedResultWithValue()
        {
            // Act
            var result = Results.Fail<int>("First error message")
                .WithValue(5);

            // Assert
            result.Value.Should().Be(5);
        }
    }
}