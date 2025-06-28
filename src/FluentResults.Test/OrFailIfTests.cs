using System.Linq;
using FluentAssertions;
using FluentResults.Extensions;
using Xunit;

namespace FluentResults.Test
{
    public class OrFailIfTests
    {
        [Fact]
        public void Does_not_evaluate_if_the_previous_statement_has_failed()
        {
            var result = Result.FailIf(true, "Error 1")
                .OrFailIf(true, "Error 2");

            result.IsFailed.Should().BeTrue();
            result.Errors.Select(s => s.Message)
                .Should()
                .BeEquivalentTo("Error 1");
        }

        [Fact]
        public void Evaluates_if_the_previous_statement_has_succeeded()
        {
            var result = Result.FailIf(false, "Error 1")
                .OrFailIf(true, "Error 2");

            result.IsFailed.Should().BeTrue();
            result.Errors.Select(s => s.Message)
                .Should()
                .BeEquivalentTo("Error 2");
        }

        [Fact]
        public void Succeeds_if_all_statements_have_succeeded()
        {
            var result = Result.FailIf(false, "Error 1")
                .OrFailIf(false, "Error 2");

            result.IsSuccess.Should().BeTrue();
        }
    }
}