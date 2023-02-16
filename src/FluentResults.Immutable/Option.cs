namespace FluentResults.Immutable;

/// <summary>
///     Represents an optional value.
/// </summary>
public abstract record Option
{
    /// <summary>
    ///     Represents the <paramref name="value" />
    ///     as an <see cref="Option{T}" />.
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
public abstract record Option<T>
{
    /// <summary>
    ///     Projects the value of this <see cref="Option{T}" />
    ///     to a <typeparamref name="TMatchResult" />.
    /// </summary>
    /// <typeparam name="TMatchResult">Generic type of the match.</typeparam>
    /// <param name="matchSome">
    ///     A delegate accepting <typeparamref name="T" />,
    ///     which will be executed if this <see cref="Option{T}" />
    ///     is <see cref="Some{T}" />.
    /// </param>
    /// <param name="matchNone">
    ///     A delegate which will be executed if
    ///     this <see cref="Option{T}" /> is <see cref="None{T}" />.
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
        Func<TMatchResult> matchNone) =>
        this switch
        {
            Some<T> { Value: var value, } => matchSome(value),
            None<T> _ => matchNone(),
            _ => throw new NotSupportedException(
                $"Custom implementations of {nameof(Option<T>)} record are not supported!"),
        };

    /// <summary>
    ///     Executes either <paramref name="matchSome" />
    ///     or <paramref name="matchNone" />, depending on whether
    ///     this <see cref="Option{T}" /> is <see cref="Some{T}" />
    ///     or <see cref="None{T}" />.
    /// </summary>
    /// <param name="matchSome">
    ///     A delegate which will be executed if
    ///     this <see cref="Option{T}" /> is <see cref="Some{T}" />.
    /// </param>
    /// <param name="matchNone">
    ///     A delegate which will be executed if
    ///     this <see cref="Option{T}" /> is <see cref="None{T}" />.
    /// </param>
    /// <exception cref="NotSupportedException"></exception>
    public void Match(
        Action<T> matchSome,
        Action matchNone)
    {
        var actionToExecute = this switch
        {
            Some<T> { Value: var value, } => () => matchSome(value),
            None<T> _ => matchNone,
            _ => throw new NotSupportedException(
                $"Custom implementations of {nameof(Option<T>)} record are not supported!"),
        };

        actionToExecute();
    }
}

/// <summary>
///     Represents a value.
/// </summary>
/// <typeparam name="T">Generic type of the value.</typeparam>
public sealed record Some<T> : Option<T>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="None{T}" /> record.
    /// </summary>
    /// <param name="value">Value to wrap.</param>
    internal Some(T value)
    {
        Value = value;
    }

    /// <summary>
    ///     Gets the value.
    /// </summary>
    public T Value { get; init; }
}

/// <summary>
///     Represents an absence of value.
/// </summary>
/// <typeparam name="T">Generic type of the <see cref="Option{T}" />.</typeparam>
public sealed record None<T> : Option<T>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="None{T}" /> record.
    /// </summary>
    internal None()
    {
    }
}