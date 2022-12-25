using FluentResults.Immutable.Extensions;
using FluentResults.Immutable.Metadata;

namespace FluentResults.Immutable;

public partial record Result
{
    public static Result Ok() => new();

    public static Result Fail(string errorMessage) => Fail(new Error(errorMessage));

    public static Result Fail(Error error) => new(error.Yield().ToList());

    public static Result<T> Ok<T>(T value) => new(value);

    public static Result<T> Ok<T, TReason>(
        T value,
        IReadOnlyCollection<TReason> reasons)
        where TReason : Reason =>
        new(value, reasons);

    public static Result<T> Fail<T>(string errorMessage) => Fail<T>(new Error(errorMessage));

    public static Result<T> Fail<T>(Error error) => new(error.Yield().ToList());
}
