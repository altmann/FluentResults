namespace FluentResults
{
    public abstract class ValueResultBase<TResult, TValue> : ResultBase<TResult>
        where TResult : ValueResultBase<TResult, TValue>
    {
        public TValue Value { get; protected set; }

        public TResult WithValue(TValue value)
        {
            Value = value;
            return (TResult)this;
        }

        public Result ConvertTo()
        {
            return ResultHelper.Merge<Result>(this);
        }

        public override string ToString()
        {
            //todo reasons + value
            return "";
        }
    }
}
