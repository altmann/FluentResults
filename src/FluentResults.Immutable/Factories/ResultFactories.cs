using FluentResults.Immutable.Contracts;
using FluentResults.Immutable.Extensions;
using FluentResults.Immutable.Metadata;

namespace FluentResults.Immutable;

public readonly record struct Result
{
    /// <summary>
    ///     Represents a succesful operation.
    /// </summary>
    /// <returns>
    ///     A successful result, wrapping a <see cref="Unit" />.
    /// </returns>
    /// <remarks>
    ///     This method should be used by all operations
    ///     which are supposed to return <see cref="void" />.
    /// </remarks>
    public static Result<Unit> Ok() => new(Unit.Value);

    /// <summary>
    ///     Represents a succesful operation.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="value">Value to associate with successful result.</param>
    /// <returns>
    ///     A successful result, wrapping provided <paramref name="value" />.
    /// </returns>
    /// <remarks></remarks>
    public static Result<T> Ok<T>(T value) => new(value);

    /// <inheritdoc cref="Ok{T}(T)"/>
    /// <param name="successes">
    ///     A collection of <see cref="Success" />es to associate
    ///     with the returned <see cref="Result{T}" />.
    /// </param>
    public static Result<T> Ok<T>(
        T value,
        IEnumerable<Success> successes) =>
        new(value, successes);

    /// <summary>
    ///     Creates a successful <see cref="Result{T}" />
    ///     of <see cref="Unit" /> if the <paramref name="condition" />
    ///     is <see langword="true" />, otherwise - failure is returned.
    /// </summary>
    /// <param name="condition">Condition to check.</param>
    /// <param name="errorMessage">An error message to associate with failed result.</param>
    /// <returns>
    ///     A <see cref="Result{T}" /> of <see cref="Unit" />,
    ///     dependent on the <paramref name="condition" />.
    /// </returns>
    public static Result<Unit> OkIf(bool condition, string errorMessage) => condition ? Ok() : Fail(errorMessage);

    /// <inheritdoc cref="OkIf(bool, string)"/>
    /// <param name="error">An <see cref="Error" /> to associate with failed result.</param>
    public static Result<Unit> OkIf(bool condition, Error error) => condition ? Ok() : Fail(error);

    /// <inheritdoc cref="OkIf(bool, string)"/>
    /// <param name="errorMessageFactory">
    ///     A <see cref="Func{T}" />, returning a <see cref="string" />
    ///     to be used to build an <see cref="Error" /> instance.
    /// </param>
    public static Result<Unit> OkIf(bool condition, Func<string> errorMessageFactory) => condition ? Ok() : Fail(errorMessageFactory());

    /// <inheritdoc cref="OkIf(bool, Error)"/>
    /// /// <param name="errorMessageFactory">
    ///     A <see cref="Func{T}" />, returning an <see cref="Error" />.
    /// </param>
    public static Result<Unit> OkIf(bool condition, Func<Error> errorFactory) => condition ? Ok() : Fail(errorFactory());

    /// <summary>
    ///     Represents a failed operation.
    /// </summary>
    /// <param name="errorMessage">
    ///     A message to be associated with a generic 
    ///     <see cref="Error" /> returned with the <see cref="Result{T}" />.
    /// </param>
    /// <returns>
    ///     A failed <see cref="Result{T}" /> of <see cref="Unit" />.
    /// </returns>
    public static Result<Unit> Fail(string errorMessage) => Fail(new Error(errorMessage));

    /// <inheritdoc cref="Fail(string)" />
    /// <param name="error">
    ///     An <see cref="Error" /> to be associated
    ///     with the failed <see cref="Result{T}" />.
    /// </param>
    public static Result<Unit> Fail(Error error) =>
        new(
            error.Yield()
                .ToList());

    /// <inheritdoc cref="Fail(Error)" />
    /// <param name="errors">
    ///     A collection of <see cref="Error" />s
    ///     to be associated with the failed <see cref="Result{T}" />.
    /// </param>
    public static Result<Unit> Fail(IEnumerable<Error> errors) => new(errors);

    /// <inheritdoc cref="Fail(string)" />
    /// <typeparam name="T">Expected type of the value.</typeparam>
    public static Result<T> Fail<T>(string errorMessage) => Fail<T>(new Error(errorMessage));

    /// <inheritdoc cref="Fail(Error)" />
    /// <typeparam name="T">Expected type of the value.</typeparam>
    public static Result<T> Fail<T>(Error error) =>
        new(
            error.Yield()
                .ToList());

    /// <inheritdoc cref="Fail(IEnumerable{Error})" />
    /// <typeparam name="T">Expected type of the value.</typeparam>
    public static Result<T> Fail<T>(IEnumerable<Error> errors) => new(errors);

    /// <summary>
    ///     Creates a failed <see cref="Result{T}" />
    ///     of <see cref="Unit" /> if the <paramref name="condition" />
    ///     is <see langword="true" />, otherwise - successful result
    ///     is returned.
    /// </summary>
    /// <param name="condition">Condition to check.</param>
    /// <param name="errorMessage">An error message to associate with failed result.</param>
    /// <returns>
    ///     A <see cref="Result{T}" /> of <see cref="Unit" />,
    ///     dependent on the <paramref name="condition" />.
    /// </returns>
    /// <seealso cref="FailIf(bool, Func{string})" />
    public static Result<Unit> FailIf(bool condition, string errorMessage) => condition ? Fail(errorMessage) : Ok();

    /// <inheritdoc cref="FailIf(bool, string)" />
    /// <param name="error">An <see cref="Error" /> to associate with failed result.</param>
    /// <seealso cref="FailIf(bool, Func{Error})" />
    public static Result<Unit> FailIf(bool condition, Error error) => condition ? Fail(error) : Ok();

    /// <inheritdoc cref="FailIf(bool, string)" />
    /// <param name="errorMessageFactory">
    ///     A <see cref="Func{T}" />, returning a <see cref="string" />,
    ///     which contains an error message to associate with failed
    ///     <see cref="Result{T}" />.
    /// </param>
    /// <remarks>
    ///     This overload should be used if
    ///     lazy evaluation of the error is required.
    /// </remarks>
    /// <seealso cref="FailIf(bool, string)" />
    public static Result<Unit> FailIf(bool condition, Func<string> errorMessageFactory) => condition ? Fail(errorMessageFactory()) : Ok();

    /// <inheritdoc cref="FailIf(bool, Func{string})" />
    /// <param name="errorFactory">
    ///     A <see cref="Func{T}" />, returning an <see cref="Error" />
    ///     which should be associated with a failed <see cref="Result{T}" />.
    /// </param>
    /// <seealso cref="FailIf(bool, Error)" />
    public static Result<Unit> FailIf(bool condition, Func<Error> errorFactory) => condition ? Fail(errorFactory()) : Ok();

    /// <summary>
    ///     Merges all <paramref name="results" />
    ///     into one <see cref="Result{T}" />,
    ///     aggregating the values into an <see cref="IEnumerable{T}" />.
    /// </summary>
    /// <typeparam name="T">Type of the values.</typeparam>
    /// <param name="results">
    ///     A collection of <see cref="Result{T}" />s to merge.
    /// </param>
    /// <returns>
    ///     A merged <see cref="Result{T}" />,
    ///     containing an aggregation of all
    ///     <see cref="Result{T}.Reasons" />
    ///     and <see cref="Result{T}.Value" />s.
    /// </returns>
    /// <remarks>
    ///     If any of the provided <paramref name="results" />
    ///     is a failure, then the merged result will be a failure
    ///     itself.
    /// </remarks>
    public static Result<IEnumerable<T>> Merge<T>(params Result<T>[] results) =>
        Merge<T, IImmutableResult<T>>(
            results.Cast<IImmutableResult<T>>()
                .ToList());

    /// <inheritdoc cref="Merge{T}(Result{T}[])"/>
    public static Result<IEnumerable<T>> Merge<T, TResult>(IReadOnlyCollection<TResult> results)
        where TResult : IImmutableResult<T> =>
        new(
            results.Where(static r => r is { IsSuccessful: true, Value: Some<T>, })
                .Select(static r => r.Value)
                .Cast<Some<T>>()
                .Select(static s => s.Value),
            results.SelectMany(static r => r.Reasons));

    /// <summary>
    ///     Attempts to perform a <paramref name="func" />;
    ///     if no exception is thrown, a successful
    ///     <see cref="Result{T}" /> is returned,
    ///     Otherwise - a failed <see cref="Result{T}" />
    ///     is returned, with an error instance built from
    ///     <paramref name="catchHandler" />.
    /// </summary>
    /// <typeparam name="T">
    ///     Type of the value to return from <paramref name="func" />.
    /// </typeparam>
    /// <param name="func">A delegate, representing a function which can throw.</param>
    /// <param name="catchHandler">
    ///     A delegate which returns an instance of an <see cref="Error" />
    ///     if an exception is thrown.
    /// </param>
    /// <returns>
    ///     Successful <see cref="Result{T}" /> if no exception was thrown,
    ///     otherwise - a failure.
    /// </returns>
    /// <remarks>
    ///     The default <paramref name="catchHandler" />
    ///     returns an <see cref="ExceptionalError" />,
    ///     which contains the thrown exception in the
    ///     <see cref="ExceptionalError.CausedBy" /> property.
    /// </remarks>
    public static Result<T> Try<T>(
        Func<T> func,
        Func<Exception, Error>? catchHandler = null)
    {
        catchHandler ??= ExceptionHandler;

        try
        {
            return Ok(func());
        }
        catch (Exception e)
        {
            return Fail<T>(catchHandler(e));
        }
    }

    /// <summary>
    ///     Attempts to perform a <paramref name="asyncFunc" />;
    ///     if no exception is thrown, a successful
    ///     <see cref="Result{T}" /> is returned,
    ///     Otherwise - a failed <see cref="Result{T}" />
    ///     is returned, with an error instance built from
    ///     <paramref name="catchHandler" />.
    /// </summary>
    /// <param name="asyncFunc">
    ///     An asynchronous delegate, represented by a 
    ///     <see cref="Func{TResult}"/> of a <see cref="Task{TResult}" />
    ///     to execute.
    /// </param>
    /// <inheritdoc cref="Try{T}(Func{T}, Func{Exception, Error}?)" />
    /// <seealso cref="Try{T}(Func{ValueTask{T}}, Func{Exception, Error}?)" />
    public static async Task<Result<T>> Try<T>(
        Func<Task<T>> asyncFunc,
        Func<Exception, Error>? catchHandler = null)
    {
        catchHandler ??= ExceptionHandler;

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

    /// <param name="asyncFunc">
    ///     An asynchronous delegate, represented by a 
    ///     <see cref="Func{TResult}"/> of a <see cref="ValueTask{TResult}" />
    ///     to execute.
    /// </param>
    /// <inheritdoc cref="Try{T}(Func{Task{T}}, Func{Exception, Error}?)" />
    /// <seealso cref="Try{T}(Func{Task{T}}, Func{Exception, Error}?)" />
    public static async ValueTask<Result<T>> Try<T>(
        Func<ValueTask<T>> asyncFunc,
        Func<Exception, Error>? catchHandler = null)
    {
        catchHandler ??= ExceptionHandler;

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

    private static Error ExceptionHandler(Exception exception) => new ExceptionalError(exception);
}