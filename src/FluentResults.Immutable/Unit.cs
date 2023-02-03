namespace FluentResults.Immutable;

/// <summary>
///     Represents a unit.
/// </summary>
/// <remarks>
///     Should be used as a returned value for <see cref="Result{T}" />
///     if the equivalent of non-generic result is to be returned.
/// </remarks>
public readonly struct Unit
{
    private static readonly Unit ValueOfUnit = new();

    public static ref readonly Unit Value => ref ValueOfUnit;
}