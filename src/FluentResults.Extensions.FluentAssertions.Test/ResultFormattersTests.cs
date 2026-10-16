using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions.Formatting;
using Xunit;
using Xunit.Sdk;

namespace FluentResults.Extensions.FluentAssertions.Test
{
    [CollectionDefinition("Formatter registration", DisableParallelization = true)]
    public class FormatterRegistrationCollection
    {
    }

    [Collection("Formatter registration")]
    public class ResultFormattersTests
    {
        [Fact]
        public void Explicit_registration_can_add_and_restore_the_formatter()
        {
            Result.Ok().Should().BeSuccess();
            var originalFormatters = Formatter.Formatters.OfType<ErrorListValueFormatter>().ToArray();

            try
            {
                ResultFormatters.Register();
                Assert.Equal(originalFormatters.Length + 1,
                    Formatter.Formatters.OfType<ErrorListValueFormatter>().Count());

                foreach (var formatter in Formatter.Formatters.OfType<ErrorListValueFormatter>().ToArray())
                    Formatter.RemoveFormatter(formatter);

                Assert.Empty(Formatter.Formatters.OfType<ErrorListValueFormatter>());
                ResultFormatters.Register();
                Assert.Single(Formatter.Formatters.OfType<ErrorListValueFormatter>());
            }
            finally
            {
                foreach (var formatter in Formatter.Formatters.OfType<ErrorListValueFormatter>().ToArray())
                    Formatter.RemoveFormatter(formatter);

                foreach (var formatter in originalFormatters)
                    Formatter.AddFormatter(formatter);
            }
        }

        [Fact]
        public async Task Assertions_for_different_result_types_do_not_modify_the_formatter_collection()
        {
            Result.Ok(new FirstValue()).Should().BeSuccess();

            using (var reader = Formatter.Formatters.GetEnumerator())
            {
                Assert.True(reader.MoveNext());

                await Task.Run(() =>
                {
                    Result.Ok(new SecondValue()).Should().BeSuccess();
                    Result.Ok().Should().BeSuccess();
                });

                while (reader.MoveNext())
                {
                }
            }

            Assert.Single(Formatter.Formatters.OfType<ErrorListValueFormatter>());
        }

        [Fact]
        public void Error_formatting_is_preserved()
        {
            var result = Result.Fail<FormattingValue>(new[] { "Error 1", "Error 2" });

            var exception = Assert.Throws<XunitException>(() => result.Should().BeSuccess());

            Assert.Equal("Expected result be success, but is failed because of '{Error with Message='Error 1', Error with Message='Error 2'}'", exception.Message);
            Assert.Equal("Error 1; Error 2", Formatter.ToString(new List<IError>
            {
                new Error("Error 1"),
                new Error("Error 2")
            }));
        }

        private class FirstValue
        {
        }

        private class SecondValue
        {
        }

        private class FormattingValue
        {
        }
    }
}
