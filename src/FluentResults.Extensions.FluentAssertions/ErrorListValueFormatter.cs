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

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            var errors = (IEnumerable<Error>)value;
            formattedGraph.AddFragment(string.Join("; ", errors.Select(error => error.Message)));
        }
    }
}