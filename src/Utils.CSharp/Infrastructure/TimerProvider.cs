using System;

namespace SFX.Utils.Infrastructure
{
    /// <summary>
    /// Implements <see cref="ITimerProvider"/>
    /// </summary>
    public sealed class TimerProvider : ITimerProvider
    {
        /// <inheritdoc/>
        public Result<ITimer> Create(TimeSpan interval, Action handler, bool autoStart)
        {
            try
            {
                var result = new Timer(interval, handler);
                if (autoStart)
                    result.Start();
                return new Result<ITimer>(result, default);
            }
            catch (Exception error)
            {
                return new Result<ITimer>(default, error);
            }
        }
    }
}
