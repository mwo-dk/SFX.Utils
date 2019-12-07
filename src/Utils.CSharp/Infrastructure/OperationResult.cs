using System;

namespace SFX.Utils.Infrastructure
{
    /// <summary>
    /// Represents the result of an operation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct OperationResult<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="error">The optional <see cref="Exception"/></param>
        /// <param name="value">The optional value</param>
        internal OperationResult(Exception error, T value) =>
            (Error, Value) = (error, value);

        public Exception Error { get; }
        public T Value { get; }

        public void Deconstruct(out bool success, out Exception error, out T value) =>
            (success, error, value) = (Error is null, Error, Value);

        public static implicit operator T(OperationResult<T> x)
        {
            if (!(x.Error is null))
                throw x.Error;
            else return x.Value;
        }
    }
}
