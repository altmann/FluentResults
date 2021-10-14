using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public class ReasonStringBuilder
    {
        private string _reasonType = string.Empty;
        private readonly List<string> _infos = new List<string>();
         
        public ReasonStringBuilder WithReasonType(Type type)
        {
            _reasonType = type.Name;
            return this;
        }

        public ReasonStringBuilder WithInfo(string label, string value)
        {
            var infoString = value.ToLabelValueStringOrEmpty(label);

            if(!string.IsNullOrEmpty(infoString))
            {
                _infos.Add(infoString);
            }

            return this;
        }

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