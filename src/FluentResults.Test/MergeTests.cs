using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FluentResults.Test
{
    public class MergeTests
    {
        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void MergeFlat_WithSuccessResultWithListValue_ShouldMergeResults()
        {
            var result1 = Result.Ok(new List<string> { "A", "B" });
            var result2 = Result.Ok(new List<string> { "C", "D" });
            var result3 = Result.Ok(new List<string> { "E", "F" });

            var mergedResult = Result.MergeFlat<string, List<string>>(result1, result2, result3);

            mergedResult.IsSuccess.Should().BeTrue();
            mergedResult.Value.Should().HaveCount(6);
            mergedResult.Value.Should().BeEquivalentTo(new[]
            {
                "A", "B", "C", "D", "E", "F"
            });
        }

        [Fact]
        public void MergeFlat_WithSuccessResultWithArrayValue_ShouldMergeResults()
        {
            var result1 = Result.Ok(new string[] { "A", "B" });
            var result2 = Result.Ok(new string[] { "C", "D" });
            var result3 = Result.Ok(new string[] { "E", "F" });

            var mergedResult = Result.MergeFlat<string, string[]>(result1, result2, result3);

            mergedResult.IsSuccess.Should().BeTrue();
            mergedResult.Value.Should().HaveCount(6);
            mergedResult.Value.Should().BeEquivalentTo(new[]
            {
                "A", "B", "C", "D", "E", "F"
            });
        }

        [Fact]
        public void MergeFlat_WithSuccessResultWithEnumerableValue_ShouldMergeResults()
        {
            Result<IEnumerable<string>> result1 = Result.Ok(new string[] { "A", "B" }.Select(a => a));
            Result<IEnumerable<string>> result2 = Result.Ok(new string[] { "C", "D" }.Select(a => a));
            Result<IEnumerable<string>> result3 = Result.Ok(new string[] { "E", "F" }.Select(a => a));

            var mergedResult = Result.MergeFlat<string, IEnumerable<string>>(result1, result2, result3);

            mergedResult.IsSuccess.Should().BeTrue();
            mergedResult.Value.Should().HaveCount(6);
            mergedResult.Value.Should().BeEquivalentTo(new[]
            {
                "A", "B", "C", "D", "E", "F"
            });
        }
    }
}