using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace System.Serialization;

public class MoneyConverter : JsonConverter<Money>
{
    public override void WriteJson(JsonWriter writer, Money value, Newtonsoft.Json.JsonSerializer serializer)
    {
        new JValue((decimal) value).WriteTo(writer);
    }

    public override Money ReadJson(JsonReader reader, Type objectType, Money existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        return reader.Value switch
        {
            string stringAmount => new Money(decimal.Parse(stringAmount)),
            decimal decimalAmount => new Money(decimalAmount),
            double doubleAmount => new Money(doubleAmount),
            int intAmount => new Money(intAmount),
            long longAmount => new Money(longAmount),
            _ => throw new SerializationException($"Cannot convert {reader.Value?.GetType()} , {reader.Value} to Money")
        };
    }
}

public class NullableMoneyConverter : JsonConverter<Money?>
{
    public override void WriteJson(JsonWriter writer, Money? value, Newtonsoft.Json.JsonSerializer serializer)
    {
        new JValue((decimal?) value).WriteTo(writer);
    }

    public override Money? ReadJson(JsonReader reader, Type objectType, Money? existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        return reader.Value switch
        {
            string stringAmount => new Money(decimal.Parse(stringAmount)),
            decimal decimalAmount => new Money(decimalAmount),
            double doubleAmount => new Money(doubleAmount),
            int intAmount => new Money(intAmount),
            long longAmount => new Money(longAmount),
            null => null,
            _ => throw new SerializationException($"Cannot convert {reader.Value?.GetType()} , {reader.Value} to Money")
        };
    }
}