using FluentResults.Immutable.Metadata;

using System.Collections.Immutable;

namespace FluentResults.Immutable.Tests;

public class ResultFactoriesTests
{
    private const string ErrorMessage = "An error message";

    [Fact(DisplayName = "Should create successful result with no reasons")]
    public void ShouldCreateSuccessfulResultWithoutReasons() =>
        Result.Ok()
            .Should()
            .Match<Result<Unit>>(static r => r.IsSuccess && !r.Reasons.Any());

    [Fact(DisplayName = "Should create a failed result with one error from error message")]
    public void ShouldCreateFailedResultFromErrorMessage() => 
        Result.Fail(ErrorMessage)
            .Should()
            .Match<Result<Unit>>(
                static r => r.IsFailed && r.Errors.ToArray().Single().Message == ErrorMessage);

    [Fact(DisplayName = "Should create a failed result with one error from error record")]
    public void ShouldCreateFailedResultFromAnErrorRecord()
    {
        var error = new Error(ErrorMessage);

        Result.Fail(error)
            .Should()
            .Match<Result<Unit>>(
                r => r.IsFailed && r.Errors.Single().Equals(error));
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
}