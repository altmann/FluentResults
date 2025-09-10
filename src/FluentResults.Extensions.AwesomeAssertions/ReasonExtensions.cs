using System;
using AwesomeAssertions.Execution;

namespace FluentResults.Extensions.AwesomeAssertions
{
    public static class ReasonExtensions
    {
        public static ReasonAssertions Should(this IReason reason)
        {
            if (reason == null) throw new ArgumentNullException(nameof(reason));
            return new ReasonAssertions(reason, AssertionChain.GetOrCreate());
        }
    }
}