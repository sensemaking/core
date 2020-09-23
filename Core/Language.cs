using System.Text.RegularExpressions;

namespace System
{
    public static class Language
    {
        public static bool IsNullOrEmpty(this string? source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        public static bool IsBase64(this string? source)
        {
            source = source?.Trim();
            return  !source.IsNullOrEmpty() && (source!.Length % 4 == 0) && Regex.IsMatch(source, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }
    }
}