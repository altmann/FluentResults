namespace FluentResults.Immutable;

/// <summary>
///     Represents a unit.
/// </summary>
/// <remarks>
///     Should be used as a returned value for <see cref="Result{T}" />
///     if the equivalent of non-generic result is to be returned.
/// </remarks>
public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
{
    private static readonly Unit ValueOfUnit = new();

    public static ref readonly Unit Value => ref ValueOfUnit;

    /// <inheritdoc />
    public bool Equals(Unit _) => true;

    /// <inheritdoc />
    public int CompareTo(Unit other) => 0;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Unit;

    /// <inheritdoc />
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc />
    public int CompareTo(object? obj) => 0;

    public static bool operator ==(Unit _1, Unit _2) => true;

    public static bool operator !=(Unit _1, Unit _2) => false;
}