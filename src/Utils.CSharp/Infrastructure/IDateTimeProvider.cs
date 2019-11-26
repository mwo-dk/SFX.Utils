﻿using System;

namespace SFX.Utils.Infrastructure
{
    /// <summary>
    /// Interface describing the capability to resolve current <see cref="DateTimeOffset"/> in UTC
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Get "now" in UTC
        /// </summary>
        /// <returns>Now in UTC</returns>
        DateTimeOffset GetUtcNow();
    }
}