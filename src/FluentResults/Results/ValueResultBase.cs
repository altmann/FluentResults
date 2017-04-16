using System;

namespace FluentResults
{
    public abstract class ValueResultBase<TResult, TValue> : ResultBase<TResult>
        where TResult : ValueResultBase<TResult, TValue>
    {
        public TValue Value { get; set; }

        public TResult WithValue(TValue value)
        {
            Value = value;
            return (TResult)this;
        }

        public TResult With(Action<TResult> setProperty)
        {
            setProperty((TResult)this);
            return (TResult)this;
        }

        public Result ConvertTo()
        {
            return ResultHelper.Merge<Result>(this);
        }

        public override string ToString()
        {
            var baseString = base.ToString();
            var valueString = Value.ToLabelValueStringOrEmpty(nameof(Value));
            return $"{baseString}, {valueString}";
        }
    }
}
