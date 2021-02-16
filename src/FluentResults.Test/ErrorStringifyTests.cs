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
            var error = new Error();

            // Assert
            error.ToString().Should().Be("Error { Message =  }");
        }
        
        [Fact]
        public void ErrorWithMessageToString_TypeWithMessage()
        {
            // Act
            var error = new Error()
                .WithMessage("Error message");

            // Assert
            error.ToString().Should().Be("Error { Message = Error message }");
        }

        [Fact]
        public void ErrorWithReasonsToString_TypeWithReasons()
        {
            // Act
            var error = new Error()
                .CausedBy("My first cause")
                .CausedBy("My second cause");

            // Assert
            error.ToString().Should().Be("Error { Message = , Reasons = [ Error { Message = My first cause }, Error { Message = My second cause } ] }");
        }

        [Fact]
        public void ErrorWithMetadataToString_TypeWithMetadata()
        {
            // Act
            var error = new Error()
                .WithMetadata("Key1", 123)
                .WithMetadata("Key2", "Value");

            // Assert
            error.ToString().Should().Be("Error { Message = , Metadata = [ { Key1 = 123 }, { Key2 = Value } ] }");
        }
    }
}
