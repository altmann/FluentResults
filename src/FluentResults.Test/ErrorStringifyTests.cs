using FluentAssertions;
using Xunit;

namespace FluentResults.Test
{
    public class ErrorStringifyTests
    {
        [Fact]
        public void EmptyErrorToString_OnlyType()
        {
            // Act
            var error = new Error("");

            // Assert
            error.ToString().Should().Be("Error");
        }
        
        [Fact]
        public void ErrorWithReasonsToString_TypeWithReasons()
        {
            // Act
            var error = new Error("")
                .CausedBy("My first cause")
                .CausedBy("My second cause");

            // Assert
            error.ToString().Should().Be("Error with Reasons='Error with Message='My first cause'; Error with Message='My second cause''");
        }
    }
}
