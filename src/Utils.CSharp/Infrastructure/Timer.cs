using System;
using static System.Threading.Interlocked;

namespace SFX.Utils.Infrastructure
{
    /// <summary>
    /// Implements <see cref="ITimer"/>
    /// </summary>
    public sealed class Timer : ITimer
    {
        internal static readonly string ObjectName = typeof(Timer).FullName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="interval">The repetition interval</param>
        /// <param name="handler">The handler invoked when timer is invoked</param>
        public Timer(TimeSpan interval, Action handler)
        {
            if (interval <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(interval));
            Interval = interval;
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        internal TimeSpan Interval { get; }
        internal Action Handler { get; }
        internal System.Threading.Timer InnerTimer { get; private set; }

        internal long StartCount;
        /// <inheritdoc/>
        public OperationResult<Unit> Start()
        {
            if (IsDisposed())
                return new OperationResult<Unit>(new ObjectDisposedException(ObjectName), default);

            if (1L < Increment(ref StartCount))
            {
                Decrement(ref StartCount);
                return new OperationResult<Unit>(default, Unit.Value);
            }

            try
            {
                InnerTimer = new System.Threading.Timer(new System.Threading.TimerCallback(_ => Handler()),
                    default, TimeSpan.Zero, Interval);
                return new OperationResult<Unit>(default, Unit.Value);
            }
            catch (Exception error)
            {
                return new OperationResult<Unit>(error, default);
            }
        }

        /// <inheritdoc/>
        public OperationResult<Unit> Stop()
        {
            if (IsDisposed())
                return new OperationResult<Unit>(new ObjectDisposedException(ObjectName), default);

            if (Decrement(ref StartCount) < 0L)
            {
                Increment(ref StartCount);
                return new OperationResult<Unit>(default, Unit.Value);
            }

            try
            {
                InnerTimer.Dispose();
                InnerTimer = default;
                return new OperationResult<Unit>(default, Unit.Value);
            }
            catch (Exception error)
            {
                return new OperationResult<Unit>(error, default);
            }
        }

        internal long DisposeCount;
        private bool IsDisposed() => 0L < Read(ref DisposeCount);
        /// <inheritdoc/>
        public void Dispose()
        {
            if (1L < Increment(ref DisposeCount))
                return;

            InnerTimer?.Dispose();
            InnerTimer = default;
        }
    }
}
