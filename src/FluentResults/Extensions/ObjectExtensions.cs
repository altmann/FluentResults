namespace FluentResults
{
    public static class ObjectExtensions
    {
        public static Result<TValue> ToResult<TValue>(this TValue value)
        {
            return new Result<TValue>()
                .WithValue(value);
        }

        public static TResult ToResult<TResult, TValue>(this TValue value)
            where TResult : ValueResultBase<TResult, TValue>, new()
        {
            return new TResult()
                .WithValue(value);
        }

        internal static string ToLabelValueStringOrEmpty(this object value, string label)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var valueText = value.ToString();

            if (valueText == string.Empty)
            {
                return string.Empty;
            }

            return $"{label}='{valueText}'";
        }
    }
}