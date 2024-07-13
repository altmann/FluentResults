// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Extension methods for object base type
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Convert value to result
        /// </summary>
        /// <typeparam name="TValue">The type of the result's <see cref="Result{TValue}.Value"/></typeparam>
        /// <param name="value">The value of the result</param>
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