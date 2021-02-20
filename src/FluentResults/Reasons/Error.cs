using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Objects from Error class cause a failed result
    /// </summary>
    public record Error : Reason
    {
        private readonly ImmutableList<Error> _reasons = ImmutableList<Error>.Empty;

        /// <summary>
        /// Get the reasons of an error
        /// </summary>
        public IReadOnlyList<Error> Reasons
        {
            get => _reasons;
            private init => _reasons = value.ToImmutableList();
        }

        public Error(string message = "", Error? causedBy = null)
            : base(message)
        {
            if (causedBy is not null)
            {
                Reasons = _reasons.Add(causedBy);
            }
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(Error error)
        {
            return this with
            {
                Reasons = _reasons.Add(error),
            };
        }
        
        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(Exception exception)
        {
            return CausedBy(new ExceptionalError(exception));
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(string message, Exception exception)
        {
            return CausedBy(new ExceptionalError(exception, message));
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(string message)
        {
            return CausedBy(new Error(message));
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(IEnumerable<Error> errors)
        {
            return this with
            {
                Reasons = _reasons.AddRange(errors),
            };
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(IEnumerable<string> errors)
        {
            return CausedBy(errors.Select(errorMessage => new Error(errorMessage)));
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(params Error[] errors)
        {
            return CausedBy(errors.AsEnumerable());
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public Error CausedBy(params string[] errors)
        {
            return CausedBy(errors.Select(errorMessage => new Error(errorMessage)));
        }

        /// <summary>
        /// Set the message
        /// </summary>
        public Error WithMessage(string message)
        {
            return this with
            {
                Message = message,
            };
        }

        public override Error WithMetadata(string metadataName, object metadataValue)
        {
            return (Error)base.WithMetadata(metadataName, metadataValue);
        }

        public override Error WithMetadata(IDictionary<string, object> metadata)
        {
            return (Error)base.WithMetadata(metadata);
        }
        
        internal override IEnumerable<object> GetEqualityComponents()
        {
            foreach (var component in base.GetEqualityComponents())
            {
                yield return component;
            }

            foreach (var reason in _reasons)
            {
                yield return reason;
            }
        }

        protected override bool PrintMembers(StringBuilder builder)
        {
            if (base.PrintMembers(builder) && !_reasons.IsEmpty)
                builder.Append(", ");

            if (!_reasons.IsEmpty)
            {
                builder.Append(nameof(Reasons));
                builder.Append(" = [ ");

                foreach (var reason in _reasons)
                {
                    builder.Append(reason);
                    builder.Append(", ");
                }

                builder.Remove(builder.Length - 2, 2);
                builder.Append(" ]");
            }

            return true;
        }
    }

    internal class ReasonFormat
    {
        public static string ErrorReasonsToString(IReadOnlyList<Error> errorReasons)
        {
            return string.Join("; ", errorReasons);
        }

        public static string ReasonsToString(IReadOnlyList<Reason> errorReasons)
        {
            return string.Join("; ", errorReasons);
        }
    }
}