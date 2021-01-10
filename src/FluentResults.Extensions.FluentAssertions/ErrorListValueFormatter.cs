using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Formatting;

namespace FluentResults.Extensions.FluentAssertions
{
    public class ErrorListValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return value is List<Error>;
        }

        public string Format(object value, FormattingContext context, FormatChild formatChild)
        {
            var errors = (IEnumerable<Error>)value;

            return string.Join("; ", errors.Select(error => error.Message));
        }
    }
}