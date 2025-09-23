using System;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Serialization;
using Newtonsoft.Json;
using NodaTime;
using Sensemaking.Bdd;
using JsonSerializer = System.Serialization.JsonSerializer;

namespace Sensemaking.Specs
{
    public partial class SerializationSpecs
    {
        public class Returns254Converter : JsonConverter
        {
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
            {
                return (uint) 254;
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(uint);
            }

            public override bool CanWrite => false;

            public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
            {
            }
        }

        private class DeserializedObject
        {
            public int[] EmptyArrayFromNull { get; set; }
            public string[] EmptyArrayFromMissing { get; set; }
            public ImmutableArray<int> EmptyImmutableArrayFromMissing { get; set; }
            public ImmutableList<int> EmptyImmutableListFromMissing { get; set; }
            public Money Money { get; set; }
            public Money MoneyAsInteger { get; set; }
            public Money MoneyAsLong { get; set; }
            public Money? NonNullMoney { get; set; }
            public Money? NullMoney { get; set; }
            public LocalDate Date { get; set; }
            public LocalDate DateWithATime { get; set; }
            public LocalDate? NullDate { get; set; }
            public LocalDate? NonNullNullableDate { get; set; }
            public LocalDate? NonNullNullableDateWithATime { get; set; }
            public int APrivateSetterNumber { get; private set; }
            public RemoveWhitespaceStringEnumConverterSpecs.FakeEnum StringEnumValue { get; set; }
            public uint Two54 { get; set; }
            [JsonProperty("JsonPropertyName")]
            public uint PropertyName { get; set; }
        }

        private DeserializedObject deserializedByExtensionMethod;
        private DeserializedObject deserializedByJsonSerializer;

        private static readonly DeserializedObject the_object = new()
        {
            Money = new Money(12.23m),
            MoneyAsInteger = new Money(11),
            MoneyAsLong = new Money(11111111111),
            NonNullMoney = new Money(34.45m),
            Date = Date.Today().Date,
            DateWithATime = Date.Today().Date.PlusDays(-1),
            NonNullNullableDate = Date.Today().Date.PlusDays(-2),
            NonNullNullableDateWithATime = Date.Today().Date.PlusDays(-3),
            StringEnumValue = RemoveWhitespaceStringEnumConverterSpecs.FakeEnum.Wobble
        };

        protected override void before_all()
        {
            base.before_all();
            Serialization.Configure(new Returns254Converter());
        }

        protected override void before_each()
        {
            base.before_each();
            deserializedByExtensionMethod = null;
            deserializedByJsonSerializer = null;
        }

        private void a_json_payload() { }

        private void an_object() { }

        private void deserializing()
        {
            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Sensemaking.Specs.Payload.json")))
                deserializedByExtensionMethod = reader.ReadToEnd().Deserialize<DeserializedObject>();

            using (var reader = new JsonTextReader(new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Sensemaking.Specs.Payload.json"))))
                deserializedByJsonSerializer = new JsonSerializer().Deserialize<DeserializedObject>(reader);
        }

        private void serializing_and_deserialising()
        {
            var serialize = the_object.Serialize();
            deserializedByExtensionMethod = serialize.Deserialize<DeserializedObject>();
        }

        private void json_property_name_is_respected()
        {
            the_object.Serialize().Contains("JsonPropertyName").should_be_true();
        }    
        
        private void private_setter_are_deserialized()
        {
            deserializedByExtensionMethod.APrivateSetterNumber.should_be(1);
            deserializedByJsonSerializer.APrivateSetterNumber.should_be(1);
        }

        private void null_arrays_are_deserialized_into_empty_arrays()
        {
            deserializedByExtensionMethod.EmptyArrayFromNull.should_be_empty();
            deserializedByJsonSerializer.EmptyArrayFromNull.should_be_empty();

            deserializedByExtensionMethod.EmptyArrayFromMissing.should_be_empty();
            deserializedByJsonSerializer.EmptyArrayFromMissing.should_be_empty();
        }

        private void missing_immutable_arrays_are_deserialized_into_empty_arrays()
        {
            deserializedByExtensionMethod.EmptyImmutableArrayFromMissing.should_be_empty();
            deserializedByJsonSerializer.EmptyImmutableArrayFromMissing.should_be_empty();
        }

        private void null_immutable_lists_are_deserialized_into_empty_lists()
        {
            deserializedByExtensionMethod.EmptyImmutableListFromMissing.should_be_empty();
            deserializedByJsonSerializer.EmptyImmutableListFromMissing.should_be_empty();
        }

        private void money_is_deserialized()
        {
            deserializedByExtensionMethod.Money.should_be(new Money(12.23m));
            deserializedByExtensionMethod.MoneyAsInteger.should_be(new Money(11));
            deserializedByExtensionMethod.MoneyAsLong.should_be(new Money(11111111111));
            deserializedByExtensionMethod.NonNullMoney.should_be(new Money(34.45m));
            deserializedByExtensionMethod.NullMoney.should_be_null();
            deserializedByJsonSerializer.Money.should_be(new Money(12.23m));
            deserializedByJsonSerializer.MoneyAsInteger.should_be(new Money(11));
            deserializedByJsonSerializer.MoneyAsLong.should_be(new Money(11111111111));
            deserializedByJsonSerializer.NonNullMoney.should_be(new Money(34.45m));
            deserializedByJsonSerializer.NullMoney.should_be_null();
        }

        private void dates_values_are_serialized_to_local_dates()
        {
            deserializedByExtensionMethod.Date.should_be(new LocalDate(1992, 5, 1));
            deserializedByJsonSerializer.Date.should_be(new LocalDate(1992, 5, 1));
        }

        private void date_time_values_can_be_deserialized_to_local_dates()
        {
            deserializedByExtensionMethod.DateWithATime.should_be(new LocalDate(1994, 2, 6));
            deserializedByJsonSerializer.DateWithATime.should_be(new LocalDate(1994, 2, 6));
        }

        private void dates_values_are_deserialized_to_nullable_local_dates()
        {
            deserializedByExtensionMethod.NonNullNullableDate.should_be(new LocalDate(1990, 11, 24));
            deserializedByJsonSerializer.NonNullNullableDate.should_be(new LocalDate(1990, 11, 24));
        }

        private void date_time_values_can_be_deserialized_to_nullable_local_dates()
        {
            deserializedByExtensionMethod.NonNullNullableDateWithATime.should_be(new LocalDate(1995, 12, 14));
            deserializedByJsonSerializer.NonNullNullableDateWithATime.should_be(new LocalDate(1995, 12, 14));
        }

        private void null_is_serialized_to_nullable_local_dates()
        {
            deserializedByExtensionMethod.NullDate.should_be_null();
            deserializedByJsonSerializer.NullDate.should_be_null();
        }

        private void removes_whitespace_from_string_enums()
        {
            deserializedByExtensionMethod.StringEnumValue.should_be(RemoveWhitespaceStringEnumConverterSpecs.FakeEnum.WibbleWobble);
            deserializedByJsonSerializer.StringEnumValue.should_be(RemoveWhitespaceStringEnumConverterSpecs.FakeEnum.WibbleWobble);
        }

        private void custom_converters_are_applied()
        {
            deserializedByExtensionMethod.Two54.should_be(254);
            deserializedByJsonSerializer.Two54.should_be(254);
        }

        private void the_object_is_reanimated()
        {
            deserializedByExtensionMethod.Money.should_be(the_object.Money);
            deserializedByExtensionMethod.MoneyAsInteger.should_be(the_object.MoneyAsInteger);
            deserializedByExtensionMethod.MoneyAsLong.should_be(the_object.MoneyAsLong);
            deserializedByExtensionMethod.NonNullMoney.should_be(the_object.NonNullMoney);
            deserializedByExtensionMethod.NullMoney.should_be(the_object.NullMoney);
            deserializedByExtensionMethod.Date.should_be(the_object.Date);
            deserializedByExtensionMethod.DateWithATime.should_be(the_object.DateWithATime);
            deserializedByExtensionMethod.NullDate.should_be(the_object.NullDate);
            deserializedByExtensionMethod.NonNullNullableDate.should_be(the_object.NonNullNullableDate);
            deserializedByExtensionMethod.NonNullNullableDateWithATime.should_be(the_object.NonNullNullableDateWithATime);
            deserializedByExtensionMethod.StringEnumValue.should_be(the_object.StringEnumValue);
        }
    }
}