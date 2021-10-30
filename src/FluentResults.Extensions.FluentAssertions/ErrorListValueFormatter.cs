using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Formatting;

namespace FluentResults.Extensions.FluentAssertions
{
    public class ErrorListValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return value is List<IError>;
        }

        public void Format(object value, FormattedObjectGraph formattedGraph, FormattingContext context, FormatChild formatChild)
        {
            var errors = (IEnumerable<IError>)value;
            formattedGraph.AddFragment(string.Join("; ", errors.Select(error => error.Message)));
        }
    }
}