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
        void Start();
        /// <summary>
        /// Stops the timer
        /// </summary>
        void Stop();
    }
}