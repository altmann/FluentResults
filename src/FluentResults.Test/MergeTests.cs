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
                Result.Ok(),
                Result.Fail("Fail 1")
            };

            var mergedResult = results.Merge();

            mergedResult.IsFailed.Should().BeTrue();
            mergedResult.Errors.Should().HaveCount(1);
        }

        [TestMethod]
        public void Merge_WithFailedResultWithValue_ShouldMergeResults()
        {
            var results = new List<Result<int>>
            {
                Result.Ok(12),
                Result.Fail<int>("Fail 1")
            };

            var mergedResult = results.Merge();

            mergedResult.IsFailed.Should().BeTrue();
            mergedResult.Errors.Should().HaveCount(1);
        }

        [TestMethod]
        public void Merge_WithSuccessResultWithValue_ShouldMergeResults()
        {
            var results = new List<Result<int>>
            {
                Result.Ok(1),
                Result.Ok(2)
            };

            var mergedResult = results.Merge();

            mergedResult.IsSuccess.Should().BeTrue();
            mergedResult.Value.Should().HaveCount(2);
            mergedResult.Value.Should().BeEquivalentTo(new[]
            {
                1, 2
            });
        }
    }
}