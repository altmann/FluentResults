using System;

namespace FluentResults.Extensions.FluentAssertions
{
    public static class FluentResultAssertionsConfig
    {
        public static Func<string, string, bool> ErrorMessageComparison { get; set; } = ErrorMessageComparisonLogics.Equal;
    }
}