using System.Collections.Immutable;

using FluentResults.Immutable.Metadata;

namespace FluentResults.Immutable.Tests;

public class ResultWithoutValueTests
{
    [Fact(DisplayName = "Should create a new result with provided reason")]
    public void ShouldCreateANewResultWithProvidedReason()
    {
        var initialResult = Result.Ok();

        var reason = new Reason("A new and totally justifiable reason");

        initialResult.WithReason(reason)
            .Should()
            .NotBe(initialResult)
            .And.Match<Result>(static r => r.IsSuccess)
            .And.BeOfType<Result>()
            .Which.Reasons.Single()
            .Should()
            .Be(reason);
    }

    [Fact(DisplayName = "Should create a new result with provided reasons")]
    public void ShouldCreateANewResultWithProvidedReasons()
    {
        var initialResult = Result.Ok();

        const string message = "A new reason";

        var reasons = Enumerable.Range(1, 5)
            .Select(static i => new Reason($"{message} number {i}"))
            .ToList();

        initialResult.WithReasons(reasons)
            .Should()
            .NotBe(initialResult)
            .And.BeOfType<Result>()
            .Which.Reasons.Should()
            .BeEquivalentTo(reasons.ToImmutableList());
    }

    [Fact(DisplayName = "Should create a new result with an error based on provided message")]
    public void ShouldCreateANewResultWithAnErrorBasedOnProvidedMessage()
    {
        var initialResult = Result.Ok();
        
        const string message = "An error";

        initialResult.WithError(message)
            .Should()
            .NotBe(initialResult)
            .And.BeOfType<Result>()
            .Which.HasError((Error e) => e.Message == message)
            .Should()
            .BeTrue();
    }

    [Fact(DisplayName = "Should create a new result with errors based on provided messages")]
    public void ShouldCreateANewResultWithErrorsBasedOnProvidedMessages()
    {
        var initialResult = Result.Ok();

        var errors = Enumerable.Range(1, 5)
            .Select(static i => $"Error number {i}")
            .ToList();

        initialResult.WithErrors(errors)
            .Should()
            .NotBe(initialResult)
            .And.BeOfType<Result>()
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
            .And.BeOfType<Result>()
            .Which.HasError((Error e) => e.Equals(error))
            .Should()
            .BeTrue();
    }

    [Fact(DisplayName = "Should create a new failed result with provided errors")]
    public void ShouldCreateANewResultWithProvidedErrors()
    {
        var initialResult = Result.Ok();

        var errors = Enumerable.Range(1, 5)
            .Select(static i => new Error($"Error number {i}"))
            .ToList();

        initialResult.WithErrors(errors)
            .Should()
            .NotBe(initialResult)
            .And.BeOfType<Result>()
            .Which.Errors.Should()
            .BeEquivalentTo(errors.ToImmutableList());
    }

    [Fact(DisplayName = "Should create a new successful result with provided success")]
    public void ShouldCreateASuccessfulResultWithProvidedSuccess()
    {
        var initialResult = Result.Ok();

        var success = new Success("A success");

        initialResult.WithSuccess(success)
            .Should()
            .NotBe(initialResult)
            .And.Match<Result>(static r => r.IsSuccess)
            .And.BeOfType<Result>()
            .Which.HasSuccess((Success s) => s.Equals(success))
            .Should()
            .BeTrue();
    }

    [Fact(DisplayName = "Should create a new successful result with provided successes")]
    public void ShouldCreateANewSuccessfulResultWithProvidedSuccesses()
    {
        var initialResult = Result.Ok();

        var successes = Enumerable.Range(1, 10)
            .Select(static i => new Success($"A success number {i}"))
            .ToList();

        initialResult.WithSuccesses(successes)
            .Should()
            .NotBe(initialResult)
            .And.Match<Result>(static r => r.IsSuccess)
            .And.BeOfType<Result>()
            .Which.Successes.Should()
            .BeEquivalentTo(successes.ToImmutableList());
    }
}
