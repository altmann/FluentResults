namespace FluentResults.Immutable;

public readonly partial record struct Result<T>
{
    /// <summary>
    ///     Combines the results and uses <paramref name="selector" />
    ///     to project the combined values to a new <see cref="Result{T}" />.
    /// </summary>
    /// <typeparam name="T1">Generic type of the combined result.</typeparam>
    /// <typeparam name="TOut">Generic type of the final result.</typeparam>
    /// <param name="combinator">
    ///     Delegate used to obtain a <see cref="Result{T}" />
    ///     of <typeparamref name="T1" /> from existing result.
    /// </param>
    /// <param name="selector">
    ///     Result selector for the final <see cref="Result{T}" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Result{T}" /> wrapping <typeparamref name="TOut" />,
    ///     which combines all intermediate results using
    ///     <see cref="Select{TNew}(Func{IOption{T}, Result{TNew}})" />.
    /// </returns>
    public Result<TOut> SelectMany<T1, TOut>(
        Func<IOption<T>, Result<T1>> combinator,
        Func<IOption<T>, IOption<T1>, Result<TOut>> selector) =>
        Select(
            value => combinator(value)
                .Select(secondValue => selector(value, secondValue)));

    /// <typeparam name="T2">Generic type of the combined result.</typeparam>
    /// <param name="firstCombinator">
    ///     Delegate used to obtain a <see cref="Result{T}" />
    ///     of <typeparamref name="T1" /> from existing result.
    /// </param>
    /// <param name="secondCombinator">
    ///     Delegate used to obtain a <see cref="Result{T}" />
    ///     of <typeparamref name="T2" /> from existing result.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1,TOut}" />
    public Result<TOut> SelectMany<T1, T2, TOut>(
        Func<IOption<T>, Result<T1>> firstCombinator,
        Func<IOption<T>, Result<T2>> secondCombinator,
        Func<IOption<T>, IOption<T1>, IOption<T2>, Result<TOut>> selector) =>
        Select(
            value => firstCombinator(value)
                .Select(
                    secondValue => secondCombinator(value)
                        .Select(
                            finalValue => selector(
                                value,
                                secondValue,
                                finalValue))));

    /// <param name="thirdCombinator">
    ///     Delegate used to obtain a <see cref="Result{T}" />
    ///     of <typeparamref name="T3" /> from existing result.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1, T2, TOut}" />
    public Result<TOut> SelectMany<T1, T2, T3, TOut>(
        Func<IOption<T>, Result<T1>> firstCombinator,
        Func<IOption<T>, Result<T2>> secondCombinator,
        Func<IOption<T>, Result<T3>> thirdCombinator,
        Func<IOption<T>, IOption<T1>, IOption<T2>, IOption<T3>, Result<TOut>> selector) =>
        Select(
            value => firstCombinator(value)
                .Select(
                    secondValue => secondCombinator(value)
                        .Select(
                            thirdValue => thirdCombinator(value)
                                .Select(
                                    finalValue => selector(
                                        value,
                                        secondValue,
                                        thirdValue,
                                        finalValue)))));

    /// <param name="fourthCombinator">
    ///     Delegate used to obtain a <see cref="Result{T}" />
    ///     of <typeparamref name="T4" /> from existing result.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1, T2, T3, TOut}" />
    public Result<TOut> SelectMany<T1, T2, T3, T4, TOut>(
        Func<IOption<T>, Result<T1>> firstCombinator,
        Func<IOption<T>, Result<T2>> secondCombinator,
        Func<IOption<T>, Result<T3>> thirdCombinator,
        Func<IOption<T>, Result<T4>> fourthCombinator,
        Func<IOption<T>, IOption<T1>, IOption<T2>, IOption<T3>, IOption<T4>, Result<TOut>> selector)
    {
        return Select(
            value => firstCombinator(value)
                .Select(
                    secondValue => secondCombinator(value)
                        .Select(
                            thirdValue => thirdCombinator(value)
                                .Select(
                                    fourthValue => fourthCombinator(value)
                                        .Select(
                                            finalValue => selector(
                                                value,
                                                secondValue,
                                                thirdValue,
                                                fourthValue,
                                                finalValue))))));
    }

    /// <param name="fifthCombinator">
    ///     Delegate used to obtain a <see cref="Result{T}" />
    ///     of <typeparamref name="T5" /> from existing result.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1, T2, T3, T4, TOut}" />
    public Result<TOut> SelectMany<T1, T2, T3, T4, T5, TOut>(
        Func<IOption<T>, Result<T1>> firstCombinator,
        Func<IOption<T>, Result<T2>> secondCombinator,
        Func<IOption<T>, Result<T3>> thirdCombinator,
        Func<IOption<T>, Result<T4>> fourthCombinator,
        Func<IOption<T>, Result<T5>> fifthCombinator,
        Func<IOption<T>, IOption<T1>, IOption<T2>, IOption<T3>, IOption<T4>, IOption<T5>, Result<TOut>> selector) =>
        Select(
            value => firstCombinator(value)
                .Select(
                    secondValue => secondCombinator(value)
                        .Select(
                            thirdValue => thirdCombinator(value)
                                .Select(
                                    fourthValue => fourthCombinator(value)
                                        .Select(
                                            fifthValue => fifthCombinator(value)
                                                .Select(
                                                    finalValue => selector(
                                                        value,
                                                        secondValue,
                                                        thirdValue,
                                                        fourthValue,
                                                        fifthValue,
                                                        finalValue)))))));
}