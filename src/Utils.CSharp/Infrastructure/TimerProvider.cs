using System;

namespace SFX.Utils.Infrastructure
{
    /// <summary>
    /// Implements <see cref="ITimerProvider"/>
    /// </summary>
    public sealed class TimerProvider : ITimerProvider
    {
        /// <inheritdoc/>
        public ITimer Create(TimeSpan interval, Action handler, bool autoStart)
        {
            var result = new Timer(interval, handler);
            if (autoStart)
                result.Start();
            return result;
        }
    }
}
