using System;

// ReSharper disable once CheckNamespace
namespace FluentResults.Extensions.FluentAssertions
{
    public static class FluentResultAssertionsConfig
    {
        public static Func<string, string, bool> MessageComparison { get; set; } = MessageComparisonLogics.Equal;
    }
}