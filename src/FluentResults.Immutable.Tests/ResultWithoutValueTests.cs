﻿using System.Collections.Immutable;

using FluentResults.Immutable.Metadata;

namespace FluentResults.Immutable.Tests;

public class ResultWithoutValueTests
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
        var initialResult = Result.Ok();
        
        const string message = "An error";

        initialResult.WithError(message)
            .Should()
            .NotBe(initialResult)
            .And.BeOfType<Result<Unit>>()
            .Which.HasError(static (Error e) => e.Message == message)
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
        var initialResult = Result.Ok();

        var errors = Enumerable.Range(1, 5)
            .Select(static i => new Error($"Error number {i}"))
            .ToList();

        initialResult.WithErrors(errors)
            .Should()
            .NotBe(initialResult)
            .And.BeOfType<Result<Unit>>()
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
            .And.Match<Result<Unit>>(static r => r.IsSuccess)
            .And.BeOfType<Result<Unit>>()
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
