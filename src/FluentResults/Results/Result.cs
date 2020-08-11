using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public partial class Result : ResultBase<Result>
    {
        public Result()
        { }

        public Result<TNewValue> ToResult<TNewValue>()
        {
            return new Result<TNewValue>()
                .WithReasons(Reasons);
        }
    }

    public class Result<TValue> : ResultBase<Result<TValue>>
    {
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
            return new Result()
                .WithReasons(Reasons);
        }

        public Result<TNewValue> ToResult<TNewValue>(Func<TValue, TNewValue> valueConverter = null)
        {
            if(IsSuccess && valueConverter == null)
                throw new ArgumentException("If result is success then valueConverter should not be null");

            return new Result<TNewValue>()
                .WithValue(IsFailed ? default : valueConverter(Value))
                .WithReasons(Reasons);
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