using FluentResults.Immutable.Metadata;

namespace FluentResults.Immutable.Contracts;

/// <summary>
///     Contracts for immutable, generic results.
/// </summary>
/// <typeparam name="T">Type of the value.</typeparam>
public interface IImmutableResult<T>
{
    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="Result{T}" />
    ///     represents a failed operation.
    /// </summary>
    bool IsAFailure { get; }

    /// <summary>
    ///     Gets the boolean indicator whether this <see cref="Result{T}" />
    ///     represents a successful operation.
    /// </summary>
    bool IsSuccessful { get; }

    /// <summary>
    ///     Gets the value of the performed operation,
    ///     represented as <see cref="IOption{T}" />.
    /// </summary>
    /// <remarks>
    ///     Failed results should have <see cref="None{T}" /> set, while successful ones
    ///     will return <see cref="Some{T}" />.
    ///     To safely access the result value of an operation,
    ///     <see cref="IOption{T}.Match" /> overloads should be used.
    ///     Keep in mind that custom implementations of <see cref="IOption{T}" />
    ///     interface are currently not supported.
    /// </remarks>
    IOption<T> Value { get; }

    /// <summary>
    ///     Gets an <see cref="ImmutableList{T}" /> of all
    ///     <see cref="Reason" />s associated with this
    ///     <see cref="Result{T}" />.
    /// </summary>
    /// <remarks>
    ///     A <see cref="Reason" /> is either a <see cref="Success" />,
    ///     <see cref="Error" />, or any other inheritor.
    /// </remarks>
    ImmutableList<Reason> Reasons { get; }

    /// <summary>
    ///     Gets an <see cref="ImmutableList{T}" /> of <see cref="Error" />s
    ///     associated with this <see cref="Result{T}" />.
    /// </summary>
    /// <remarks>
    ///     This list will be empty for successful results.
    /// </remarks>
    ImmutableList<Error> Errors { get; }

    /// <summary>
    ///     Gets an <see cref="ImmutableList{T}" /> of
    ///     <see cref="Success" />es associated with this
    ///     <see cref="Result{T}" />.
    /// </summary>
    ImmutableList<Success> Successes { get; }
}