using System;
using AwesomeAssertions.Execution;

namespace FluentResults.Extensions.AwesomeAssertions
{
    public static class ResultExtensions
    {
        public static ResultAssertions Should(this Result value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return new ResultAssertions(value, AssertionChain.GetOrCreate());
        }

        public static ResultAssertions<T> Should<T>(this Result<T> value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return new ResultAssertions<T>(value, AssertionChain.GetOrCreate());
        }
    }
}