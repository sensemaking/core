using NodaTime;
using NodaTime.Extensions;

namespace System
{
    public static class Date
    {
        private static ZonedClock TheUk => SystemClock.Instance.InZone(DateTimeZoneProviders.Tzdb["Europe/London"]);
        public static Func<LocalDate> Today = () => TheUk.GetCurrentDate();
        public static Func<LocalDateTime> Now = () => TheUk.GetCurrentLocalDateTime();

        public static bool FallsWithin(this LocalDate date, LocalDate start, LocalDate end)
        {
            return start <= date && end >= date;
        }

        public static uint Age(this LocalDate date)
        {
            var age = Date.Today().Year - date.Year;
            return (uint)(!date.PlusYears(age).IsFuture() || age == 0 ? age : age - 1);
        }

        public static bool IsFuture(this LocalDate date)
        {
            return Date.Today() < date;
        }

        public static bool IsFuture(this LocalDateTime date)
        {
            return Date.Now().Date < date.Date;
        }

        public static LocalDate ToLocalDate(this DateTime dateTime)
        {
            return new LocalDate(dateTime.Year, dateTime.Month, dateTime.Day);
        }
    }
}
