namespace FluentResults
{
    public class Result : ResultBase<Result>
    {
    }

    public class Result<TValue> : ValueResultBase<Result<TValue>, TValue>
    {
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