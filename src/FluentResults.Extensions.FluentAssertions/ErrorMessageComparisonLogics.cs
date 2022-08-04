using System;

namespace FluentResults.Extensions.FluentAssertions
{
    public static class ErrorMessageComparisonLogics
    {
        public static Func<string, string, bool> Equal = (actual, expected) => actual == expected;
        public static Func<string, string, bool> ActualContainsExpected = (actual, expected) => actual.Contains(expected);
    }
}