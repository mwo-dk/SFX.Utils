using System;

namespace SFX.Utils.Infrastructure
{
    /// <summary>
    /// Implements <see cref="ITimerProvider"/>
    /// </summary>
    public sealed class TimerProvider : ITimerProvider
    {
        /// <inheritdoc/>
        public OperationResult<ITimer> Create(TimeSpan interval, Action handler, bool autoStart)
        {
            try
            {
                var result = new Timer(interval, handler);
                if (autoStart)
                    result.Start();
                return new OperationResult<ITimer>(default, result);
            }
            catch (Exception error)
            {
                return new OperationResult<ITimer>(error, default);
            }
        }
    }
}
