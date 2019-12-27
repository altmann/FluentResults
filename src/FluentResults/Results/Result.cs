using System;

namespace FluentResults
{
    public class Result : ResultBase<Result>
    {
        [Obsolete("Removed in next major version. Use Results.Ok() instead.")]
        public Result()
        { }
    }

    public class Result<TValue> : ResultBase<Result<TValue>>
    {
        [Obsolete("Removed in next major version. Use Results.Ok<TValue>(TValue value) instead.")]
        public Result()
        { }

        private TValue _value;

        public TValue ValueOrDefault => _value;

        public TValue Value
        {
            get
            {
                if (IsFailed)
                    throw new InvalidOperationException("Result is in status failed. Value is not set.");

                return _value;
            }
            private set
            {
                if (IsFailed)
                    throw new InvalidOperationException("Result is in status failed. Value is not set.");

                _value = value;
            }
        }

        public Result<TValue> WithValue(TValue value)
        {
            Value = value;
            return this;
        }

        public Result ToResult()
        {
            return ResultHelper.Merge(this);
        }

        public override string ToString()
        {
            var baseString = base.ToString();
            var valueString = ValueOrDefault.ToLabelValueStringOrEmpty(nameof(Value));
            return $"{baseString}, {valueString}";
        }

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