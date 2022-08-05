using FluentAssertions.Formatting;

// ReSharper disable once CheckNamespace
namespace FluentResults.Extensions.FluentAssertions
{
    public static class ResultFormatters
    {
        public static void Register()
        {
            Formatter.AddFormatter(new ErrorListValueFormatter());
        }
    }
}