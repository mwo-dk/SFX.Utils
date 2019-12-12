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
        public Result<TimeZoneInfo> FindSystemTimeZoneById(string id)
        {
            try
            {
                return new Result<TimeZoneInfo>(TimeZoneInfo.FindSystemTimeZoneById(id), default);
            }
            catch (Exception error)
            {
                return new Result<TimeZoneInfo>(default, error);
            }
        }
    }
}
