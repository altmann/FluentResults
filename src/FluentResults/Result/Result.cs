using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

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

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(nameof(Result));
            builder.Append(" { ");

            if (PrintMembers(builder))
                builder.Append(" ");

            builder.Append("}");
            return builder.ToString();
        }
    }

    public class Result<TValue> : ResultBase<Result<TValue>>
    {
        private TValue? _value;

        /// <summary>
        /// Get the Value. If result is failed then a default value is returned. Opposite see property Value.
        /// </summary>
        public TValue? ValueOrDefault => _value;

        /// <summary>
        /// Get the Value. If result is failed then an Exception is thrown because a failed result has no value. Opposite see property ValueOrDefault.
        /// </summary>
        public TValue Value
        {
            get
            {
                if (IsFailed)
                    throw new InvalidOperationException("Result is in status failed. Value is not set.");

                return _value!;
            }
            private set
            {
                if (IsFailed)
                    throw new InvalidOperationException("Result is in status failed. Value is not set.");

                _value = value;
            }
        }

        /// <summary>
        /// Set value
        /// </summary>
        public Result<TValue> WithValue(TValue value)
        {
            Value = value;
            return this;
        }

        /// <summary>
        /// Convert result with value to result without value
        /// </summary>
        public Result ToResult()
        {
            return new Result()
                .WithReasons(Reasons);
        }

        /// <summary>
        /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
        /// </summary>
        public Result<TNewValue> ToResult<TNewValue>(Func<TValue, TNewValue>? valueConverter = null)
        {
            if (IsSuccess && valueConverter == null)
                throw new ArgumentException("If result is success then valueConverter should not be null");

            return new Result<TNewValue>()
                .WithValue(IsSuccess ? valueConverter!(Value) : default!)
                .WithReasons(Reasons);
        }

        protected override bool PrintMembers(StringBuilder builder)
        {
            var printed = base.PrintMembers(builder);

            if (IsSuccess)
            {
                if (printed)
                    builder.Append(", ");

                builder.Append(nameof(Value));
                builder.Append(" = ");
                builder.Append(Value);
            }

            return true;
        }
        
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(nameof(Result<TValue>));
            builder.Append(" { ");

            if (PrintMembers(builder))
                builder.Append(" ");

            builder.Append("}");
            return builder.ToString();
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