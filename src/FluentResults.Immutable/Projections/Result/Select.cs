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
        IsSuccessful && selector() is var boundResult
            ? boundResult with
            {
                Reasons = Reasons.AddRange(boundResult.Reasons),
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
    public Result<TNew> Select<TNew>(Func<IOption<T>, Result<TNew>> bindingFunction) =>
        IsSuccessful && bindingFunction(Value) is var boundResult
            ? boundResult with
            {
                Reasons = Reasons.AddRange(boundResult.Reasons),
            }
            : new(Reasons);

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
        IsSuccessful && await asyncSelector() is var boundResult
            ? boundResult with
            {
                Reasons = Reasons.AddRange(boundResult.Reasons),
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
    public async Task<Result<TNew>> SelectAsync<TNew>(
        Func<IOption<T>, Task<Result<TNew>>> asyncBindingFunction)
    {
        return IsSuccessful && await asyncBindingFunction(Value) is var boundResult
            ? boundResult with
            {
                Reasons = Reasons.AddRange(boundResult.Reasons),
            }
            : new(Reasons);
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
        IsSuccessful && await asyncSelector() is var boundResult
            ? boundResult with
            {
                Reasons = Reasons.AddRange(boundResult.Reasons),
            }
            : new(Reasons);

    /// <typeparam name="TNew">
    ///     Generic parameter of the new <see cref="Result{T}" />.
    /// </typeparam>
    /// <returns>
    ///     A <see cref="ValueTask{T}" />, representing the result of an asynchronous
    ///     operation, wrapping a <see cref="Result{T}" />, which contains
    ///     <typeparamref name="TNew" /> instance.
    /// </returns>
    /// <inheritdoc cref="SelectAsync{TNew}(Func{IOption{T},Task{Result{TNew}}})"/>
    public async ValueTask<Result<TNew>> SelectAsync<TNew>(
        Func<IOption<T>, ValueTask<Result<TNew>>> asyncBindingFunction)
    {
        return IsSuccessful && await asyncBindingFunction(Value) is var bindResult
            ? bindResult with
            {
                Reasons = Reasons.AddRange(bindResult.Reasons),
            }
            : new(Reasons);
    }
}