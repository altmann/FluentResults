using System.Collections.Generic;
using FluentAssertions;

namespace FluentResults.Extensions.FluentAssertions
{
    public class AndWhichThatConstraint<TParentConstraint, TMatchedElement, TThatConstraint> : AndWhichConstraint<TParentConstraint, TMatchedElement>
    {
        public AndWhichThatConstraint(TParentConstraint parentConstraint, TMatchedElement matchedConstraint, TThatConstraint thatConstraint)
            : base(parentConstraint, matchedConstraint)
        {
            That = thatConstraint;
        }

        public AndWhichThatConstraint(TParentConstraint parentConstraint, TMatchedElement matchedConstraint, IEnumerable<TMatchedElement> matchedElements)
            : base(parentConstraint, matchedConstraint)
        {
        }

        public TThatConstraint That { get; }
    }
}