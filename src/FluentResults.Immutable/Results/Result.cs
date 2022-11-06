using System.Collections.Immutable;
using FluentResults.Immutable.Extensions;
using FluentResults.Immutable.Results.Contracts;
using FluentResults.Immutable.Results.Metadata;

namespace FluentResults.Immutable.Results;

public readonly partial record struct Result : IResult
{
    private readonly ImmutableList<Reason>? reasons;

    public bool IsFailed => Errors.Count > 0;

    public bool IsSuccess => !IsFailed;

    public ImmutableList<Reason> Reasons => reasons ?? ImmutableList<Reason>.Empty;

    public ImmutableList<Error> Errors => GetReasonsOfType<Error>();

    public ImmutableList<Success> Successes => GetReasonsOfType<Success>();

    public Result()
        : this(Array.Empty<Reason>())
    {
    }

    internal Result(IReadOnlyCollection<Reason> reasons)
    {
        this.reasons = reasons.Any() ? reasons.ToImmutableList() : null;
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

    private ImmutableList<TReason> GetReasonsOfType<TReason>()
        where TReason : Reason, IEquatable<TReason> =>
        Reasons.OfType<TReason>().ToImmutableList();
}
