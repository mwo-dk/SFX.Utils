using System;

namespace SFX.Utils.Infrastructure
{
    /// <summary>
    /// Represents the result of an operation/invokation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Result<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">The optional value</param>
        /// <param name="error">The optional <see cref="Exception"/></param>
        internal Result(T value, Exception error) =>
            (Value, Error) = (value, error);

        /// <summary>
        /// The result of the operation if success full
        /// </summary>
        public T Value { get; }
        /// <summary>
        /// The error of the operation if successfull
        /// </summary>
        public Exception Error { get; }

        /// <summary>
        /// Deconstructor
        /// </summary>
        /// <param name="success">Flag telling whether the operation was successfull</param>
        /// <param name="error">The <see cref="Exception"/> if present</param>
        /// <param name="value">The result - of successfull</param>
        public void Deconstruct(out bool success, out Exception error, out T value) =>
            (success, error, value) = (Error is null, Error, Value);

        /// <summary>
        /// Implicit cast to the value if the operation was successfull
        /// </summary>
        /// <param name="x">The <see cref="Result{T}"/> to cast</param>
        public static implicit operator T(Result<T> x)
        {
            if (!(x.Error is null))
                throw x.Error;
            else return x.Value;
        }
    }

    /// <summary>
    /// Extension methods for <see cref="Result{T}"/>
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Returns an <see cref="Result{T}"/> denoting success
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the result</typeparam>
        /// <param name="value">The successfull result</param>
        /// <returns>An <see cref="Result{T}"/> denoting success</returns>
        public static Result<T> Succeed<T>(T value) =>
            new Result<T>(value, default);

        /// <summary>
        /// Returns an <see cref="Result{T}"/> denoting failure
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the failed result</typeparam>
        /// <param name="error">The error of the operation</param>
        /// <returns>An <see cref="Result{T}"/> denoting success</returns>
        public static Result<T> Fail<T>(Exception error) =>
            new Result<T>(default, error);
    }
}
