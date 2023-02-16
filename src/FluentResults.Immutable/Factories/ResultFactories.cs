using FluentResults.Immutable.Contracts;
using FluentResults.Immutable.Extensions;
using FluentResults.Immutable.Metadata;

namespace FluentResults.Immutable;

public readonly record struct Result
{
    public static Result<Unit> Ok() => new(Unit.Value);

    public static Result<Unit> Fail(string errorMessage) => Fail(new Error(errorMessage));

    public static Result<Unit> Fail(Error error) => new(error.Yield().ToList());

    public static Result<Unit> Fail(IEnumerable<Error> errors) => new(errors);

    public static Result<T> Ok<T>(T value) => new(value);

    public static Result<T> Ok<T>(
        T value,
        IEnumerable<Success> reasons) =>
        new(value, reasons);

    public static Result<T> Fail<T>(string errorMessage) => Fail<T>(new Error(errorMessage));

    public static Result<T> Fail<T>(Error error) => new(error.Yield().ToList());

    public static Result<IEnumerable<T>> Merge<T>(params IImmutableResult<T>[] results) =>
        Merge(results.AsEnumerable());

    public static Result<IEnumerable<T>> Merge<T>(IEnumerable<IImmutableResult<T>> results)
    {
        var resultList = results.ToList();

        return new(
            resultList.Where(static r => r is { IsSuccessful: true, Value: Some<T>, })
                .Select(static r => r.Value)
                .Cast<Some<T>>()
                .Select(static s => s.Value),
            resultList.SelectMany(static r => r.Reasons));
    }
}
