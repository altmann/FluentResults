using FluentResults.Immutable.Contracts;
using FluentResults.Immutable.Extensions;
using FluentResults.Immutable.Metadata;

using static FluentResults.Immutable.Maybe;

namespace FluentResults.Immutable;

public partial record Result : IResult
{
    public bool IsFailed => Errors.Count > 0;

    public bool IsSuccess => !IsFailed;

    public ImmutableList<Reason> Reasons { get; internal init; }

    public ImmutableList<Error> Errors => GetReasonsOfType<Error>();

    public ImmutableList<Success> Successes => GetReasonsOfType<Success>();

    protected internal Result()
        : this(ImmutableList<Reason>.Empty)
    {
    }

    protected internal Result(IReadOnlyCollection<Reason> reasons)
    {
        Reasons = reasons.Flatten(
            r => r switch
            {
                Success { Successes.Count: > 0 } s => s.Successes.Cast<Reason>().ToImmutableList(),
                Error { Errors.Count: > 0 } e => e.Errors.Cast<Reason>().ToImmutableList(),
                _ => ImmutableList<Reason>.Empty,
            })
            .ToImmutableList();
    }

    public Result WithReason(Reason reason) =>
        reason switch
        {
            Error { Errors.Count: > 0 } e => WithError(e),
            Success { Successes.Count: > 0 } s => WithSuccess(s),
            _ => new(Reasons.Add(reason)),
        };

    public Result WithReasons(IEnumerable<Reason> reasons) =>
        reasons.Aggregate(
            this,
            static (resultSeed, reason) => resultSeed.WithReason(reason));

    public Result WithError(string errorMessage) =>
        WithReason(new Error(errorMessage));

    public Result WithError(Error error) =>
        WithReasons(error.Yield().Flatten(static e => e.Errors));

    public Result WithErrors(IEnumerable<string> errorMessages) =>
        WithReasons(
            errorMessages.Select(static message => new Error(message))
                .ToArray());

    public Result WithErrors(IEnumerable<Error> errors) =>
        WithReasons(errors);

    public Result WithSuccess(Success success) =>
        WithReasons(success.Yield().Flatten(static e => e.Successes));

    public Result WithSuccesses(IEnumerable<Success> successes) =>
        WithReasons(successes);

    public bool HasError<TError>(Func<TError, bool>? predicate)
        where TError : Error =>
        HasReason(predicate ?? (_ => true));

    public bool HasSuccess<TSuccess>(Func<TSuccess, bool>? predicate)
        where TSuccess : Success =>
        HasReason(predicate ?? (_ => true));

    public TResult Bind<TResult>(Func<TResult> bindingFunction)
        where TResult : Result, new() =>
        IsSuccess && bindingFunction() is { } bind
            ? bind with { Reasons = Reasons.AddRange(bind.Reasons) }
            : new TResult() { Reasons = Reasons };

    public async Task<TResult> Bind<TResult>(Func<Task<TResult>> bindingAsyncFunction)
        where TResult : Result, new() =>
        IsSuccess && await bindingAsyncFunction() is { } bind
            ? bind with { Reasons = Reasons.AddRange(bind.Reasons) }
            : new TResult() { Reasons = Reasons };

    public async ValueTask<TResult> Bind<TResult>(Func<ValueTask<TResult>> bindingAsyncFunction)
        where TResult : Result, new() =>
        IsSuccess && await bindingAsyncFunction() is { } bind
            ? bind with { Reasons = Reasons.AddRange(bind.Reasons) }
            : new TResult() { Reasons = Reasons };

    private ImmutableList<TReason> GetReasonsOfType<TReason>()
        where TReason : Reason, IEquatable<TReason> =>
        Reasons.OfType<TReason>().ToImmutableList();

    private bool HasReason<TReason>(Func<TReason, bool> predicate)
        where TReason : Reason =>
        Reasons.OfType<TReason>()
            .SelectMany(
                static r => r switch
                {
                    Success { Successes.Count: > 0 } s =>
                        s.Yield()
                            .Flatten(static s => s.Successes)
                            .Cast<TReason>(),
                    Error { Errors.Count: > 0 } e => 
                        e.Yield()
                            .Flatten(static s => s.Errors)
                            .Cast<TReason>(),
                    _ => r.Yield(),
                })
            .Any(predicate);
}

public partial record Result<T> : Result
{
    protected internal Result()
        : base()
    {
        Value = None<T>();
    }

    protected internal Result(IReadOnlyCollection<Reason> reasons)
        : base(reasons)
    {
        Value = None<T>();
    }

    protected internal Result(T value)
        : base()
    {
        Value = Some(value);
    }

    protected internal Result(
        T value,
        IReadOnlyCollection<Reason> reasons)
        : base(reasons)
    {
        Value = Some(value);
    }

    public Maybe<T> Value { get; internal init; }

    public TResult Bind<TResult, TNewValue>(Func<T, TResult> bindingFunction)
        where TResult : Result<TNewValue>, new()
    {
        return IsSuccess ?
            Value.Match(
                bindingFunction,
                FallbackResult) :
            FallbackResult();

        TResult FallbackResult() =>
            new() { Reasons = Reasons };
    }

    public Task<TResult> Bind<TResult, TNewValue>(Func<T, Task<TResult>> asyncBindingFunction)
        where TResult : Result<TNewValue>, new()
    {
        return IsSuccess ?
            Value.Match(
                asyncBindingFunction,
                FallbackResult) :
            FallbackResult();

        Task<TResult> FallbackResult() =>
            Task.FromResult(new TResult() { Reasons = Reasons });
    }

    public ValueTask<TResult> Bind<TResult, TNewValue>(Func<T, ValueTask<TResult>> asyncBindingFunction)
        where TResult : Result<TNewValue>, new()
    {
        return IsSuccess ?
            Value.Match(
                asyncBindingFunction,
                FallbackResult) :
            FallbackResult();

        ValueTask<TResult> FallbackResult() =>
            ValueTask.FromResult(new TResult() { Reasons = Reasons });
    }
}
