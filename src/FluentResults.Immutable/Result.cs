using FluentResults.Immutable.Contracts;
using FluentResults.Immutable.Extensions;
using FluentResults.Immutable.Metadata;
using static FluentResults.Immutable.Option;

namespace FluentResults.Immutable;

/// <summary>
///     A structure representing the result of an operation.
/// </summary>
/// <typeparam name="T">Type of the value associated with this <see cref="Result{T}" />.</typeparam>
public readonly record struct Result<T> : IImmutableResult<T>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Result{T}" /> structure.
    /// </summary>
    public Result()
        : this(None<T>(), Enumerable.Empty<Reason>())
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result{T}" /> structure.
    /// </summary>
    /// <param name="reasons">
    ///     A collection of <see cref="Reason" />s
    ///     to associate with the <see cref="Result{T}" />.
    /// </param>
    internal Result(IEnumerable<Reason> reasons)
        : this(None<T>(), reasons)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result{T}" /> structure.
    /// </summary>
    /// <param name="value">
    ///     A <typeparamref name="T" />
    ///     to associate with the <see cref="Result{T}" />.
    /// </param>
    internal Result(T value)
        : this(value, Enumerable.Empty<Reason>())
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result{T}" /> structure.
    /// </summary>
    /// <param name="value">
    ///     A <typeparamref name="T" />
    ///     to associate with the <see cref="Result{T}" />.
    /// </param>
    /// <param name="reasons">
    ///     A collection of <see cref="Reason" />s
    ///     to associate with the <see cref="Result{T}" />.
    /// </param>
    internal Result(
        T value,
        IEnumerable<Reason> reasons)
        : this(Some(value), reasons)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Result{T}" /> structure.
    /// </summary>
    /// <param name="value">
    ///     A <see cref="Option{T}" />,
    ///     which will be assigned as <see cref="Value" />.
    /// </param>
    /// <param name="reasons">
    ///     A collection of <see cref="Reason" />s
    ///     to associate with the <see cref="Result{T}" />.
    /// </param>
    private Result(
        Option<T> value,
        IEnumerable<Reason> reasons)
    {
        Value = value;
        Reasons = reasons.ToImmutableList();
    }

    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="Result{T}" />
    ///     represents a failed operation.
    /// </summary>
    public bool IsAFailure => Errors.Any();

    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="Result{T}" />
    ///     represents a successful operation.
    /// </summary>
    public bool IsSuccessful => !IsAFailure;

    /// <summary>
    ///     Gets an <see cref="ImmutableList{T}" /> of <see cref="Error" />s
    ///     associated with this <see cref="Result{T}" />.
    /// </summary>
    /// <remarks>
    ///     This list will be empty for successful results.
    /// </remarks>
    public ImmutableList<Error> Errors => GetReasonsOfType<Error>();

    /// <summary>
    ///     Gets an <see cref="ImmutableList{T}" /> of
    ///     <see cref="Success" />es associated with this
    ///     <see cref="Result{T}" />.
    /// </summary>
    public ImmutableList<Success> Successes => GetReasonsOfType<Success>();

    /// <summary>
    ///     Gets an <see cref="ImmutableList{T}" /> of all
    ///     <see cref="Reason" />s associated with this
    ///     <see cref="Result{T}" />.
    /// </summary>
    /// <remarks>
    ///     A <see cref="Reason" /> is either a <see cref="Success" />,
    ///     <see cref="Error" />, or any other inheritor.
    /// </remarks>
    public ImmutableList<Reason> Reasons { get; internal init; }

    /// <summary>
    ///     Gets the value of the performed operation,
    ///     represented as <see cref="Option{T}" />.
    /// </summary>
    /// <remarks>
    ///     Failed results will have <see cref="None{T}" />, while successful ones
    ///     will return <see cref="Some{T}" />.
    ///     To safely access the result value of an operation,
    ///     <see cref="Option{T}.Match" /> overloads should be used.
    ///     Keep in mind that custom implementations of <see cref="Option" />
    ///     record and its inheritors are currently not supported.
    /// </remarks>
    public Option<T> Value { get; internal init; }

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with a provided <paramref name="reason" />.
    /// </summary>
    /// <param name="reason">
    ///     A <see cref="Reason" /> to add.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with provided <paramref name="reason" />.
    /// </returns>
    public Result<T> WithReason(Reason reason) => new(Value, Reasons.Add(reason));

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with provided <paramref name="reasons" />.
    /// </summary>
    /// <param name="reasons">
    ///     A collection of <see cref="Reason" />s to add.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with provided <paramref name="reasons" />.
    /// </returns>
    public Result<T> WithReasons(IEnumerable<Reason> reasons) =>
        reasons.Aggregate(
            this,
            static (resultSeed, reason) => resultSeed.WithReason(reason));

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with an <see cref="Error" />,
    ///     containing provided <paramref name="errorMessage" />.
    /// </summary>
    /// <param name="errorMessage">
    ///     Message to provide to an <see cref="Error" />.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with an <see cref="Error" />,
    ///     containing provided <paramref name="errorMessage" />.
    /// </returns>
    public Result<T> WithError(string errorMessage) => WithReason(new Error(errorMessage));

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with a provided <paramref name="error" />.
    /// </summary>
    /// <param name="error">
    ///     An <see cref="Error" /> to add.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with provided <paramref name="error" />.
    /// </returns>
    public Result<T> WithError(Error error) =>
        WithReasons(
            error.Yield()
                .Flatten(static e => e.Errors));

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with <see cref="Error" />s
    ///     built from provided <paramref name="errorMessages" />.
    /// </summary>
    /// <param name="errorMessages">
    ///     A collection of error messages to build <see cref="Error" />
    ///     instances from.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with <see cref="Error" />s,
    ///     built from <paramref name="errorMessages" />.
    /// </returns>
    public Result<T> WithErrors(IEnumerable<string> errorMessages) =>
        WithReasons(
            errorMessages.Select(static message => new Error(message))
                .ToArray());

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with provided <paramref name="errors" />.
    /// </summary>
    /// <param name="errors">
    ///     A collection of <see cref="Error" />s to add.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with provided <paramref name="errors" />.
    /// </returns>
    public Result<T> WithErrors(IEnumerable<Error> errors) => WithReasons(errors);

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with a provided <paramref name="success" />.
    /// </summary>
    /// <param name="success">
    ///     A <see cref="Success" /> to add.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with provided <paramref name="success" />.
    /// </returns>
    public Result<T> WithSuccess(Success success) =>
        WithReasons(
            success.Yield()
                .Flatten(static e => e.Successes));

    /// <summary>
    ///     Creates a new <see cref="Result{T}" /> with provided <paramref name="successes" />.
    /// </summary>
    /// <param name="successes">
    ///     A collection of <see cref="Success" />es to add.
    /// </param>
    /// <returns>
    ///     A new <see cref="Result{T}" /> with provided <paramref name="successes" />.
    /// </returns>
    public Result<T> WithSuccesses(IEnumerable<Success> successes) => WithReasons(successes);

    /// <summary>
    ///     Returns a <see cref="bool" /> value indicating whether
    ///     this <see cref="Result{T}" /> instance contains a <typeparamref name="TError" />
    ///     matching provided <paramref name="predicate"/>
    /// </summary>
    /// <typeparam name="TError">
    ///     Generic type of the error.
    /// </typeparam>
    /// <param name="predicate">
    ///     A <see cref="Predicate{T}" /> to match.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if any of the <see cref="Errors" />
    ///     match provided <paramref name="predicate" />,
    ///     otherwise - <see langword="false" />.
    /// </returns>
    public bool HasError<TError>(Predicate<TError>? predicate = null)
        where TError : Error =>
        HasReason(predicate);

    /// <summary>
    ///     Returns a <see cref="bool" /> value indicating whether
    ///     this <see cref="Result{T}" /> instance contains a <typeparamref name="TSuccess" />
    ///     matching provided <paramref name="predicate"/>
    /// </summary>
    /// <typeparam name="TSuccess">
    ///     Generic type of the success.
    /// </typeparam>
    /// <param name="predicate">
    ///     A <see cref="Predicate{T}" /> to match.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if any of the <see cref="Successes" />
    ///     match provided <paramref name="predicate" />,
    ///     otherwise - <see langword="false" />.
    /// </returns>
    public bool HasSuccess<TSuccess>(Predicate<TSuccess>? predicate = null)
        where TSuccess : Success =>
        HasReason(predicate);

    /// <summary>
    ///     Returns a <see cref="bool" /> value indicating whether
    ///     this <see cref="Result{T}" /> instance contains a <typeparamref name="TReason" />
    ///     matching provided <paramref name="predicate"/>
    /// </summary>
    /// <typeparam name="TReason">
    ///     Generic type of the reason.
    /// </typeparam>
    /// <param name="predicate">
    ///     A <see cref="Predicate{T}" /> to match.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if any of the <see cref="Reasons" />
    ///     match provided <paramref name="predicate" />,
    ///     otherwise - <see langword="false" />.
    /// </returns>
    public bool HasReason<TReason>(Predicate<TReason>? predicate = null)
        where TReason : Reason =>
        Reasons.SelectMany(
                static r => r switch
                {
                    Success { Successes.Count: > 0, } s =>
                        s.Yield()
                            .Flatten(static s => s.Successes),
                    Error { Errors.Count: > 0, } e =>
                        e.Yield()
                            .Flatten(static s => s.Errors),
                    _ => r.Yield(),
                })
            .OfType<TReason>()
            .Any(r => predicate?.Invoke(r) ?? true);

    /// <summary>
    ///     Projects this <see cref="Result{T}" />
    ///     to a <see cref="Result{T}" /> of <typeparamref name="TNew" />
    ///     by executing <paramref name="selector" />
    ///     if this <see cref="Result{T}" /> is successful.
    /// </summary>
    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="selector">Bind delegate to execute.</param>
    /// <returns>
    ///     A <see cref="Result{T}" />, wrapping <typeparamref name="TNew" />
    ///     instance.
    /// </returns>
    /// <remarks>
    ///     This overload disregards the <see cref="Value" /> of this
    ///     <see cref="Result{T}" />.
    /// </remarks>
    public Result<TNew> Select<TNew>(Func<Result<TNew>> selector) =>
        IsSuccessful && selector() is var bind
            ? bind with
            {
                Reasons = Reasons.AddRange(bind.Reasons),
            }
            : new(Reasons);

    /// <summary>
    ///     Projects this <see cref="Result{T}" />
    ///     to a <see cref="Result{T}" /> of <typeparamref name="TNew" />
    ///     by executing <paramref name="bindingFunction" />
    ///     if this <see cref="Result{T}" /> is successful.
    /// </summary>
    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="bindingFunction">Bind delegate to execute.</param>
    /// <returns>
    ///     A <see cref="Result{T}" />, wrapping <typeparamref name="TNew" />
    ///     instance.
    /// </returns>
    public Result<TNew> Select<TNew>(Func<T, Result<TNew>> bindingFunction)
    {
        Result<TNew> fallback = new(Reasons);

        return IsSuccessful
            ? Value.Match(
                bindingFunction,
                () => fallback)
            : fallback;
    }

    /// <summary>
    ///     Projects this <see cref="Result{T}" />
    ///     to a <see cref="Result{T}" /> of <typeparamref name="TNew" />
    ///     by executing <paramref name="asyncSelector" /> asynchronously
    ///     if this <see cref="Result{T}" /> is successful.
    /// </summary>
    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="asyncSelector">Asynchronous delegate to execute.</param>
    /// <returns>
    ///     A <see cref="Task{T}" />, representing the result of an asynchronous
    ///     operation, wrapping a <see cref="Result{T}" />, which contains
    ///     <typeparamref name="TNew" /> instance.
    /// </returns>
    /// <remarks>
    ///     This overload disregards the <see cref="Value" /> of this
    ///     <see cref="Result{T}" />.
    /// </remarks>
    public async Task<Result<TNew>> Select<TNew>(Func<Task<Result<TNew>>> asyncSelector) =>
        IsSuccessful && await asyncSelector() is var bind
            ? bind with
            {
                Reasons = Reasons.AddRange(bind.Reasons),
            }
            : new(Reasons);

    /// <summary>
    ///     Projects this <see cref="Result{T}" />
    ///     to a <see cref="Result{T}" /> of <typeparamref name="TNew" />
    ///     by executing <paramref name="asyncBindingFunction" /> asynchronously
    ///     if this <see cref="Result{T}" /> is successful.
    /// </summary>
    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="asyncBindingFunction">Asynchronous delegate to execute.</param>
    /// <returns>
    ///     A <see cref="Task{T}" />, representing the result of an asynchronous
    ///     operation, wrapping a <see cref="Result{T}" />, which contains
    ///     <typeparamref name="TNew" /> instance.
    /// </returns>
    public Task<Result<TNew>> Select<TNew>(Func<T, Task<Result<TNew>>> asyncBindingFunction)
    {
        var fallback = Task.FromResult<Result<TNew>>(new(Reasons));

        return IsSuccessful
            ? Value.Match(
                asyncBindingFunction,
                () => fallback)
            : fallback;
    }

    /// <summary>
    ///     Projects this <see cref="Result{T}" />
    ///     to a <see cref="Result{T}" /> of <typeparamref name="TNew" />
    ///     by executing <paramref name="asyncSelector" /> asynchronously
    ///     if this <see cref="Result{T}" /> is successful.
    /// </summary>
    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="asyncSelector">Asynchronous delegate to execute.</param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous
    ///     operation, wrapping a <see cref="Result{T}" />, which contains
    ///     <typeparamref name="TNew" /> instance.
    /// </returns>
    /// <remarks>
    ///     This overload disregards the <see cref="Value" /> of this
    ///     <see cref="Result{T}" />.
    /// </remarks>
    public async ValueTask<Result<TNew>> Select<TNew>(Func<ValueTask<Result<TNew>>> asyncSelector) =>
        IsSuccessful && await asyncSelector() is var bind
            ? bind with
            {
                Reasons = Reasons.AddRange(bind.Reasons),
            }
            : new(Reasons);

    /// <summary>
    ///     Projects this <see cref="Result{T}" />
    ///     to a <see cref="Result{T}" /> of <typeparamref name="TNew" />
    ///     by executing <paramref name="asyncBindingFunction" /> asynchronously
    ///     if this <see cref="Result{T}" /> is successful.
    /// </summary>
    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="asyncBindingFunction">Asynchronous delegate to execute.</param>
    /// <returns>
    ///     A <see cref="Task{T}" />, representing the result of an asynchronous
    ///     operation, wrapping a <see cref="Result{T}" />, which contains
    ///     <typeparamref name="TNew" /> instance.
    /// </returns>
    public ValueTask<Result<TNew>> Select<TNew>(Func<T, ValueTask<Result<TNew>>> asyncBindingFunction)
    {
        var fallback = new ValueTask<Result<TNew>>(new Result<TNew>(Reasons));

        return IsSuccessful
            ? Value.Match(
                asyncBindingFunction,
                () => fallback)
            : fallback;
    }

    private ImmutableList<TReason> GetReasonsOfType<TReason>()
        where TReason : Reason, IEquatable<TReason> =>
        Reasons.OfType<TReason>()
            .ToImmutableList();
}