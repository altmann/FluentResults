using System;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

// ReSharper disable once CheckNamespace
namespace FluentResults.Extensions.FluentAssertions
{
    public class ReasonAssertions : ReferenceTypeAssertions<IReason, ReasonAssertions>
    {
        public ReasonAssertions(IReason subject)
            : base(subject)
        { }

        protected override string Identifier => nameof(IReason);

        public AndWhichConstraint<ReasonAssertions, IReason> HaveMetadata(string metadataKey, object metadataValue, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .Given(() => Subject.Metadata)
                   .ForCondition(metadata =>
                                 {
                                     metadata.TryGetValue(metadataKey, out var actualMetadataValue);
                                     return actualMetadataValue == metadataValue;
                                 })
                   .FailWith($"Reason should contain '{metadataKey}' with '{metadataValue}', but not contain it");

            return new AndWhichConstraint<ReasonAssertions, IReason>(this, Subject);
        }

        public AndWhichConstraint<ReasonAssertions, IReason> Satisfy<TReason>(Action<TReason> action) where TReason : class, IReason
        {
            var specificReason = Subject as TReason;

            Execute.Assertion
                   .Given(() => Subject)
                   .ForCondition(reason => reason is TReason)
                   .FailWith($"Reason should be of type '{typeof(TReason)}', but is of type '{Subject.GetType()}'");

            action(specificReason);

            return new AndWhichConstraint<ReasonAssertions, IReason>(this, Subject);
        }
    }
}