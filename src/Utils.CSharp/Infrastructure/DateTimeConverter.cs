using System;

namespace SFX.Utils.Infrastructure
{
    /// <summary>
	/// Implements <see cref="IDateTimeConverter"/>
	/// </summary>
	public sealed class DateTimeConverter : IDateTimeConverter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timeZoneProvider">The utilized <see cref="ITimeZoneProvider"/></param>
        public DateTimeConverter(ITimeZoneProvider timeZoneProvider) =>
            TimeZoneProvider = timeZoneProvider ?? throw new ArgumentNullException(nameof(timeZoneProvider));

        internal ITimeZoneProvider TimeZoneProvider { get; }

        /// <inheritdoc/>
        public Result<DateTimeOffset> Convert(DateTimeOffset dateTimeOffset, TimeZoneInfo timeZoneInfo)
        {
            try
            {
                
                return new Result<DateTimeOffset>(TimeZoneInfo.ConvertTime(dateTimeOffset, timeZoneInfo), default);
            }
            catch (Exception error)
            {
                return new Result<DateTimeOffset>(default, error);
            }
        }

        /// <inheritdoc/>
        public Result<DateTimeOffset> ToUtc(DateTimeOffset dateTimeOffset)
        {
            try
            {
                return new Result<DateTimeOffset>(TimeZoneInfo.ConvertTime(dateTimeOffset, TimeZoneProvider.GetUtc()), default);
            }
            catch (Exception error)
            {
                return new Result<DateTimeOffset>(default, error);
            }
        }
    }
}
