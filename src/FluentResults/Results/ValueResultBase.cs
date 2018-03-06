using System;

namespace FluentResults
{
    public abstract class ValueResultBase<TResult, TValue> : ResultBase<TResult>
        where TResult : ValueResultBase<TResult, TValue>
    {
        private TValue _value;

        public TValue ValueOrDefault
        {
            get => _value;
        }

        public TValue Value
        {
            get
            {
                if (IsFailed)
                    throw new InvalidOperationException("Result is in status failed. Value is not set.");

                return _value;
            }
            set
            {
                if(IsFailed)
                    throw new InvalidOperationException("Result is in status failed. Value is not set.");

                _value = value;
            }
        }

        public TResult WithValue(TValue value)
        {
            Value = value;
            return (TResult)this;
        }

        public override string ToString()
        {
            var baseString = base.ToString();
            var valueString = ValueOrDefault.ToLabelValueStringOrEmpty(nameof(Value));
            return $"{baseString}, {valueString}";
        }
    }
}
