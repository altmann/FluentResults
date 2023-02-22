namespace FluentResults.Immutable;

public partial interface IOption<out T>
{
    /// <summary>
    ///     Combines the options and uses <paramref name="selector" />
    ///     to project the combined values to a new <see cref="IOption{T}" />.
    /// </summary>
    /// <typeparam name="T1">
    ///     Generic type of the combined <see cref="IOption{T}" />.
    /// </typeparam>
    /// <typeparam name="TOut">
    ///     Generic type of the resulting <see cref="IOption{T}" />.
    /// </typeparam>
    /// <param name="combinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T1" /> from existing option.
    /// </param>
    /// <param name="selector">
    ///     Selector for the final <see cref="IOption{T}" />.
    /// </param>
    /// <returns>
    ///     An <see cref="IOption{T}" />, which combines all
    ///     of the intermediate options.
    /// </returns>
    public IOption<TOut> SelectMany<T1, TOut>(
        Func<IOption<T>, IOption<T1>> combinator,
        Func<T, T1, IOption<TOut>> selector);

    /// <typeparam name="T2">
    ///     Generic type of the combined <see cref="IOption{T}" />.
    /// </typeparam>
    /// <param name="firstCombinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T1" /> from existing option.
    /// </param>
    /// <param name="secondCombinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T2" /> from existing option.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1,TOut}" />
    public IOption<TOut> SelectMany<T1, T2, TOut>(
        Func<IOption<T>, IOption<T1>> firstCombinator,
        Func<IOption<T>, IOption<T2>> secondCombinator,
        Func<T, T1, T2, IOption<TOut>> selector);

    /// <typeparam name="T3">
    ///     Generic type of the combined <see cref="IOption{T}" />.
    /// </typeparam>
    /// <param name="firstCombinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T1" /> from existing option.
    /// </param>
    /// <param name="secondCombinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T2" /> from existing option.
    /// </param>
    /// <param name="thirdCombinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T3" /> from existing option.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1,T2,TOut}" />
    public IOption<TOut> SelectMany<T1, T2, T3, TOut>(
        Func<IOption<T>, IOption<T1>> firstCombinator,
        Func<IOption<T>, IOption<T2>> secondCombinator,
        Func<IOption<T>, IOption<T3>> thirdCombinator,
        Func<T, T1, T2, T3, IOption<TOut>> selector);

    /// <typeparam name="T4">
    ///     Generic type of the combined <see cref="IOption{T}" />.
    /// </typeparam>
    /// <param name="firstCombinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T1" /> from existing option.
    /// </param>
    /// <param name="secondCombinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T2" /> from existing option.
    /// </param>
    /// <param name="thirdCombinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T3" /> from existing option.
    /// </param>
    /// <param name="fourthCombinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T4" /> from existing option.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1,T2,T3,TOut}" />
    public IOption<TOut> SelectMany<T1, T2, T3, T4, TOut>(
        Func<IOption<T>, IOption<T1>> firstCombinator,
        Func<IOption<T>, IOption<T2>> secondCombinator,
        Func<IOption<T>, IOption<T3>> thirdCombinator,
        Func<IOption<T>, IOption<T4>> fourthCombinator,
        Func<T, T1, T2, T3, T4, IOption<TOut>> selector);

    /// <typeparam name="T5">
    ///     Generic type of the combined <see cref="IOption{T}" />.
    /// </typeparam>
    /// <param name="firstCombinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T1" /> from existing option.
    /// </param>
    /// <param name="secondCombinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T2" /> from existing option.
    /// </param>
    /// <param name="thirdCombinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T3" /> from existing option.
    /// </param>
    /// <param name="fourthCombinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T4" /> from existing option.
    /// </param>
    /// <param name="fifthCombinator">
    ///     Delegate used to obtain a <see cref="IOption{T}" />
    ///     of <typeparamref name="T4" /> from existing option.
    /// </param>
    /// <inheritdoc cref="SelectMany{T1,T2,T3,T4,TOut}" />
    public IOption<TOut> SelectMany<T1, T2, T3, T4, T5, TOut>(
        Func<IOption<T>, IOption<T1>> firstCombinator,
        Func<IOption<T>, IOption<T2>> secondCombinator,
        Func<IOption<T>, IOption<T3>> thirdCombinator,
        Func<IOption<T>, IOption<T4>> fourthCombinator,
        Func<IOption<T>, IOption<T5>> fifthCombinator,
        Func<T, T1, T2, T3, T4, T5, IOption<TOut>> selector);
}

public readonly partial record struct Some<T>
{
    /// <inheritdoc />
    public IOption<TOut> SelectMany<T1, TOut>(
        Func<IOption<T>, IOption<T1>> combinator,
        Func<T, T1, IOption<TOut>> selector) =>
        (this, combinator(this)) switch
        {
            ({ Value: var initialValue, }, Some<T1> { Value: var secondValue, }) => selector(initialValue, secondValue),
            (_, _) => Option.None<TOut>(),
        };

    /// <inheritdoc />
    public IOption<TOut> SelectMany<T1, T2, TOut>(
        Func<IOption<T>, IOption<T1>> firstCombinator,
        Func<IOption<T>, IOption<T2>> secondCombinator,
        Func<T, T1, T2, IOption<TOut>> selector) =>
        (this, firstCombinator(this), secondCombinator(this)) switch
        {
            ({ Value: var initialValue, }, Some<T1> { Value: var secondValue, }, Some<T2> { Value: var thirdValue, }) =>
                selector(
                    initialValue,
                    secondValue,
                    thirdValue),
            (_, _, _) => Option.None<TOut>(),
        };

    /// <inheritdoc />
    public IOption<TOut> SelectMany<T1, T2, T3, TOut>(
        Func<IOption<T>, IOption<T1>> firstCombinator,
        Func<IOption<T>, IOption<T2>> secondCombinator,
        Func<IOption<T>, IOption<T3>> thirdCombinator,
        Func<T, T1, T2, T3, IOption<TOut>> selector) =>
        (this, firstCombinator(this), secondCombinator(this), thirdCombinator(this)) switch
        {
            ({ Value: var initialValue, }, Some<T1> { Value: var secondValue, }, Some<T2> { Value: var thirdValue, },
                Some<T3> { Value: var fourthValue, }) =>
                selector(
                    initialValue,
                    secondValue,
                    thirdValue,
                    fourthValue),
            (_, _, _, _) => Option.None<TOut>(),
        };

    /// <inheritdoc />
    public IOption<TOut> SelectMany<T1, T2, T3, T4, TOut>(
        Func<IOption<T>, IOption<T1>> firstCombinator,
        Func<IOption<T>, IOption<T2>> secondCombinator,
        Func<IOption<T>, IOption<T3>> thirdCombinator,
        Func<IOption<T>, IOption<T4>> fourthCombinator,
        Func<T, T1, T2, T3, T4, IOption<TOut>> selector) =>
        (this, firstCombinator(this), secondCombinator(this), thirdCombinator(this), fourthCombinator(this)) switch
        {
            ({ Value: var initialValue, }, Some<T1> { Value: var secondValue, }, Some<T2> { Value: var thirdValue, },
                Some<T3> { Value: var fourthValue, }, Some<T4> { Value: var fifthValue, }) =>
                selector(
                    initialValue,
                    secondValue,
                    thirdValue,
                    fourthValue,
                    fifthValue),
            (_, _, _, _, _) => Option.None<TOut>(),
        };

    /// <inheritdoc />
    public IOption<TOut> SelectMany<T1, T2, T3, T4, T5, TOut>(
        Func<IOption<T>, IOption<T1>> firstCombinator,
        Func<IOption<T>, IOption<T2>> secondCombinator,
        Func<IOption<T>, IOption<T3>> thirdCombinator,
        Func<IOption<T>, IOption<T4>> fourthCombinator,
        Func<IOption<T>, IOption<T5>> fifthCombinator,
        Func<T, T1, T2, T3, T4, T5, IOption<TOut>> selector) =>
        (this, firstCombinator(this), secondCombinator(this), thirdCombinator(this), fourthCombinator(this),
                fifthCombinator(this)) switch
            {
                ({ Value: var initialValue, }, Some<T1> { Value: var secondValue, }, Some<T2> { Value: var thirdValue, }
                    ,
                    Some<T3> { Value: var fourthValue, }, Some<T4> { Value: var fifthValue, },
                    Some<T5> { Value: var sixthValue, }) =>
                    selector(
                        initialValue,
                        secondValue,
                        thirdValue,
                        fourthValue,
                        fifthValue,
                        sixthValue),
                (_, _, _, _, _, _) => Option.None<TOut>(),
            };
}

public readonly partial record struct None<T>
{
    /// <inheritdoc />
    public IOption<TOut> SelectMany<T1, TOut>(
        Func<IOption<T>, IOption<T1>> combinator,
        Func<T, T1, IOption<TOut>> selector) =>
        Option.None<TOut>();

    /// <inheritdoc />
    public IOption<TOut> SelectMany<T1, T2, TOut>(
        Func<IOption<T>, IOption<T1>> firstCombinator,
        Func<IOption<T>, IOption<T2>> secondCombinator,
        Func<T, T1, T2, IOption<TOut>> selector) =>
        Option.None<TOut>();

    /// <inheritdoc />
    public IOption<TOut> SelectMany<T1, T2, T3, TOut>(
        Func<IOption<T>, IOption<T1>> firstCombinator,
        Func<IOption<T>, IOption<T2>> secondCombinator,
        Func<IOption<T>, IOption<T3>> thirdCombinator,
        Func<T, T1, T2, T3, IOption<TOut>> selector) =>
        Option.None<TOut>();

    /// <inheritdoc />
    public IOption<TOut> SelectMany<T1, T2, T3, T4, TOut>(
        Func<IOption<T>, IOption<T1>> firstCombinator,
        Func<IOption<T>, IOption<T2>> secondCombinator,
        Func<IOption<T>, IOption<T3>> thirdCombinator,
        Func<IOption<T>, IOption<T4>> fourthCombinator,
        Func<T, T1, T2, T3, T4, IOption<TOut>> selector) =>
        Option.None<TOut>();

    /// <inheritdoc />
    public IOption<TOut> SelectMany<T1, T2, T3, T4, T5, TOut>(
        Func<IOption<T>, IOption<T1>> firstCombinator,
        Func<IOption<T>, IOption<T2>> secondCombinator,
        Func<IOption<T>, IOption<T3>> thirdCombinator,
        Func<IOption<T>, IOption<T4>> fourthCombinator,
        Func<IOption<T>, IOption<T5>> fifthCombinator,
        Func<T, T1, T2, T3, T4, T5, IOption<TOut>> selector) =>
        Option.None<TOut>();
}