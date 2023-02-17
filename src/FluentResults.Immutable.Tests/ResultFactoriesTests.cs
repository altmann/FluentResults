using System.Collections.Immutable;
using FluentResults.Immutable.Extensions;
using FluentResults.Immutable.Metadata;
using FluentResults.Immutable.Tests.Extensions;
using FsCheck;
using FsCheck.Xunit;
using static FluentResults.Immutable.Tests.Generators.IntegerResultGenerator;

namespace FluentResults.Immutable.Tests;

public class ResultFactoriesTests
{
    private const string ErrorMessage = "An error message";

    [Fact(DisplayName = "Should create successful result with no reasons")]
    public void ShouldCreateSuccessfulResultWithoutReasons() =>
        Result.Ok()
            .Should()
            .Match<Result<Unit>>(static r => r.IsSuccessful && !r.Reasons.Any() && ValueIsAUnit(r));

    [Fact(DisplayName = "Should create a failed result with one error from error message")]
    public void ShouldCreateFailedResultFromErrorMessage() =>
        Result.Fail(ErrorMessage)
            .Should()
            .Match<Result<Unit>>(
                static r => r.IsAFailure &&
                    r.Errors.ToArray()
                        .Single()
                        .Message ==
                    ErrorMessage);

    [Fact(DisplayName = "Should create a failed result with one error from error record")]
    public void ShouldCreateFailedResultFromAnErrorRecord()
    {
        var error = new Error(ErrorMessage);

        Result.Fail(error)
            .Should()
            .Match<Result<Unit>>(
                r => r.IsAFailure &&
                    r.Errors.Single()
                        .Equals(error));
    }

    [Fact(DisplayName = "HasError should return true for a failed result with generic error")]
    public void ShouldReturnTrueForFailedResult()
    {
        var error = new Error("A failure");

        Result.Fail(error)
            .HasError<Error>()
            .Should()
            .BeTrue();
    }

    [Fact(DisplayName = "HasError should return true for a defined, nested error")]
    public void ShouldReturnTrueForANestedError()
    {
        var complexError = new Error(
            "Some complex failure",
            ImmutableList.Create<Error>(new RootError("Root cause of a failure")),
            ImmutableDictionary<string, object>.Empty);

        Result.Fail(complexError)
            .HasError<RootError>()
            .Should()
            .BeTrue();
    }

    [Fact(DisplayName = "HasSuccess should return true for a defined, nested success")]
    public void ShouldReturnTrueForANestedSuccess()
    {
        var complexSuccess = new Success(
            "Some complex success",
            ImmutableList.Create<Success>(new RootSuccess("Root cause")),
            ImmutableDictionary<string, object>.Empty);

        Result.Ok(
                Unit.Value,
                new[]
                {
                    complexSuccess,
                })
            .HasSuccess<RootSuccess>()
            .Should()
            .BeTrue();
    }

    [Fact(DisplayName = "Should create successful result with a provided value without any reasons")]
    public void ShouldCreateSuccessfulResultWithProvidedValue() =>
        Result.Ok(0)
            .Should()
            .Match<Result<int>>(static r => r.IsSuccessful && !r.Reasons.Any() && r.ValueMatches(static i => i == 0));

    [Property(
        DisplayName = "Merger of a list of successful results should result in a success",
        MaxTest = 1000)]
    public Property MergingAListOfSuccessfulResultsShouldReturnSuccessfulResult() =>
        Prop.ForAll(
            Gen.ListOf(GetSuccessfulIntegerResultGenerator())
                .ToArbitrary(),
            static list =>
                list.Merge() is { IsSuccessful: true, Value: Some<IEnumerable<int>> { Value: var enumerable, }, } &&
                enumerable.SequenceEqual(
                    list.Select(static r => r.Value)
                        .Cast<Some<int>>()
                        .Select(static s => s.Value)));

    [Property(
        DisplayName = "Merger of a list of results is a failure if any of them has failed",
        MaxTest = 1000)]
    public Property MergingAListOrResultsIsAFailureIfAnyOfThemHasFailed() =>
        Prop.ForAll(
            Gen.ListOf(GetIntegerResultGenerator())
                .Where(static list => list.Any(static r => r.IsAFailure))
                .ToArbitrary(),
            static list => list.Merge()
                .IsAFailure);

    [Fact(DisplayName = "Merger of an empty list of results is successful")]
    public void MergingOfAnEmptyListOfResultsIsSuccessful() =>
        Enumerable.Empty<Result<Unit>>()
            .Merge()
            .Should()
            .Match<Result<IEnumerable<Unit>>>(static r => r.IsSuccessful && r.ValueMatches(static e => !e.Any()));

    [Property(
        DisplayName = "Merger of successful Result params is successful",
        MaxTest = 1000)]
    public Property MergingOfSuccessfulResultsIsSuccessful() =>
        Prop.ForAll(
            Gen.Two(GetSuccessfulIntegerResultGenerator())
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second) = tuple;

                return Result.Merge(first, second)
                    .IsSuccessful.ToProperty();
            });

    [Property(
        DisplayName = "Merger of Result params is a failure if any of them is a failure",
        MaxTest = 1000)]
    public Property MergingOfResultsIsAFailureIfAnyOfThemIsAFailure() =>
        Prop.ForAll(
            Gen.Two(GetIntegerResultGenerator())
                .Where(
                    static tuple =>
                    {
                        var (first, second) = tuple;

                        return first.IsAFailure || second.IsAFailure;
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second) = tuple;

                return Result.Merge(first, second)
                    .IsAFailure.ToProperty();
            });

    private static bool ValueIsAUnit(Result<Unit> result) => result.ValueMatches(static u => u == Unit.Value);

    private sealed record RootError(
            string Message,
            ImmutableList<Error> Errors,
            ImmutableDictionary<string, object> Metadata)
        : Error(
            Message,
            Errors,
            Metadata)
    {
        public RootError(string message)
            : this(
                message,
                ImmutableList<Error>.Empty,
                ImmutableDictionary<string, object>.Empty)
        {
        }
    }

    private sealed record RootSuccess(
            string Message,
            ImmutableList<Success> Successes,
            ImmutableDictionary<string, object> Metadata)
        : Success(
            Message,
            Successes,
            Metadata)
    {
        public RootSuccess(string message)
            : this(
                message,
                ImmutableList<Success>.Empty,
                ImmutableDictionary<string, object>.Empty)
        {
        }
    }
}