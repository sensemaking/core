using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class Flags
    {
        public static bool Includes(this Enum @this, Enum that)
        {
            return @this.HasFlag(that);
        }

        public static IEnumerable<Enum> Included(this Enum value)
        {
            return Enum.GetValues(value.GetType()).Cast<Enum>().Where(value.Includes);
        }

        public static bool IsDefined(this Enum value)
        {
            return Enum.IsDefined(value.GetType(), value);
        }

        public static bool IsFlagDefined(this Enum value)
        {
            return !int.TryParse(value.ToString(), out _);
        }
    }
}