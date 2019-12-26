using System;

namespace FluentResults
{
    public class Result : ResultBase<Result>
    {
        [Obsolete("Removed in next major version. Use Results.Ok() instead.")]
        public Result()
        { }
    }

    public class Result<TValue> : ValueResultBase<Result<TValue>, TValue>
    {
        [Obsolete("Removed in next major version. Use Results.Ok<TValue>(TValue value) instead.")]
        public Result()
        { }

        public static implicit operator Result<TValue>(Result result)
        {
            return result.ToResult<TValue>();
        }

        public static implicit operator Result(Result<TValue> result)
        {
            return result.ToResult();
        }
    }
}