using FluentResults.Immutable.Contracts;
using FluentResults.Immutable.Extensions;
using FluentResults.Immutable.Metadata;
using static FluentResults.Immutable.Maybe;

namespace FluentResults.Immutable;

public readonly record struct Result<T> : IImmutableResult
{
    public Result()
        : this(None<T>(), ImmutableList<Reason>.Empty)
    {
    }

    internal Result(IEnumerable<Reason> reasons)
        : this(None<T>(), reasons)
    {
    }

    internal Result(T value)
        : this(value, ImmutableList<Reason>.Empty)
    {
    }

    internal Result(
        T value,
        IEnumerable<Reason> reasons)
        : this(Some(value), reasons)
    {
    }

    private Result(
        Maybe<T> value,
        IEnumerable<Reason> reasons)
    {
        Value = value;
        Reasons = reasons.Flatten(
                static r => r switch
                {
                    Success { Successes.Count: > 0, } s => s.Successes.Cast<Reason>()
                        .ToImmutableList(),
                    Error { Errors.Count: > 0, } e => e.Errors.Cast<Reason>()
                        .ToImmutableList(),
                    _ => ImmutableList<Reason>.Empty,
                })
            .ToImmutableList();
    }

    public bool IsFailed => Errors.Count > 0;

    public bool IsSuccess => !IsFailed;

    public ImmutableList<Error> Errors => GetReasonsOfType<Error>();

    public ImmutableList<Success> Successes => GetReasonsOfType<Success>();

    public ImmutableList<Reason> Reasons { get; internal init; }

    public Maybe<T> Value { get; internal init; }

    public Result<T> WithReason(Reason reason) => new(Value, Reasons.Add(reason));

    public Result<T> WithReasons(IEnumerable<Reason> reasons) =>
        reasons.Aggregate(
            this,
            static (resultSeed, reason) => resultSeed.WithReason(reason));

    public Result<T> WithError(string errorMessage) => WithReason(new Error(errorMessage));

    public Result<T> WithError(Error error) =>
        WithReasons(
            error.Yield()
                .Flatten(static e => e.Errors));

    public Result<T> WithErrors(IEnumerable<string> errorMessages) =>
        WithReasons(
            errorMessages.Select(static message => new Error(message))
                .ToArray());

    public Result<T> WithErrors(IEnumerable<Error> errors) => WithReasons(errors);

    public Result<T> WithSuccess(Success success) =>
        WithReasons(
            success.Yield()
                .Flatten(static e => e.Successes));

    public Result<T> WithSuccesses(IEnumerable<Success> successes) => WithReasons(successes);

    public bool HasError<TError>(Func<TError, bool>? predicate = null)
        where TError : Error =>
        HasReason(predicate ?? (static _ => true));

    public bool HasSuccess<TSuccess>(Func<TSuccess, bool>? predicate = null)
        where TSuccess : Success =>
        HasReason(predicate ?? (static _ => true));

    public Result<TNewValue> Bind<TNewValue>(Func<T, Result<TNewValue>> bindingFunction)
    {
        Result<TNewValue> fallback = new()
        {
            Reasons = Reasons,
        };

        return IsSuccess
            ? Value.Match(
                bindingFunction,
                () => fallback)
            : fallback;
    }

    public Task<Result<TNewValue>> Bind<TNewValue>(Func<T, Task<Result<TNewValue>>> asyncBindingFunction)
    {
        var fallback = Task.FromResult<Result<TNewValue>>(
            new()
            {
                Reasons = Reasons,
            });

        return IsSuccess
            ? Value.Match(
                asyncBindingFunction,
                () => fallback)
            : fallback;
    }

    public ValueTask<Result<TNewValue>> Bind<TNewValue>(Func<T, ValueTask<Result<TNewValue>>> asyncBindingFunction)
    {
        var fallback = new ValueTask<Result<TNewValue>>(
            new Result<TNewValue>
            {
                Reasons = Reasons,
            });

        return IsSuccess
            ? Value.Match(
                asyncBindingFunction,
                () => fallback)
            : fallback;
    }

    private ImmutableList<TReason> GetReasonsOfType<TReason>()
        where TReason : Reason, IEquatable<TReason> =>
        Reasons.OfType<TReason>()
            .ToImmutableList();

    private bool HasReason<TReason>(Func<TReason, bool> predicate)
        where TReason : Reason =>
        Reasons.OfType<TReason>()
            .SelectMany(
                static r => r switch
                {
                    Success { Successes.Count: > 0, } s =>
                        s.Yield()
                            .Flatten(static s => s.Successes)
                            .Cast<TReason>(),
                    Error { Errors.Count: > 0, } e =>
                        e.Yield()
                            .Flatten(static s => s.Errors)
                            .Cast<TReason>(),
                    _ => r.Yield(),
                })
            .Any(predicate);
}