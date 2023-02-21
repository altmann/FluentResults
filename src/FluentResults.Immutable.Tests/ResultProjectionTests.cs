using FluentResults.Immutable.Tests.Extensions;
using FsCheck;
using FsCheck.Xunit;
using static FluentAssertions.FluentActions;
using static FluentResults.Immutable.Tests.Generators.IntegerResultGenerator;

namespace FluentResults.Immutable.Tests;

public sealed class ResultProjectionTests
{
    private static Result<Unit> SuccessfulResult => Result.Ok();

    private static Result<int> SuccessfulResultWithNoValue => Result.Ok<int>();

    [Fact(DisplayName = "No-op bind on a successful result of unit should return a result of unit")]
    public void NoOpBindOnSuccessfulResultShouldReturnAUnit() =>
        SuccessfulResult.Select(static () => Result.Ok())
            .Should()
            .Be(SuccessfulResult);

    [Fact(DisplayName = "No-op Task bind on a successful result of unit should return a result of unit")]
    public async Task NoOpAsynchronousBindOnSuccessfulResultShouldReturnAUnit()
    {
        var resultOfANoOpBind = await SuccessfulResult.SelectAsync(static () => Task.FromResult(Result.Ok()));

        resultOfANoOpBind.Should()
            .Be(SuccessfulResult);
    }

    [Fact(DisplayName = "No-op ValueTask bind on a successful result of unit should return a result of unit")]
    public async Task NoOpAsynchronousValueTaskBindOnSuccessfulResultShouldReturnAUnit()
    {
        var resultOfANoOpBind =
            await SuccessfulResult.SelectAsync(static () => new ValueTask<Result<Unit>>(Result.Ok()));

        resultOfANoOpBind.Should()
            .Be(SuccessfulResult);
    }

    [Fact(DisplayName = "No-op bind on a successful result with no value should return the same result")]
    public void NoOpBindOnASuccessfulResultWithNoValueReturnsTheSameResult() =>
        SuccessfulResultWithNoValue.Select(
                static _ => throw new InvalidOperationException(
                    "Binding on a result with value where there should be none!"),
                static () => Result.Ok<int>())
            .Should()
            .Be(SuccessfulResultWithNoValue);

    [Fact(DisplayName = "No-op Task bind on a successful result with no value should return the same result")]
    public async Task NoOpAsyncTaskBindOnASuccessfulResultWithoutValueReturnsTheSameResult() =>
        (await Awaiting(
                static () => SuccessfulResultWithNoValue.SelectAsync(
                    static _ => throw new InvalidOperationException(
                        "Binding on a result with value where there should be none!"),
                    static () => Task.FromResult(Result.Ok<int>())))
            .Should()
            .NotThrowAsync())
        .Which
        .Should()
        .Be(SuccessfulResultWithNoValue);

    [Fact(DisplayName = "No-op ValueTask bind on a successful result with no value should return the same result")]
    public async Task NoOpAsyncValueTaskBindOnASuccessfulResultWithoutValueReturnsTheSameResult()
    {
        (await SuccessfulResultWithNoValue.SelectAsync(
                ThrowException,
                static () => new(Result.Ok<int>())))
            .Should()
            .Be(SuccessfulResultWithNoValue);

        static ValueTask<Result<int>> ThrowException(int _) =>
            throw new InvalidOperationException(
                "Binding on a result with value where there should be none!");
    }

    [Fact(DisplayName = "Bind on a failed result should return a new, equivalent result without executing the bind")]
    public void BindOnFailedResultShouldReturnANewEquivalentFailedResult()
    {
        const string errorMessage = "An error";
        var fail = Result.Fail(errorMessage);

        Invoking(() => fail.Select(ThrowException))
            .Should()
            .NotThrow()
            .Which
            .Should()
            .Match<Result<Unit>>(
                static r => r.IsAFailure &&
                    r.Value is None<Unit> &&
                    r.Errors.Single()
                        .Message ==
                    errorMessage);

        static Result<Unit> ThrowException() =>
            throw new InvalidOperationException("Synchronous bind on a failed result was executed");
    }

    [Fact(
        DisplayName =
            "Asynchronous Task bind on a failed result should return a new, equivalent result without executing the bind")]
    public async Task AsynchronousTaskBindOnFailedResultShouldReturnANewEquivalentFailedResult()
    {
        const string errorMessage = "An error";
        var fail = Result.Fail(errorMessage);

        (await Awaiting(() => fail.SelectAsync(ThrowException))
                .Should()
                .NotThrowAsync())
            .Which
            .Should()
            .Match<Result<Unit>>(
                static r => r.IsAFailure &&
                    r.Value is None<Unit> &&
                    r.Errors.Single()
                        .Message ==
                    errorMessage);

        static Task<Result<Unit>> ThrowException() =>
            throw new InvalidOperationException("Asynchronous Task bind on a failed result was executed");
    }

    [Fact(
        DisplayName =
            "Asynchronous ValueTask bind on a failed result should return a new, equivalent result without executing the bind")]
    public async Task AsynchronousValueTaskBindOnFailedResultShouldReturnANewEquivalentFailedResult()
    {
        const string errorMessage = "An error";
        var fail = Result.Fail(errorMessage);

        (await fail.SelectAsync(ThrowException))
            .Should()
            .Match<Result<Unit>>(
                static r => r.IsAFailure &&
                    r.Value is None<Unit> &&
                    r.Errors.Single()
                        .Message ==
                    errorMessage);

        static ValueTask<Result<Unit>> ThrowException() =>
            throw new InvalidOperationException("Asynchronous ValueTask bind on a failed result was executed");
    }

    [Property(DisplayName = "Bind on a successful result with value converts it")]
    public Property BindOnSuccessfulResultWithValueConvertsIt(int firstValue, int finalValue) =>
        (Result.Ok(firstValue)
                    .Select(_ => Result.Ok(finalValue))
                is { IsSuccessful: true, } r &&
            r.ValueMatches(i => i == finalValue))
        .ToProperty();

    [Fact(DisplayName = "Asynchronously binding on a successful result with a value converts it")]
    public async Task AsynchronouslyBindingWithATaskOnASuccessfulResultWithValueConvertsIt()
    {
        (await Result.Ok(1)
                .SelectAsync(Increment))
            .Should()
            .Match<Result<int>>(static r => r.ValueMatches(static i => i == 2));

        static Task<Result<int>> Increment(int i) => Task.FromResult(Result.Ok(i + 1));
    }

    [Fact(DisplayName = "Asynchronously binding with a ValueTask on a successful result with a value converts it")]
    public async Task AsynchronouslyBindingWithAValueTaskOnASuccessfulResultWithValueConvertsIt()
    {
        (await Result.Ok(1)
                .SelectAsync(Increment))
            .Should()
            .Match<Result<int>>(static r => r.ValueMatches(static i => i == 2));

        static ValueTask<Result<int>> Increment(int i) => new(Result.Ok(i + 1));
    }

    [Fact(DisplayName = "Binding on a result with no value should return a successful result with no value")]
    public void BindingOnASuccessfulResultWithoutValueShouldReturnAFallbackResult()
    {
        SuccessfulResultWithNoValue.Select(ThrowException)
            .Should()
            .Match<Result<Unit>>(static r => r.IsSuccessful && r.Value is None<Unit>);

        static Result<Unit> ThrowException(int _) =>
            throw new InvalidOperationException("Bind on a result with no value!");
    }

    [Fact(DisplayName = "Asynchronously binding on a result with no value using a Task should return a successful result with no value")]
    public async Task AsynchronouslyBindingOnASuccessfulResultWithoutValueWithATaskShouldReturnAFallbackResult()
    {
        (await Awaiting(static () => SuccessfulResultWithNoValue.SelectAsync(ThrowException))
                .Should()
                .NotThrowAsync())
            .Which
            .Should()
            .Match<Result<Unit>>(static r => r.IsSuccessful && r.Value is None<Unit>);

        static Task<Result<Unit>> ThrowException(int _) =>
            throw new InvalidOperationException("Bind on a result with no value!");
    }

    [Fact(DisplayName = "Asynchronously binding on a result with no value using a ValueTask should return a successful result with no value")]
    public async Task AsynchronouslyBindingOnASuccessfulResultWithoutValueWithAValueTaskShouldReturnAFallbackResult()
    {
        (await SuccessfulResultWithNoValue.SelectAsync(ThrowException))
            .Should()
            .Match<Result<Unit>>(static r => r.IsSuccessful && r.Value is None<Unit>);

        static ValueTask<Result<Unit>> ThrowException(int _) =>
            throw new InvalidOperationException("Bind on a result with no value!");
    }

    [Fact(DisplayName = "No-op binding on multiple results should return result of unit")]
    public void NoOpBindingOnMultipleResultsShouldReturnResultOfUnit() =>
        SuccessfulResult.SelectMany(
                static _ => Result.Ok(),
                static (_, _) => Result.Ok())
            .Should()
            .Be(SuccessfulResult);

    [Property(
        DisplayName = "Binds are associative",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindsAreAssociative() =>
        Prop.ForAll(
            Gen.Three(GetSuccessfulIntegerResultWithValueGenerator())
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third) = tuple;

                return first.SelectMany(
                            _ => second,
                            SumAsResult)
                        .SelectMany(
                            _ => third,
                            SumAsResult) ==
                    first.SelectMany(
                        _ => second,
                        _ => third,
                        static (
                            one,
                            two,
                            three) => Result.Ok(one + two + three));

                static Result<int> SumAsResult(int one, int two) => Result.Ok(one + two);
            });

    [Property(
        DisplayName = "Binding on two successful results should return result with a proper value",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnTwoSuccessfulResultsShouldReturnASuccessfulResultWithAProperValue(
        int firstValue,
        int secondValue)
    {
        var sum = firstValue + secondValue;

        return (Result.Ok(firstValue)
                    .SelectMany(
                        _ => Result.Ok(secondValue),
                        static (first, second) => Result.Ok(first + second)) is { IsSuccessful: true, } success &&
                success.ValueMatches(i => i == sum))
            .ToProperty();
    }

    [Property(
        DisplayName = "Binding on three successful results should return result with a proper value",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnThreeSuccessfulResultsShouldReturnASuccessfulResultWithAProperValue(
        int firstValue,
        int secondValue,
        int thirdValue)
    {
        var sum = firstValue + secondValue + thirdValue;

        return (Result.Ok(firstValue)
                    .SelectMany(
                        _ => Result.Ok(secondValue),
                        _ => Result.Ok(thirdValue),
                        static (
                            first,
                            second,
                            third) => Result.Ok(first + second + third)) is { IsSuccessful: true, } success &&
                success.ValueMatches(i => i == sum))
            .ToProperty();
    }

    [Property(
        DisplayName = "Binding on four successful results should return a result with a proper value",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnFourSuccessfulResultsShouldReturnASuccessfulResultWithAProperValue(
        int first,
        int second,
        int third,
        int fourth)
    {
        var sum = first + second + third + fourth;

        return (Result.Ok(first)
                    .SelectMany(
                        _ => Result.Ok(second),
                        _ => Result.Ok(third),
                        _ => Result.Ok(fourth),
                        static (
                            a,
                            b,
                            c,
                            d) => Result.Ok(a + b + c + d)) is { IsSuccessful: true, } success &&
                success.ValueMatches(i => i == sum))
            .ToProperty();
    }

    [Property(
        DisplayName = "Binding on five successful results should return a result with a proper value",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnFiveSuccessfulResultsShouldReturnASuccessfulResultWithAProperValue(
        int first,
        int second,
        int third,
        int fourth,
        int fifth)
    {
        var sum = first + second + third + fourth + fifth;

        return (Result.Ok(first)
                    .SelectMany(
                        _ => Result.Ok(second),
                        _ => Result.Ok(third),
                        _ => Result.Ok(fourth),
                        _ => Result.Ok(fifth),
                        static (
                            a,
                            b,
                            c,
                            d,
                            e) => Result.Ok(a + b + c + d + e)) is { IsSuccessful: true, } success &&
                success.ValueMatches(i => i == sum))
            .ToProperty();
    }

    [Property(
        DisplayName = "Binding on six successful results should return a result with a proper value",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnSixSuccessfulResultsShouldReturnASuccessfulResultWithAProperValue(
        int first,
        int second,
        int third,
        int fourth,
        int fifth,
        int sixth)
    {
        var sum = first + second + third + fourth + fifth + sixth;

        return (Result.Ok(first)
                    .SelectMany(
                        _ => Result.Ok(second),
                        _ => Result.Ok(third),
                        _ => Result.Ok(fourth),
                        _ => Result.Ok(fifth),
                        _ => Result.Ok(sixth),
                        static (
                            a,
                            b,
                            c,
                            d,
                            e,
                            f) => Result.Ok(a + b + c + d + e + f)) is { IsSuccessful: true, } success &&
                success.ValueMatches(i => i == sum))
            .ToProperty();
    }

    [Property(
        DisplayName = "Binding on two successful results when any number has no value should return a default fallback result",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnTwoSuccessfulResultsWhenAnyNumberHasNoValueShouldReturnDefaultFallbackResult() =>
        Prop.ForAll(
            Gen.Two(GetSuccessfulIntegerResultGenerator())
                .Where(
                    static tuple =>
                    {
                        var (first, second) = tuple;

                        return first.Value is None<int> || second.Value is None<int>;
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second) = tuple;

                return first.SelectMany(
                            _ => second,
                            static (
                                first,
                                second) => Result.Ok(first + second))
                        is { Value: None<int>, Reasons: var reasons, } &&
                    reasons.SequenceEqual(first.Reasons);
            });

    [Property(
        DisplayName = "Binding on three successful results when any number has no value should return a default fallback result",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnThreeSuccessfulResultsWhenAnyNumberHasNoValueShouldReturnDefaultFallbackResult() =>
        Prop.ForAll(
            Gen.Three(GetSuccessfulIntegerResultGenerator())
                .Where(
                    static tuple =>
                    {
                        var (first, second, third) = tuple;

                        return first.Value is None<int> || second.Value is None<int> || third.Value is None<int>;
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third) = tuple;

                return first.SelectMany(
                        _ => second,
                        _ => third,
                        static (
                            first,
                            second,
                            third) => Result.Ok(first + second + third))
                    is { Value: None<int>, Reasons: var reasons, } &&
                    reasons.SequenceEqual(first.Reasons);
            });

    [Property(
        DisplayName = "Binding on four successful results when any number has no value should return a default fallback result",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnFourSuccessfulResultsWhenAnyNumberHasNoValueShouldReturnDefaultFallbackResult() =>
        Prop.ForAll(
            Gen.Four(GetSuccessfulIntegerResultGenerator())
                .Where(
                    static tuple =>
                    {
                        var (first, second, third, fourth) = tuple;

                        return first.Value is None<int> ||
                            second.Value is None<int> ||
                            third.Value is None<int> ||
                            fourth.Value is None<int>;
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth) = tuple;

                return first.SelectMany(
                            _ => second,
                            _ => third,
                            _ => fourth,
                            static (
                                first,
                                second,
                                third,
                                fourth) => Result.Ok(first + second + third + fourth))
                        is { Value: None<int>, Reasons: var reasons, } &&
                    reasons.SequenceEqual(first.Reasons);
            });

    [Property(
        DisplayName = "Binding on five successful results when any number has no value should return a default fallback result",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnFiveSuccessfulResultsWhenAnyNumberHasNoValueShouldReturnDefaultFallbackResult() =>
        Prop.ForAll(
            Gen.Four(GetSuccessfulIntegerResultGenerator())
                .SelectMany(
                    static _ => GetSuccessfulIntegerResultGenerator(),
                    static (tuple, result) => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, result))
                .Where(
                    static tuple =>
                    {
                        var (first, second, third, fourth, fifth) = tuple;

                        return first.Value is None<int> ||
                            second.Value is None<int> ||
                            third.Value is None<int> ||
                            fourth.Value is None<int> ||
                            fifth.Value is None<int>;
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth, fifth) = tuple;

                return first.SelectMany(
                            _ => second,
                            _ => third,
                            _ => fourth,
                            _ => fifth,
                            static (
                                first,
                                second,
                                third,
                                fourth,
                                fifth) => Result.Ok(first + second + third + fourth + fifth))
                        is { Value: None<int>, Reasons: var reasons, } &&
                    reasons.SequenceEqual(first.Reasons);
            });

    [Property(
        DisplayName = "Binding on six successful results when any number has no value should return a default fallback result",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnSixSuccessfulResultsWhenAnyNumberHasNoValueShouldReturnDefaultFallbackResult() =>
        Prop.ForAll(
            Gen.Four(GetSuccessfulIntegerResultGenerator())
                .SelectMany(
                    static _ => Gen.Two(GetSuccessfulIntegerResultGenerator()),
                    static (tuple, tuple2) => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple2.Item1, tuple2.Item2))
                .Where(
                    static tuple =>
                    {
                        var (first, second, third, fourth, fifth, sixth) = tuple;

                        return first.Value is None<int> ||
                            second.Value is None<int> ||
                            third.Value is None<int> ||
                            fourth.Value is None<int> ||
                            fifth.Value is None<int> ||
                            sixth.Value is None<int>;
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth, fifth, sixth) = tuple;

                return first.SelectMany(
                            _ => second,
                            _ => third,
                            _ => fourth,
                            _ => fifth,
                            _ => sixth,
                            static (
                                first,
                                second,
                                third,
                                fourth,
                                fifth,
                                sixth) => Result.Ok(first + second + third + fourth + fifth + sixth))
                        is { Value: None<int>, Reasons: var reasons, } &&
                    reasons.SequenceEqual(first.Reasons);
            });

    [Property(
        DisplayName = "Binding on two successful results when any number has no value should return a provided fallback result",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnTwoSuccessfulResultsWhenOneHasNoValueShouldReturnAProvidedFallbackResult() =>
        Prop.ForAll(
            Gen.Two(GetSuccessfulIntegerResultGenerator())
                .Where(
                    static tuple =>
                    {
                        var (first, second) = tuple;

                        return first.Value is None<int> || second.Value is None<int>;
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second) = tuple;

                return first.SelectMany(
                        _ => second,
                        static (first, second) => Result.Ok(first + second),
                        Fallback) ==
                    Fallback();

                static Result<int> Fallback() => Result.Ok(0);
            });

    [Property(
        DisplayName = "Binding on three successful results when any number has no value should return a provided fallback result",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnThreeSuccessfulResultsWhenOneHasNoValueShouldReturnAProvidedFallbackResult() =>
        Prop.ForAll(
            Gen.Three(GetSuccessfulIntegerResultGenerator())
                .Where(
                    static tuple =>
                    {
                        var (first, second, third) = tuple;

                        return first.Value is None<int> || second.Value is None<int> || third.Value is None<int>;
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third) = tuple;

                return first.SelectMany(
                        _ => second,
                        _ => third,
                        static (first, second, third) => Result.Ok(first + second + third),
                        Fallback) ==
                    Fallback();

                static Result<int> Fallback() => Result.Ok(0);
            });

    [Property(
        DisplayName = "Binding on four successful results when any number has no value should return a provided fallback result",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnFourSuccessfulResultsWhenOneHasNoValueShouldReturnAProvidedFallbackResult() =>
        Prop.ForAll(
            Gen.Four(GetSuccessfulIntegerResultGenerator())
                .Where(
                    static tuple =>
                    {
                        var (first, second, third, fourth) = tuple;

                        return first.Value is None<int> || second.Value is None<int> || third.Value is None<int> || fourth.Value is None<int>;
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth) = tuple;

                return first.SelectMany(
                        _ => second,
                        _ => third,
                        _ => fourth,
                        static (first, second, third, fourth) => Result.Ok(first + second + third + fourth),
                        Fallback) ==
                    Fallback();

                static Result<int> Fallback() => Result.Ok(0);
            });

    [Property(
        DisplayName = "Binding on five successful results when any number has no value should return a provided fallback result",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnFiveSuccessfulResultsWhenOneHasNoValueShouldReturnAProvidedFallbackResult() =>
        Prop.ForAll(
            Gen.Four(GetSuccessfulIntegerResultGenerator())
                .SelectMany(
                    static _ => GetSuccessfulIntegerResultGenerator(),
                    static (tuple, result) => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, result))
                .Where(
                    static tuple =>
                    {
                        var (first, second, third, fourth, fifth) = tuple;

                        return first.Value is None<int> ||
                            second.Value is None<int> ||
                            third.Value is None<int> ||
                            fourth.Value is None<int> ||
                            fifth.Value is None<int>;
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth, fifth) = tuple;

                return first.SelectMany(
                        _ => second,
                        _ => third,
                        _ => fourth,
                        _ => fifth,
                        static (first, second, third, fourth, fifth) => Result.Ok(first + second + third + fourth + fifth),
                        Fallback) ==
                    Fallback();

                static Result<int> Fallback() => Result.Ok(0);
            });

    [Property(
        DisplayName = "Binding on six successful results when any number has no value should return a provided fallback result",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnSixSuccessfulResultsWhenOneHasNoValueShouldReturnAProvidedFallbackResult() =>
        Prop.ForAll(
            Gen.Four(GetSuccessfulIntegerResultGenerator())
                .SelectMany(
                    static _ => Gen.Two(GetSuccessfulIntegerResultGenerator()),
                    static (tuple, tuple2) => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple2.Item1, tuple2.Item2))
                .Where(
                    static tuple =>
                    {
                        var (first, second, third, fourth, fifth, sixth) = tuple;

                        return first.Value is None<int> ||
                            second.Value is None<int> ||
                            third.Value is None<int> ||
                            fourth.Value is None<int> ||
                            fifth.Value is None<int> ||
                            sixth.Value is None<int>;
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth, fifth, sixth) = tuple;

                return first.SelectMany(
                        _ => second,
                        _ => third,
                        _ => fourth,
                        _ => fifth,
                        _ => sixth,
                        static (first, second, third, fourth, fifth, sixth) => Result.Ok(first + second + third + fourth + fifth + sixth),
                        Fallback) ==
                    Fallback();

                static Result<int> Fallback() => Result.Ok(0);
            });

    [Property(
        DisplayName = "Binding on two results should return a failure if at least one of these results is a failure",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnTwoResultsShouldReturnAFailureIfAtLeastOneOfThemIsAFailure() =>
        Prop.ForAll(
            Gen.Two(GetIntegerResultGenerator())
                .Where(
                    static t =>
                    {
                        var (r1, r2) = t;

                        return r1.IsAFailure || r2.IsAFailure;
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second) = tuple;

                return first.SelectMany(
                        _ => second,
                        static (_, _) => Result.Ok())
                    .IsAFailure;
            });

    [Property(
        DisplayName = "Binding on three results should return a failure if at least one of these results is a failure",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnThreeResultsShouldReturnAFailureIfAtLeastOneOfThemIsAFailure() =>
        Prop.ForAll(
            Gen.Three(GetIntegerResultGenerator())
                .Where(
                    static t =>
                    {
                        var (r1, r2, r3) = t;

                        return new[]
                        {
                            r1,
                            r2,
                            r3,
                        }.Any(static r => r.IsAFailure);
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third) = tuple;

                return first.SelectMany(
                        _ => second,
                        _ => third,
                        static (
                            _,
                            _,
                            _) => Result.Ok())
                    .IsAFailure;
            });

    [Property(
        DisplayName = "Binding on four results should return a failure if at least one of these results is a failure",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnFourResultsShouldReturnAFailureIfAtLeastOneOfThemIsAFailure() =>
        Prop.ForAll(
            Gen.Four(GetIntegerResultGenerator())
                .Where(
                    static t =>
                    {
                        var (r1, r2, r3, r4) = t;

                        return new[]
                        {
                            r1,
                            r2,
                            r3,
                            r4,
                        }.Any(static r => r.IsAFailure);
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth) = tuple;

                return first.SelectMany(
                        _ => second,
                        _ => third,
                        _ => fourth,
                        static (
                            _,
                            _,
                            _,
                            _) => Result.Ok())
                    .IsAFailure;
            });

    [Property(
        DisplayName = "Binding on five results should return a failure if at least one of these results is a failure",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnFiveResultsShouldReturnAFailureIfAtLeastOneOfThemIsAFailure() =>
        Prop.ForAll(
            Gen.Four(GetIntegerResultGenerator())
                .SelectMany(
                    static _ => GetIntegerResultGenerator(),
                    static (tuple, result) => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, result))
                .Where(
                    static t =>
                    {
                        var (r1, r2, r3, r4, r5) = t;

                        return new[]
                        {
                            r1,
                            r2,
                            r3,
                            r4,
                            r5,
                        }.Any(static r => r.IsAFailure);
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth, fifth) = tuple;

                return first.SelectMany(
                        _ => second,
                        _ => third,
                        _ => fourth,
                        _ => fifth,
                        static (
                            _,
                            _,
                            _,
                            _,
                            _) => Result.Ok())
                    .IsAFailure;
            });

    [Property(
        DisplayName = "Binding on six results should return a failure if at least one of these results is a failure",
        MaxTest = 1000,
        StartSize = int.MaxValue / 2,
        EndSize = int.MaxValue)]
    public Property BindingOnSixResultsShouldReturnAFailureIfAtLeastOneOfThemIsAFailure() =>
        Prop.ForAll(
            Gen.Four(GetIntegerResultGenerator())
                .SelectMany(
                    static _ => Gen.Two(GetIntegerResultGenerator()),
                    static (tuple1, tuple2) =>
                    {
                        var ((first, second, third, fourth), (fifth, sixth)) = (tuple1, tuple2);

                        return (first, second, third, fourth, fifth, sixth);
                    })
                .Where(
                    static t =>
                    {
                        var (r1, r2, r3, r4, r5, r6) = t;

                        return new[]
                        {
                            r1,
                            r2,
                            r3,
                            r4,
                            r5,
                            r6,
                        }.Any(static r => r.IsAFailure);
                    })
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth, fifth, sixth) = tuple;

                return first.SelectMany(
                        _ => second,
                        _ => third,
                        _ => fourth,
                        _ => fifth,
                        _ => sixth,
                        static (
                            _,
                            _,
                            _,
                            _,
                            _,
                            _) => Result.Ok())
                    .IsAFailure;
            });
}