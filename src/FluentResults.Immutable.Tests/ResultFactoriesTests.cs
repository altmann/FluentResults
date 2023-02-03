using FluentResults.Immutable.Metadata;

namespace FluentResults.Immutable.Tests;

public class ResultFactoriesTests
{
    private const string ErrorMessage = "An error message";

    [Fact(DisplayName = "Should create succesful result with no reasons")]
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
}