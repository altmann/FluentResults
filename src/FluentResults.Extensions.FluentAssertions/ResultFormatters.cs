using FluentAssertions.Formatting;

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