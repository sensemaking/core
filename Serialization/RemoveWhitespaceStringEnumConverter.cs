using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace System.Serialization
{
    public class RemoveWhitespaceStringEnumConverter : StringEnumConverter
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var value = reader.Value;
            if (value == null)
                return null;

            var enumType = Nullable.GetUnderlyingType(objectType) ?? objectType;
            return value switch
            {
                string stringValue => Parse(enumType, stringValue),
                byte byteValue => Convert(enumType, byteValue),
                short shortValue => Convert(enumType, shortValue),
                int intValue => Convert(enumType, intValue),
                long longValue => Convert(enumType, longValue),
                _ => throw new SerializationException(GetErrorMessage(value, enumType, Enum.GetNames(enumType)))
            };
        }
        
        private static object Parse(Type enumType, string value)
        {
            if (int.TryParse(value, out var intValue)) return Convert(enumType, intValue);

            try
            {
                return Enum.Parse(enumType, value.Replace(" ", string.Empty), true);
            }
            catch (Exception)
            {
                throw new SerializationException(GetErrorMessage(value, enumType, Enum.GetNames(enumType)));
            }
        }

        private static object Convert(Type enumType, object value)
        {
            var convertedValue = enumType.Convert(value) ;
            if (Enum.IsDefined(enumType, convertedValue)) 
                return Enum.ToObject(enumType, value);

            var allNames = Enum.GetNames(enumType);
            throw new SerializationException(GetErrorMessage(value, enumType, enumType.GetAllValues().Select((v, i) => $"{v} ({allNames[i]})").ToArray()));

        }

        private static string GetErrorMessage(object value, Type enumType, string[] allowable)
        {
            return $"{value} is not a valid {enumType.Name}. Allowable values are: {string.Join("; ", allowable)}.";
        }
    }

    internal static class EnumExtensions
    {
        public static IEnumerable<object> GetAllValues(this Type enumType)
        {
            return Enum.GetValues(enumType).Cast<object>().Select(x => System.Convert.ChangeType(x, Enum.GetUnderlyingType(enumType)));
        }

        public static object Convert(this Type enumType, object value)
        {
            return System.Convert.ChangeType(value, Enum.GetUnderlyingType(enumType));
        }
    }
}