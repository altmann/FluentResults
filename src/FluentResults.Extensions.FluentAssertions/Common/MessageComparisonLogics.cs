using System;

// ReSharper disable once CheckNamespace
namespace FluentResults.Extensions.FluentAssertions
{
    public static class MessageComparisonLogics
    {
        public static Func<string, string, bool> Equal = (actual, expected) => actual == expected;
        public static Func<string, string, bool> ActualContainsExpected = (actual, expected) => actual.Contains(expected);
    }
}