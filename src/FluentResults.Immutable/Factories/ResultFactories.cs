using FluentResults.Immutable.Contracts;
using FluentResults.Immutable.Extensions;
using FluentResults.Immutable.Metadata;

namespace FluentResults.Immutable;

public readonly record struct Result
{
    private static Func<Exception, Error> DefaultCatchHandler => static e => new ExceptionalError(e);

    public static Result<Unit> Ok() => new(Unit.Value);

    public static Result<Unit> Fail(string errorMessage) => Fail(new Error(errorMessage));

    public static Result<Unit> Fail(Error error) =>
        new(
            error.Yield()
                .ToList());

    public static Result<Unit> Fail(IEnumerable<Error> errors) => new(errors);

    public static Result<T> Ok<T>(T value) => new(value);

    public static Result<T> Ok<T>(
        T value,
        IEnumerable<Success> reasons) =>
        new(value, reasons);

    public static Result<T> Fail<T>(string errorMessage) => Fail<T>(new Error(errorMessage));

    public static Result<T> Fail<T>(Error error) =>
        new(
            error.Yield()
                .ToList());

    public static Result<IEnumerable<T>> Merge<T>(params Result<T>[] results) =>
        Merge<T, IImmutableResult<T>>(
            results.Cast<IImmutableResult<T>>()
                .ToList());

    public static Result<IEnumerable<T>> Merge<T, TResult>(IReadOnlyCollection<TResult> results)
        where TResult : IImmutableResult<T>
    {
        return new(
            results.Where(static r => r is { IsSuccessful: true, Value: Some<T>, })
                .Select(static r => r.Value)
                .Cast<Some<T>>()
                .Select(static s => s.Value),
            results.SelectMany(static r => r.Reasons));
    }

    public static Result<T> Try<T>(
        Func<T> func,
        Func<Exception, Error>? catchHandler = null)
    {
        catchHandler ??= DefaultCatchHandler;

        try
        {
            return Ok(func());
        }
        catch (Exception e)
        {
            return Fail<T>(catchHandler(e));
        }
    }

    public static async Task<Result<T>> Try<T>(
        Func<Task<T>> asyncFunc,
        Func<Exception, Error>? catchHandler = null)
    {
        catchHandler ??= DefaultCatchHandler;

        try
        {
            var result = await asyncFunc();
            return Ok(result);
        }
        catch (Exception e)
        {
            return Fail<T>(catchHandler(e));
        }
    }

    public static async ValueTask<Result<T>> Try<T>(
        Func<ValueTask<T>> asyncFunc,
        Func<Exception, Error>? catchHandler = null)
    {
        catchHandler ??= DefaultCatchHandler;

        try
        {
            var result = await asyncFunc();
            return Ok(result);
        }
        catch (Exception e)
        {
            return Fail<T>(catchHandler(e));
        }
    }
}