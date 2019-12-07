using System;

namespace SFX.Utils.Infrastructure
{
    /// <summary>
    /// Represents a timer
    /// </summary>
    public interface ITimer : IDisposable
    {
        /// <summary>
        /// Starts the timer
        /// </summary>
        OperationResult<Unit> Start();
        /// <summary>
        /// Stops the timer
        /// </summary>
        OperationResult<Unit> Stop();
    }
}