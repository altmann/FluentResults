// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public static class ObjectExtensions
    {
        public static Result<TValue> ToResult<TValue>(this TValue value)
        {
            return new Result<TValue>()
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