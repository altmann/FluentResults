using FluentAssertions.Formatting;

// ReSharper disable once CheckNamespace
namespace FluentResults.Extensions.AwesomeAssertions
{
    public static class ResultFormatters
    {
        public static void Register()
        {
            Formatter.AddFormatter(new ErrorListValueFormatter());
        }
    }
}