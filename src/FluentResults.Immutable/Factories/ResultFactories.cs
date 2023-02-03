using FluentResults.Immutable.Extensions;
using FluentResults.Immutable.Metadata;

namespace FluentResults.Immutable;

public static class Result
{
    public static Result<Unit> Ok() => new(Unit.Value);

    public static Result<Unit> Fail(string errorMessage) => Fail(new Error(errorMessage));

    public static Result<Unit> Fail(Error error) => new(error.Yield().ToList());

    public static Result<T> Ok<T>(T value) => new(value);

    public static Result<T> Ok<T, TReason>(
        T value,
        IReadOnlyCollection<TReason> reasons)
        where TReason : Reason =>
        new(value, reasons);

    public static Result<T> Fail<T>(string errorMessage) => Fail<T>(new Error(errorMessage));

    public static Result<T> Fail<T>(Error error) => new(error.Yield().ToList());
}
