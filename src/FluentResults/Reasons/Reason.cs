using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Reason class is the base class
    /// </summary>
    public record Reason(string Message = "")
    {
        private readonly ImmutableSortedDictionary<string, object> _metadata = ImmutableSortedDictionary<string, object>.Empty;

        public IReadOnlyDictionary<string, object> Metadata
        {
            get => _metadata;
            protected init => _metadata = value.ToImmutableSortedDictionary();
        }

        /// <summary>
        /// Set the metadata
        /// </summary>
        public virtual Reason WithMetadata(string metadataName, object metadataValue)
        {
            return this with
            {
                Metadata = _metadata.Add(metadataName, metadataValue),
            };
        }

        /// <summary>
        /// Set the metadata
        /// </summary>
        public virtual Reason WithMetadata(IDictionary<string, object> metadata)
        {
            return this with
            {
                Metadata = _metadata.AddRange(metadata),
            };
        }

        public bool HasMetadataKey(string key)
        {
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return Metadata.ContainsKey(key);
        }

        public bool HasMetadata(string key, Func<object, bool> predicate)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (Metadata.TryGetValue(key, out var actualValue))
                return predicate(actualValue);

            return false;
        }

        internal virtual IEnumerable<object> GetEqualityComponents()
        {
            yield return Message;

            foreach (var (key, value) in _metadata)
            {
                yield return key;
                yield return value;
            }
        }

        public virtual bool Equals(Reason? other)
        {
            return other != null && this.EqualityContract == other.EqualityContract
                                 && this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            foreach (var component in GetEqualityComponents())
            {
                hashCode.Add(component);
            }
            return hashCode.ToHashCode();
        }

        protected virtual bool PrintMembers(StringBuilder builder)
        {
            builder.Append(nameof(Message));
            builder.Append(" = ");
            builder.Append(Message);

            if (!_metadata.IsEmpty)
            {
                builder.Append(", ");

                builder.Append(nameof(Metadata));
                builder.Append(" = [ ");

                foreach (var (key, value) in _metadata)
                {
                    builder.Append("{ ");
                    builder.Append(key);
                    builder.Append(" = ");
                    builder.Append(value);

                    builder.Append(" }, ");
                }

                builder.Remove(builder.Length - 2, 2);
                builder.Append(" ]");
            }

            return true;
        }
    }
}