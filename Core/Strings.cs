
namespace System
{
    public static class Strings
    {
        public static bool IsNullOrEmpty(this string? source)
        {
            return string.IsNullOrWhiteSpace(source);
        }
    }
}
