using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentResults.ReasonStringBuilder
{
    public class DefaultReasonStringBuilder : IReasonStringBuilder
    {
        private string reasonType = string.Empty;
        private readonly List<string> infos = new List<string>();
         
        public IReasonStringBuilder WithReasonType(Type type)
        {
            reasonType = type.Name;
            return this;
        }

        public IReasonStringBuilder WithInfo(string label, string value)
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