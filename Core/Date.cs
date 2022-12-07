using NodaTime;
using NodaTime.Extensions;

namespace System
{
    public static class TimeZones
    {
        public static readonly DateTimeZone Utc = DateTimeZone.Utc;
        public static readonly DateTimeZone TheUk = DateTimeZoneProviders.Tzdb["Europe/London"];
    }

    public static class Date
    {
        private static readonly ZonedClock Utc = SystemClock.Instance.InZone(DateTimeZone.Utc);
        public static Func<OffsetDate> Today = () => Utc.GetCurrentOffsetDateTime().ToOffsetDate();
        public static Func<DateTimeZone, OffsetDate> TodayIn = (timeZone) => Utc.GetCurrentInstant().InZone(timeZone).ToOffsetDateTime().ToOffsetDate();
    }

    public static class Time
    {
        private static readonly ZonedClock Utc = SystemClock.Instance.InZone(DateTimeZone.Utc);
        public static Func<OffsetDateTime> Now = () => Utc.GetCurrentOffsetDateTime();
        public static Func<DateTimeZone, OffsetDateTime> NowIn = (timeZone) => Utc.GetCurrentInstant().InZone(timeZone).ToOffsetDateTime();
    }
}
