using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public record ReasonStringBuilder
    {
        private string reasonType = string.Empty;
        private readonly List<string> infos = new List<string>();
         
        public ReasonStringBuilder WithReasonType(Type type)
        {
            reasonType = type.Name;
            return this;
        }

        public ReasonStringBuilder WithInfo(string label, string value)
        {
            var infoString = value.ToLabelValueStringOrEmpty(label);

            if(!string.IsNullOrEmpty(infoString))
            {
                infos.Add(infoString);
            }

            return this;
        }

        public string Build()
        {
            var reasonInfoText = infos.Any()
                ? " with " + ReasonInfosToString(infos)
                : string.Empty;

            return $"{reasonType}{reasonInfoText}";
        }

        private static string ReasonInfosToString(List<string> reasonInfos)
        {
            return string.Join(", ", reasonInfos);
        }
    }
}