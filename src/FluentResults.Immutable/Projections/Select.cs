namespace FluentResults.Immutable;

public readonly partial record struct Result<T>
{
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
    /// <param name="noneBindingFunction">
    ///     Bind delegate to execute if the <see cref="Result{T}.Value" />
    ///     is <see cref="None{T}" />; defaults to returning a new result with
    ///     changed generic type to <typeparamref name="TNew" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Result{T}" />, wrapping <typeparamref name="TNew" />
    ///     instance.
    /// </returns>
    public Result<TNew> Select<TNew>(
        Func<T, Result<TNew>> bindingFunction,
        Func<Result<TNew>>? noneBindingFunction = null)
    {
        Result<TNew> fallback = new(Reasons);
        noneBindingFunction ??= () => fallback;
        
        return IsSuccessful
            ? Value.Match(
                bindingFunction,
                noneBindingFunction)
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
    public async Task<Result<TNew>> SelectAsync<TNew>(Func<Task<Result<TNew>>> asyncSelector) =>
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
    /// <param name="asyncNoneBindingFunction">
    ///     Asynchronous delegate to execute if the <see cref="Result{T}.Value" /> is
    ///     <see cref="None{T}" />; defaults to returning a new result
    ///     with changed generic parameter to <typeparamref name="TNew" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{T}" />, representing the result of an asynchronous
    ///     operation, wrapping a <see cref="Result{T}" />, which contains
    ///     <typeparamref name="TNew" /> instance.
    /// </returns>
    public Task<Result<TNew>> SelectAsync<TNew>(
        Func<T, Task<Result<TNew>>> asyncBindingFunction,
        Func<Task<Result<TNew>>>? asyncNoneBindingFunction = null)
    {
        var fallback = Task.FromResult<Result<TNew>>(new(Reasons));
        asyncNoneBindingFunction ??= () => fallback;

        return IsSuccessful
            ? Value.Match(
                asyncBindingFunction,
                asyncNoneBindingFunction)
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
    public async ValueTask<Result<TNew>> SelectAsync<TNew>(Func<ValueTask<Result<TNew>>> asyncSelector) =>
        IsSuccessful && await asyncSelector() is var bind
            ? bind with
            {
                Reasons = Reasons.AddRange(bind.Reasons),
            }
            : new(Reasons);

    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <param name="asyncNoneBindingFunction">
    ///     Asynchronous delegate to execute if the <see cref="Result{T}.Value" /> is
    ///     <see cref="None{T}" />; defaults to returning a new result
    ///     with changed generic parameter to <typeparamref name="TNew" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous
    ///     operation, wrapping a <see cref="Result{T}" />, which contains
    ///     <typeparamref name="TNew" /> instance.
    /// </returns>
    /// <inheritdoc cref="SelectAsync{TNew}(Func{T,Task{Result{TNew}}}, Func{Task{Result{TNew}}})"/>
    public ValueTask<Result<TNew>> SelectAsync<TNew>(
        Func<T, ValueTask<Result<TNew>>> asyncBindingFunction,
        Func<ValueTask<Result<TNew>>>? asyncNoneBindingFunction = null)
    {
        var fallback = new ValueTask<Result<TNew>>(new Result<TNew>(Reasons));
        asyncNoneBindingFunction ??= () => fallback;

        return IsSuccessful
            ? Value.Match(
                asyncBindingFunction,
                asyncNoneBindingFunction)
            : fallback;
    }
}