using System;

namespace SFX.Utils.Infrastructure
{
    /// <summary>
	/// Implements <see cref="ITimeZoneProvider"/>
	/// </summary>
	public sealed class TimeZoneProvider : ITimeZoneProvider
    {
        /// <inheritdoc/>
        public TimeZoneInfo GetLocal() => TimeZoneInfo.Local;

        /// <inheritdoc/>
        public TimeZoneInfo GetUtc() => TimeZoneInfo.Utc;

        /// <inheritdoc/>
        public OperationResult<TimeZoneInfo> FindSystemTimeZoneById(string id)
        {
            try
            {
                return new OperationResult<TimeZoneInfo>(default, TimeZoneInfo.FindSystemTimeZoneById(id));
            }
            catch (Exception error)
            {
                return new OperationResult<TimeZoneInfo>(error, default);
            }
        }
    }
}
