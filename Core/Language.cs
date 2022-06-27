using System.Text.RegularExpressions;

namespace System
{
    public static class Language
    {
        public static bool IsNullOrEmpty(this string? source)
        {
            return string.IsNullOrWhiteSpace(source);
        }
    }
}