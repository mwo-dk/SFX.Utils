﻿using SFX.ROP.CSharp;
using System;
using static SFX.ROP.CSharp.Library;

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
                return Succeed(TimeZoneInfo.FindSystemTimeZoneById(id));
            }
            catch (Exception error)
            {
                return Fail<TimeZoneInfo>(error);
            }
        }
    }
}
