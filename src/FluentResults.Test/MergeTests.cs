using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentResults.Test
{
    [TestClass]
    public class MergeTests
    {
        [TestMethod]
        public void Merge_WithResultWithoutValue_ShouldMergeResults()
        {
            var results = new List<Result>
            {
                Results.Ok(),
                Results.Fail("Fail 1")
            };

            var mergedResult = results.Merge();

            mergedResult.IsFailed.Should().BeTrue();
            mergedResult.Errors.Should().HaveCount(1);
        }

        [TestMethod]
        public void Merge_WithResultWithValue_ShouldMergeResults()
        {
            var results = new List<Result<int>>
            {
                Results.Ok(12),
                Results.Fail<int>("Fail 1")
            };

            var mergedResult = results.Merge();

            mergedResult.IsFailed.Should().BeTrue();
            mergedResult.Errors.Should().HaveCount(1);
        }

        [TestMethod]
        public void Merge_WithResultWithValue2_ShouldMergeResults()
        {
            var results = new List<Result<int>>
            {
                Results.Ok(1),
                Results.Ok(2)
            };

            var mergedResult = results.Merge();

            mergedResult.IsSuccess.Should().BeTrue();
            mergedResult.Value.Should().HaveCount(2);
        }
    }
}