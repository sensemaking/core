using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using NodaTime.Text;

namespace System.Serialization
{
    public class DateAndDateTimeToLocalDateConverter : JsonConverter<LocalDate>
    {
        public override void WriteJson(JsonWriter writer, LocalDate value, Newtonsoft.Json.JsonSerializer serializer)
        {
            NodaConverters.LocalDateConverter.WriteJson(writer, value, serializer);
        }

        public override LocalDate ReadJson(JsonReader reader, Type objectType, LocalDate existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var dateString = reader.Value as string;
            if (dateString == null)
                return LocalDatePattern.Iso.Parse(dateString).Value;

            var index = dateString.ToUpper().IndexOf("T");
            if (index > 0)
                dateString = dateString.Substring(0, index);

            return LocalDatePattern.Iso.Parse(dateString).Value;
        }
    }

    public class DateAndDateTimeToNullableLocalDateConverter : JsonConverter<LocalDate?>
    {
        public override void WriteJson(JsonWriter writer, LocalDate? value, Newtonsoft.Json.JsonSerializer serializer)
        {
            NodaConverters.LocalDateConverter.WriteJson(writer, value, serializer);
        }

        public override LocalDate? ReadJson(JsonReader reader, Type objectType, LocalDate? existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var dateString = reader.Value as string;
            var index = dateString?.ToUpper().IndexOf("T");
            if (index.HasValue && index.Value > 0)
                dateString = dateString!.Substring(0, index.Value);


            var parseResult = LocalDatePattern.Iso.Parse(dateString);
            return parseResult.Success ? parseResult.Value : new LocalDate?();
        }
    }
}