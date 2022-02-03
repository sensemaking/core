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
        public static Func<LocalDate> Today = () => Utc.GetCurrentDate();
        public static Func<LocalDateTime> Now = () => Utc.GetCurrentLocalDateTime();
        public static Func<DateTimeZone, LocalDate> TodayIn = (timeZone) => Utc.GetCurrentInstant().InZone(timeZone).Date;
        public static Func<DateTimeZone, LocalDateTime> NowIn = (timeZone) => Utc.GetCurrentInstant().InZone(timeZone).LocalDateTime;
    }
}
