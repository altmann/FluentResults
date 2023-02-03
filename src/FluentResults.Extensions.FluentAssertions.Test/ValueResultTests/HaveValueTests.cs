using System;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace FluentResults.Extensions.FluentAssertions.Test.ValueResultTests
{
    public class HaveValueTests
    {
        [Fact]
        public void Asserting_the_correct_value_of_a_success_result_throw_no_exception()
        {
            var successResult = Result.Ok(1);

            Action action = () => successResult.Should().HaveValue(1);

            action.Should().NotThrow();
        }

        [Fact]
        public void Asserting_a_false_value_of_a_success_result_throw_a_exception()
        {
            var successResult = Result.Ok(1);

            Action action = () => successResult.Should().HaveValue(2);

            action.Should()
                .Throw<XunitException>()
                .WithMessage("Expected value is '2', but is '1'");
        }

        [Fact]
        public void A_failed_result_throw_no_exception()
        {
            var failedResult = Result.Fail<int>("Error 1");

            Action action = () => failedResult.Should().HaveValue(1);

            action.Should()
                .Throw<XunitException>()
                .WithMessage("Value can not be asserted because result is failed because of 'Error 1'");
        }

        [Fact]
        public void Asserting_null_should_not_throw_exceptions_when_type_is_a_primitive()
        {
            var successResult = Result.Ok(null as int?);

            Action action = () => successResult.Should().HaveValue(null);

            action.Should().NotThrow();
        }

        [Fact]
        public void Asserting_null_should_not_throw_exceptions_when_type_is_a_struct()
        {
            var successResult = Result.Ok(null as SomeStruct?);

            Action action = () => successResult.Should().HaveValue(null);

            action.Should().NotThrow();
        }

        [Fact]
        public void Asserting_null_should_not_throw_exceptions_when_type_is_a_class()
        {
            var successResult = Result.Ok(null as SomeClass);

            Action action = () => successResult.Should().HaveValue(null);

            action.Should().NotThrow();
        }

        internal struct SomeStruct
        { }

        internal class SomeClass
        { }
    }
}