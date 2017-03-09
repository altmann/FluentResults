namespace FluentResults
{
    public class Result : ResultBase<Result>
    {
    }

    public class Result<TValue> : ValueResultBase<Result<TValue>, TValue>
    {
        
    }
}