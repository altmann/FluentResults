using System;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace FluentResults.Extensions.FluentAssertions.Test.ResultTests
{
    public class ReasonSatisfyTests
    {

        [Fact]
        public void A_reason_with_the_expected_property_throw_no_exception()
        {
            var failedResult = Result.Fail(new SomeReason("Error 1")
                                           {
                                               Prop = "Prop1"
                                           });

            Action action = () => failedResult
                                  .Should()
                                  .BeFailure()
                                  .And.HaveReason("Error 1")
                                  .That.Satisfy<SomeReason>(r => r.Prop.Should().Be("Prop1"));

            action.Should().NotThrow();
        }

        [Fact]
        public void A_reason_with_another_property_throw_exception()
        {
            var failedResult = Result.Fail(new SomeReason("Error 1")
                                           {
                                               Prop = "Prop1"
                                           });

            Action action = () => failedResult
                                  .Should()
                                  .BeFailure()
                                  .And.HaveReason("Error 1")
                                  .That.Satisfy<SomeReason>(r => r.Prop.Should().Be("Prop2"));

            action.Should()
                  .Throw<XunitException>()
                  .WithMessage($"Expected r.Prop to be \"Prop2\", but \"Prop1\" differs near \"1\" (index 4).");
        }

        [Fact]
        public void A_reason_with_another_type_throw_exception()
        {
            var failedResult = Result.Fail(new SomeReason("Error 1")
                                           {
                                               Prop = "Prop1"
                                           });

            Action action = () => failedResult
                                  .Should()
                                  .BeFailure()
                                  .And.HaveReason("Error 1")
                                  .That.Satisfy<AnotherReason>(r => {});

            action.Should()
                  .Throw<XunitException>()
                  .WithMessage($"Reason should be of type 'FluentResults.Extensions.FluentAssertions.Test.AnotherReason', but is of type 'FluentResults.Extensions.FluentAssertions.Test.SomeReason'");
        }
    }
}