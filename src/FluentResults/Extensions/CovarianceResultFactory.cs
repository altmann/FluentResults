using System;
using System.Collections.Generic;
using System.Text;

namespace FluentResults.Extensions
{
    /// <summary>
    /// Creates a covariant <see cref="IResult{TValue}"/>, 
    /// where TValue is updated to the actual type
    /// of the given value.
    /// </summary>
    internal interface ICovarianceResultFactory
    {
        /// <summary>
        /// Creates an <see cref="IResultBase" /> which actually is a 
        /// <see cref="Result{TValue}"/>. <paramref name="value"/> has to be the correct
        /// type of, otherwise we get an <see cref="InvalidCastException"/>.
        /// The result can be cast to any compatible IResult{T} via covariance.
        /// </summary>
        IResultBase Create(object value); 
    }

    /// <summary>
    /// Factory to create a <see cref="Result{TValue}"/>, that 
    /// is cast into an <see cref="IResultBase"/>.
    /// </summary>
    /// <remarks>
    /// This class is used to create <see cref="IResult{TValue}"/> – 
    /// that supports covariance – via Reflection.
    /// </remarks>
    internal sealed class CovarianceResultFactory<TValue> : ICovarianceResultFactory
    {
        /// <inheritdoc/>
        public IResultBase Create(object value) => 
            new Result<TValue>().WithValue((TValue)value); 
    }

    internal static class CovarianceResultFactory
    {
        /// <summary>
        /// Creates an <see cref="IResult{TValue}"/> that correctly supports
        /// covariances.
        /// </summary>
        /// <remarks>
        /// If <paramref name="value"/> is of a derived type, we create
        /// an IResult of the derived type, that is cast into an
        /// result of <typeparamref name="TValue"/>.
        /// </remarks>
        public static IResult<TValue> Create<TValue>(object value)
        {
            // Get the actual type (or use TValue, if value is null)
            Type valueType = value?.GetType() ?? typeof(TValue);

            // We create the factory of the actual value type.
            Type factoryType = typeof(CovarianceResultFactory<>).MakeGenericType(valueType);
            var factory = (ICovarianceResultFactory)Activator.CreateInstance(factoryType);

            // We create the IResult<TValue>
            return (IResult<TValue>)factory.Create(value);
        }
    }
}
