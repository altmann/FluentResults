namespace FluentResults.Immutable;

/// <summary>
///     Represents an optional value.
/// </summary>
public readonly record struct Option
{
    /// <summary>
    ///     Represents the <paramref name="value" />
    ///     as an option.
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    /// <param name="value">Value to associate with the option/</param>
    /// <returns>A new instance of <see cref="Immutable.Some{T}" />.</returns>
    public static Some<T> Some<T>(T value) => new(value);

    /// <summary>
    ///     Represents the lack of value.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <returns>
    ///     A new instance of <see cref="Immutable.None{T}" />.
    /// </returns>
    public static None<T> None<T>() => new();
}

/// <inheritdoc cref="Option" />
/// <typeparam name="T">Type of the value.</typeparam>
public partial interface IOption<out T>
{
    /// <summary>
    ///     Projects the value of this <see cref="IOption{T}" />
    ///     to a <typeparamref name="TMatchResult" />.
    /// </summary>
    /// <typeparam name="TMatchResult">Generic type of the match.</typeparam>
    /// <param name="matchSome">
    ///     A delegate accepting <typeparamref name="T" />,
    ///     which will be executed if this <see cref="IOption{T}" />
    ///     is <see cref="Some{T}" />.
    /// </param>
    /// <param name="matchNone">
    ///     A delegate which will be executed if
    ///     this <see cref="IOption{T}" /> is <see cref="None{T}" />.
    /// </param>
    /// <returns>
    ///     An instance of <typeparamref name="TMatchResult" />,
    ///     obtained by executing either <paramref name="matchSome" />
    ///     or <paramref name="matchNone" /> delegates.
    /// </returns>
    /// <exception cref="NotSupportedException">
    ///     Thrown when this <see cref="Option" />
    ///     is neither <see cref="Some{T}" /> nor
    ///     <see cref="None{T}" />.
    /// </exception>
    public TMatchResult Match<TMatchResult>(
        Func<T, TMatchResult> matchSome,
        Func<TMatchResult> matchNone);

    /// <summary>
    ///     Executes either <paramref name="matchSome" />
    ///     or <paramref name="matchNone" />, depending on whether
    ///     this <see cref="IOption{T}" /> is <see cref="Some{T}" />
    ///     or <see cref="None{T}" />.
    /// </summary>
    /// <param name="matchSome">
    ///     A delegate which will be executed if
    ///     this <see cref="IOption{T}" /> is <see cref="Some{T}" />.
    /// </param>
    /// <param name="matchNone">
    ///     A delegate which will be executed if
    ///     this <see cref="IOption{T}" /> is <see cref="None{T}" />.
    /// </param>
    /// <exception cref="NotSupportedException"></exception>
    public void Match(
        Action<T> matchSome,
        Action matchNone);

    private protected static TMatchResult DefaultMatch<TMatchResult>(
        IOption<T> @this,
        Func<T, TMatchResult> matchSome,
        Func<TMatchResult> matchNone) =>
        @this switch
        {
            Some<T> { Value: var value, } => matchSome(value),
            None<T> _ => matchNone(),
            _ => throw new NotSupportedException(
                $"Custom implementations of {nameof(IOption<T>)} interface are not supported!"),
        };

    private protected static void DefaultMatch(
        IOption<T> @this,
        Action<T> matchSome,
        Action matchNone)
    {
        var actionToExecute = @this switch
        {
            Some<T> { Value: var value, } => () => matchSome(value),
            None<T> _ => matchNone,
            _ => throw new NotSupportedException(
                $"Custom implementations of {nameof(IOption<T>)} interface are not supported!"),
        };

        actionToExecute();
    }
}

/// <summary>
///     Represents a value.
/// </summary>
/// <typeparam name="T">Generic type of the value.</typeparam>
public readonly partial record struct Some<T>(T Value) : IOption<T>
{
    public TMatchResult Match<TMatchResult>(Func<T, TMatchResult> matchSome, Func<TMatchResult> matchNone) =>
        IOption<T>.DefaultMatch(
            this,
            matchSome,
            matchNone);

    public void Match(Action<T> matchSome, Action matchNone) =>
        IOption<T>.DefaultMatch(
            this,
            matchSome,
            matchNone);
}

/// <summary>
///     Represents an absence of value.
/// </summary>
/// <typeparam name="T">Generic type of the <see cref="IOption{T}" />.</typeparam>
public readonly partial record struct None<T> : IOption<T>
{
    public TMatchResult Match<TMatchResult>(Func<T, TMatchResult> matchSome, Func<TMatchResult> matchNone) =>
        IOption<T>.DefaultMatch(
            this,
            matchSome,
            matchNone);

    public void Match(Action<T> matchSome, Action matchNone) =>
        IOption<T>.DefaultMatch(
            this,
            matchSome,
            matchNone);
}