using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace FluentResults.Test
{
    [TestClass]
    public class SuccessTests
    {
        [TestMethod]
        public void CreateSuccess_EmptySuccess()
        {
            // Act
            var success = new Success("My success message");

            // Assert
            success.Message.Should().Be("My success message");
            success.Tags.Should().BeEmpty();
        }

        [TestMethod]
        public void CreateSuccessWithTag_SuccessWithTag()
        {
            // Act
            var success = new Success("")
                .WithTag("MyTag");

            // Assert
            success.Tags.Should().HaveCount(1);
            success.Tags[0].Should().Be("MyTag");
        }

        [TestMethod]
        public void CreateSuccessWithMultipleTags_SuccessWithMultipleTags()
        {
            // Act
            var success = new Success("")
                .WithTag("MyTag1")
                .WithTag("MyTag2");

            // Assert
            success.Tags.Should().HaveCount(2);
            success.Tags[0].Should().Be("MyTag1");
            success.Tags[1].Should().Be("MyTag2");
        }

        [TestMethod]
        public void CreateSuccessWith3Tags_SuccessWith3Tags()
        {
            // Act
            var success = new Success("")
                .WithTags("MyTag1", "MyTag2", "MyTag3");

            // Assert
            success.Tags.Should().HaveCount(3);
            success.Tags[0].Should().Be("MyTag1");
            success.Tags[1].Should().Be("MyTag2");
            success.Tags[2].Should().Be("MyTag3");
        }
    }
}
