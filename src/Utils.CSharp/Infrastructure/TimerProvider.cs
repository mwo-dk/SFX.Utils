using System;
using SFX.ROP.CSharp;
using static SFX.ROP.CSharp.Library;

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
                return Succeed(result as ITimer);
            }
            catch (Exception error)
            {
                return Fail<ITimer>(error);
            }
        }
    }
}
