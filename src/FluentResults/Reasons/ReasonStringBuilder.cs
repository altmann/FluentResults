using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Creates the text to dispay when serializing reason instances
    /// </summary>
    public class ReasonStringBuilder
    {
        private string _reasonType = string.Empty;
        private readonly List<string> _infos = new List<string>();
         
        /// <summary>
        /// Specify the type of reason
        /// </summary>
        /// <param name="type">The type of reason</param>
        /// <returns>Reference to the current Builder</returns>
        public ReasonStringBuilder WithReasonType(Type type)
        {
            _reasonType = type.Name;
            return this;
        }

        /// <summary>
        /// Add an information label for the given value
        /// </summary>
        /// <param name="label">The label to display</param>
        /// <param name="value">The value the label is for</param>
        /// <returns>A reference to the current Builder</returns>
        public ReasonStringBuilder WithInfo(string label, string value)
        {
            var infoString = value.ToLabelValueStringOrEmpty(label);

            if(!string.IsNullOrEmpty(infoString))
            {
                _infos.Add(infoString);
            }

            return this;
        }

        /// <summary>
        /// Create the reason string from the current information
        /// </summary>
        /// <returns>The reason string</returns>
        public string Build()
        {
            var reasonInfoText = _infos.Any()
                ? " with " + ReasonInfosToString(_infos)
                : string.Empty;

            return $"{_reasonType}{reasonInfoText}";
        }

        private static string ReasonInfosToString(List<string> reasonInfos)
        {
            return string.Join(", ", reasonInfos);
        }
    }
}