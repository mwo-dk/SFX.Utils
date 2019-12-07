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
        public OperationResult<DateTimeOffset> Convert(DateTimeOffset dateTimeOffset, TimeZoneInfo timeZoneInfo)
        {
            try
            {
                return new OperationResult<DateTimeOffset>(default, TimeZoneInfo.ConvertTime(dateTimeOffset, timeZoneInfo));
            }
            catch (Exception error)
            {
                return new OperationResult<DateTimeOffset>(error, default);
            }
        }

        /// <inheritdoc/>
        public OperationResult<DateTimeOffset> ToUtc(DateTimeOffset dateTimeOffset)
        {
            try
            {
                return new OperationResult<DateTimeOffset>(default, TimeZoneInfo.ConvertTime(dateTimeOffset, TimeZoneProvider.GetUtc()));
            }
            catch (Exception error)
            {
                return new OperationResult<DateTimeOffset>(error, default);
            }
        }
    }
}
