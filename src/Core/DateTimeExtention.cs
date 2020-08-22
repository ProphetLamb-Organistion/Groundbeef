using System.Linq;
using System.Collections.Generic;
using System;

namespace Groundbeef.Core
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class DateTimeExtention
    {
        /// <summary>
        /// Returns the first day in the week at 00:00:00.000 relative to the provided <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> used to determine to week.</param>
        /// <param name="startOfWeek">The <see cref="DayOfWeek"/> that counts as the first. Default is <see cref="DayOfWeek.Monday"/>.</param>
        /// <returns>The first day of the week at 00:00:00.000 relative to the provided <see cref="DateTime"/>.</returns>
        public static DateTime FirstOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.Date.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Returns the last day in the week at 23:59:59.999 relative to the provided <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> used to determine to week.</param>
        /// <param name="endOfWeek">The <see cref="DayOfWeek"/> that counts as the last. Default is <see cref="DayOfWeek.Sunday"/>.</param>
        /// <returns>The first day of the week at  23:59:59.999 relative to the provided <see cref="DateTime"/>.</returns>
        public static DateTime LastOfWeek(this DateTime dt, DayOfWeek endOfWeek = DayOfWeek.Sunday)
        {
            // Calcualte days until first day of next week
            int diff = (endOfWeek - dt.DayOfWeek) % 7 + 1;
            // Add days, then substract 1ms
            return dt.Date.AddDays(diff).Date.AddMilliseconds(-1);
        }

        /// <summary>
        /// Return the first day in the month at 00:00:00.000 relative to the provided <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> used to determine to year and month.</param>
        /// <returns>The first day in the month at 00:00:00.000 relative to the provided <see cref="DateTime"/>.</returns>
        public static DateTime FirstOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        /// <summary>
        /// Return the last day in the month at 23:59:59.999 relative to the provided <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dt">The <see cref="DateTime"/> used to determine to year and month.</param>
        /// <returns>The last day in the month at 23:59:59.999 relative to the provided <see cref="DateTime"/>.</returns>
        public static DateTime LastOfMonth(this DateTime dt)
        {
            return dt.FirstOfMonth().AddDays(DateTime.DaysInMonth(dt.Year, dt.Month)).AddMilliseconds(-1);
        }

        public static IEnumerable<TimeZoneInfo> GetTimzonesBetween(DateTimeOffset offset1, DateTimeOffset offset2, bool distinct = true)
        {
            return GetTimzonesBetween(offset1.Offset, offset2.Offset, distinct);
        }

        public static IEnumerable<TimeZoneInfo> GetTimzonesBetween(TimeSpan utcOffset1, TimeSpan utcOffset2, bool distinct = true)
        {
            return GetTimzonesBetween(utcOffset1.TotalHours.RoundInt32(), utcOffset2.TotalHours.RoundInt32(), distinct);
        }

        /// <summary>
        /// Enumerates all timezones between the UTC timezone offsets.
        /// </summary>
        /// <param name="utcOffset1">The first UTC offset.</param>
        /// <param name="utcOffset2">The second UTC offset.</param>
        /// <param name="distinct">Whether to eliminate all timezones with duplicate offsets.</param>
        public static IEnumerable<TimeZoneInfo> GetTimzonesBetween(int utcOffset1, int utcOffset2, bool distinct = false)
        {
            int min = Math.Min(utcOffset1, utcOffset2 % 24),
                max = Math.Max(utcOffset1 % 24, utcOffset2 % 24),
                delta = min - max;
            var hoursBetween = Enumerable.Range(min, delta).ToList();
            var zones = new TimeZoneInfo[delta];
            return distinct 
            ? TimeZoneInfo.GetSystemTimeZones()
                .Where(tzi => hoursBetween.Contains(tzi.BaseUtcOffset.TotalHours.RoundInt32()))
                .Distinct(new TimeZoneInfoOffsetIntegerComparer())
            : TimeZoneInfo.GetSystemTimeZones()
                .Where(tzi => hoursBetween.Contains(tzi.BaseUtcOffset.TotalHours.RoundInt32()));
        }

        private sealed class TimeZoneInfoOffsetIntegerComparer : IEqualityComparer<TimeZoneInfo>
        {
            public bool Equals(TimeZoneInfo x, TimeZoneInfo y)
            {
                return x.BaseUtcOffset.TotalHours.RoundInt32() - y.BaseUtcOffset.TotalHours.RoundInt32() == 0;
            }

            public int GetHashCode(TimeZoneInfo obj) => obj.GetHashCode();
        }
    }
}
