using System.Collections.Immutable;

using FluentResults.Immutable.Metadata;

namespace FluentResults.Immutable.Tests;

public class ResultOfUnitTests
{
    public static readonly TheoryData<Error> Errors = new()
    {
        new("Sample flat error"),
        new(
            "Complex error",
            ImmutableList.Create(new Error("Cause of the original error")),
            ImmutableDictionary<string, object>.Empty),
    };

    private static readonly Result<Unit> InitialResult = Result.Ok();

    [Fact(DisplayName = "Should create a new result with provided reason")]
    public void ShouldCreateANewResultWithProvidedReason()
    {
        var reason = new Reason("A new and totally justifiable reason");

        InitialResult.WithReason(reason)
            .Should()
            .NotBe(InitialResult)
            .And.Match<Result<Unit>>(static r => r.IsSuccess)
            .And.BeOfType<Result<Unit>>()
            .Which.Reasons.Single()
            .Should()
            .Be(reason);
    }

    [Fact(DisplayName = "Should create a new result with provided reasons")]
    public void ShouldCreateANewResultWithProvidedReasons()
    {
        const string message = "A new reason";

        var reasons = Enumerable.Range(1, 5)
            .Select(static i => new Reason($"{message} number {i}"))
            .ToList();

        InitialResult.WithReasons(reasons)
            .Should()
            .NotBe(InitialResult)
            .And.BeOfType<Result<Unit>>()
            .Which.Reasons.Should()
            .BeEquivalentTo(reasons.ToImmutableList());
    }

    [Fact(DisplayName = "Should create a new result with an error based on provided message")]
    public void ShouldCreateANewResultWithAnErrorBasedOnProvidedMessage()
    {
        const string message = "An error";

        InitialResult.WithError(message)
            .Should()
            .NotBe(InitialResult)
            .And.BeOfType<Result<Unit>>()
            .Which.HasError(static (Error e) => e.Message == message)
            .Should()
            .BeTrue();
    }

    [Fact(DisplayName = "Should create a new result with errors based on provided messages")]
    public void ShouldCreateANewResultWithErrorsBasedOnProvidedMessages()
    {
        var errors = Enumerable.Range(1, 5)
            .Select(static i => $"Error number {i}")
            .ToList();

        InitialResult.WithErrors(errors)
            .Should()
            .NotBe(InitialResult)
            .And.BeOfType<Result<Unit>>()
            .Which.Errors.Select(static e => e.Message)
            .Should()
            .BeEquivalentTo(errors);
    }

    [Fact(DisplayName = "Should create a new result with provided error")]
    public void ShouldCreateANewResultWithProvidedError()
    {
        var initialResult = Result.Ok();

        var error = new Error("An error");

        initialResult.WithError(error)
            .Should()
            .NotBe(initialResult)
            .And.BeOfType<Result<Unit>>()
            .Which.HasError((Error e) => e.Equals(error))
            .Should()
            .BeTrue();
    }

    [Fact(DisplayName = "Should create a new failed result with provided errors")]
    public void ShouldCreateANewResultWithProvidedErrors()
    {
        var errors = Enumerable.Range(1, 5)
            .Select(static i => new Error($"Error number {i}"))
            .ToList();

        InitialResult.WithErrors(errors)
            .Should()
            .NotBe(InitialResult)
            .And.BeOfType<Result<Unit>>()
            .Which.Errors.Should()
            .BeEquivalentTo(errors.ToImmutableList());
    }

    [Fact(DisplayName = "Should create a new successful result with provided success")]
    public void ShouldCreateASuccessfulResultWithProvidedSuccess()
    {
        var success = new Success("A success");

        InitialResult.WithSuccess(success)
            .Should()
            .NotBe(InitialResult)
            .And.Match<Result<Unit>>(static r => r.IsSuccess)
            .And.BeOfType<Result<Unit>>()
            .Which.HasSuccess((Success s) => s.Equals(success))
            .Should()
            .BeTrue();
    }

    [Fact(DisplayName = "Should create a new successful result with provided successes")]
    public void ShouldCreateANewSuccessfulResultWithProvidedSuccesses()
    {
        var successes = Enumerable.Range(1, 10)
            .Select(static i => new Success($"A success number {i}"))
            .ToList();

        InitialResult.WithSuccesses(successes)
            .Should()
            .NotBe(InitialResult)
            .And.Match<Result<Unit>>(static r => r.IsSuccess)
            .And.BeOfType<Result<Unit>>()
            .Which.Successes.Should()
            .BeEquivalentTo(successes.ToImmutableList());
    }

    [Fact(DisplayName = "Should create a new result containing a generic error with provided error message")]
    public void ShouldCreateANewResultWithAGenericErrorWithProvidedMessage()
    {
        const string message = "An error";

        InitialResult.WithError(message)
            .Should()
            .NotBe(InitialResult)
            .And.BeOfType<Result<Unit>>()
            .Which.Errors.Single()
            .Should()
            .Match<Error>(static e => e.Message == message);
    }

    [Theory(DisplayName = "Should create a new result containing provided error and its underlying causes")]
    [MemberData(nameof(Errors))]
    public void ShouldCreateANewResultContainingProvidedErrorAndItsCauses(Error error)
    {
        InitialResult.WithError(error)
            .Should()
            .NotBe(InitialResult)
            .And.BeOfType<Result<Unit>>()
            .Which.Errors
            .Should()
            .BeEquivalentTo(Flatten(error));

        static IEnumerable<Error> Flatten(Error e) =>
            new[]
            {
                e,
            }.Concat(e.Errors.SelectMany(Flatten));
    }

    [Fact(DisplayName = "Should create a new result with generic errors containing provided messages")]
    public void ShouldCreateANewResultWithGenericErrorsContainingProvidedMessages()
    {
        var errorMessages = Enumerable.Repeat("Error message no", 10)
            .Select(static (messageTemplate, index) => $"{messageTemplate} {index + 1}")
            .ToList();

        InitialResult.WithErrors(errorMessages)
            .Should()
            .NotBe(InitialResult)
            .And.BeOfType<Result<Unit>>()
            .Which.Errors
            .Select(static e => e.Message)
            .Should()
            .BeEquivalentTo(errorMessages);
    }

    [Fact(DisplayName = "HasError should return false for successful result")]
    public void ShouldReturnFalseForSuccessfulResult() =>
        InitialResult.HasError<Error>()
            .Should()
            .BeFalse();

    [Fact(DisplayName = "No-op bind on a successful result of unit should return a new result with a unit")]
    public void NoOpBindOnSuccessfulResultShouldReturnAUnit() =>
        InitialResult.Bind(static () => Result.Ok())
            .Should()
            .NotBe(InitialResult)
            .And.BeOfType<Result<Unit>>()
            .Which.IsSuccess.Should()
            .BeTrue();
}
