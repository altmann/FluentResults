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
    /// <returns>
    ///     A <see cref="Task{T}" />, representing the result of an asynchronous
    ///     operation, wrapping a <see cref="Result{T}" />, which contains
    ///     <typeparamref name="TNew" /> instance.
    /// </returns>
    public Task<Result<TNew>> SelectAsync<TNew>(Func<T, Task<Result<TNew>>> asyncBindingFunction)
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
    public async ValueTask<Result<TNew>> SelectAsync<TNew>(Func<ValueTask<Result<TNew>>> asyncSelector) =>
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
    public ValueTask<Result<TNew>> SelectAsync<TNew>(Func<T, ValueTask<Result<TNew>>> asyncBindingFunction)
    {
        var fallback = new ValueTask<Result<TNew>>(new Result<TNew>(Reasons));

        return IsSuccessful
            ? Value.Match(
                asyncBindingFunction,
                () => fallback)
            : fallback;
    }
}