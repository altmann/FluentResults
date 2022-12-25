namespace FluentResults.Immutable;

public abstract record Maybe
{
    protected Maybe()
    {
    }

    public static Some<T> Some<T>(T value) => new(value);

    public static None<T> None<T>() => new();
}

public abstract record Maybe<T> : Maybe
{
    protected Maybe()
    {
    }

    public TMatchResult Match<TMatchResult>(
        Func<T, TMatchResult> matchSome,
        Func<TMatchResult> matchNone) =>
        this switch
        {
            Some<T> { Value: var value } => matchSome(value),
            None<T> _ => matchNone(),
            _ => throw new NotSupportedException($"Custom implementations of {nameof(Maybe<T>)} record are not supported!"),
        };

    public void Match(
        Action<T> matchSome,
        Action matchNone)
    {
        Action actionToExecute = this switch
        {
            Some<T> { Value: var value } => () => matchSome(value),
            None<T> _ => () => matchNone(),
            _ => throw new NotSupportedException($"Custom implementations of {nameof(Maybe<T>)} record are not supported!"),
        };

        actionToExecute();
    }
}

public sealed record Some<T> : Maybe<T>
{
    internal Some(T value)
    {
        Value = value;
    }

    public T Value { get; init; }
}

public sealed record None<T> : Maybe<T>
{
    internal None()
    {
    }
}
