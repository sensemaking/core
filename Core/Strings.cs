
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace System;

public static class Strings
{
    public static bool IsNullOrEmpty(this string? source)
    {
        return string.IsNullOrWhiteSpace(source);
    }

    public static bool IsBase64(this string? source)
    {
        source = source?.Trim();
        return !source.IsNullOrEmpty() && (source!.Length % 4 == 0) && Regex.IsMatch(source, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
    }

    public static string Base64Encode(this string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }
    
    public static bool IsDigital(this string source)
    {
        return !source.IsNullOrEmpty() && source.All(char.IsDigit);
    }
}
