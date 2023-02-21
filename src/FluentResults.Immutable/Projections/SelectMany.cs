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
    /// <param name="noneResultSelector">
    ///     Delegate to invoke when any of the values is <see cref="None{T}" />,
    ///     defaults to returning a new result with the generic type changed to
    ///     <typeparamref name="TOut" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Result{T}" /> wrapping <typeparamref name="TOut" />,
    ///     which combines all intermediate results using
    ///     <see cref="Select{TNew}(Func{T, Result{TNew}})" />.
    /// </returns>
    public Result<TOut> SelectMany<T1, TOut>(
        Func<T, Result<T1>> combinator,
        Func<T, T1, Result<TOut>> selector,
        Func<Result<TOut>>? noneResultSelector = null)
    {
        Result<TOut> fallbackResult = new(Reasons);
        noneResultSelector ??= () => fallbackResult;

        return Select(
            value => combinator(value)
                .Select(secondValue => selector(value, secondValue), noneResultSelector),
            noneResultSelector);
    }

    /// <param name="firstCombinator">
    ///     Delegate used to obtain a <see cref="Result{T}" />
    ///     of <typeparamref name="T1" /> from existing result.
    /// </param>
    /// <param name="secondCombinator">
    ///     Delegate used to obtain a <see cref="Result{T}" />
    ///     of <typeparamref name="T2" /> from existing result.
    /// </param>
    /// <param name="noneResultSelector">
    ///     Delegate to invoke when any of the values is <see cref="None{T}" />,
    ///     defaults to returning a new result with the generic type changed to
    ///     <typeparamref name="TOut" />.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1,TOut}" />
    public Result<TOut> SelectMany<T1, T2, TOut>(
        Func<T, Result<T1>> firstCombinator,
        Func<T, Result<T2>> secondCombinator,
        Func<T, T1, T2, Result<TOut>> selector,
        Func<Result<TOut>>? noneResultSelector = null)
    {
        Result<TOut> fallback = new(Reasons);
        noneResultSelector ??= () => fallback;

        return Select(
            value => firstCombinator(value)
                .Select(
                    secondValue => secondCombinator(value)
                        .Select(
                            finalValue => selector(
                                value,
                                secondValue,
                                finalValue),
                            noneResultSelector),
                    noneResultSelector),
            noneResultSelector);
    }

    /// <param name="thirdCombinator">
    ///     Delegate used to obtain a <see cref="Result{T}" />
    ///     of <typeparamref name="T3" /> from existing result.
    /// </param>
    /// <param name="noneResultSelector">
    ///     Delegate to invoke when any of the values is <see cref="None{T}" />,
    ///     defaults to returning a new result with the generic type changed to
    ///     <typeparamref name="TOut" />.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1, T2, TOut}" />
    public Result<TOut> SelectMany<T1, T2, T3, TOut>(
        Func<T, Result<T1>> firstCombinator,
        Func<T, Result<T2>> secondCombinator,
        Func<T, Result<T3>> thirdCombinator,
        Func<T, T1, T2, T3, Result<TOut>> selector,
        Func<Result<TOut>>? noneResultSelector = null)
    {
        Result<TOut> fallback = new(Reasons);
        noneResultSelector ??= () => fallback;

        return Select(
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
                                        finalValue),
                                    noneResultSelector),
                            noneResultSelector),
                    noneResultSelector),
            noneResultSelector);
    }

    /// <param name="fourthCombinator">
    ///     Delegate used to obtain a <see cref="Result{T}" />
    ///     of <typeparamref name="T4" /> from existing result.
    /// </param>
    /// <param name="noneResultSelector">
    ///     Delegate to invoke when any of the values is <see cref="None{T}" />,
    ///     defaults to returning a new result with the generic type changed to
    ///     <typeparamref name="TOut" />.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1, T2, T3, TOut}" />
    public Result<TOut> SelectMany<T1, T2, T3, T4, TOut>(
        Func<T, Result<T1>> firstCombinator,
        Func<T, Result<T2>> secondCombinator,
        Func<T, Result<T3>> thirdCombinator,
        Func<T, Result<T4>> fourthCombinator,
        Func<T, T1, T2, T3, T4, Result<TOut>> selector,
        Func<Result<TOut>>? noneResultSelector = null)
    {
        Result<TOut> fallback = new(Reasons);
        noneResultSelector ??= () => fallback;

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
                                                finalValue),
                                            noneResultSelector),
                                    noneResultSelector),
                            noneResultSelector),
                    noneResultSelector),
            noneResultSelector);
    }

    /// <param name="fifthCombinator">
    ///     Delegate used to obtain a <see cref="Result{T}" />
    ///     of <typeparamref name="T5" /> from existing result.
    /// </param>
    /// <param name="noneResultSelector">
    ///     Delegate to invoke when any of the values is <see cref="None{T}" />,
    ///     defaults to returning a new result with the generic type changed to
    ///     <typeparamref name="TOut" />.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1, T2, T3, T4, TOut}" />
    public Result<TOut> SelectMany<T1, T2, T3, T4, T5, TOut>(
        Func<T, Result<T1>> firstCombinator,
        Func<T, Result<T2>> secondCombinator,
        Func<T, Result<T3>> thirdCombinator,
        Func<T, Result<T4>> fourthCombinator,
        Func<T, Result<T5>> fifthCombinator,
        Func<T, T1, T2, T3, T4, T5, Result<TOut>> selector,
        Func<Result<TOut>>? noneResultSelector = null)
    {
        Result<TOut> fallback = new(Reasons);
        noneResultSelector ??= () => fallback;

        return Select(
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
                                                        finalValue),
                                                    noneResultSelector),
                                            noneResultSelector),
                                    noneResultSelector),
                            noneResultSelector),
                    noneResultSelector),
            noneResultSelector);
    }
}