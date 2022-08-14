using System;
using System.Serialization;
using Newtonsoft.Json;
using NSubstitute;
using Sensemaking.Bdd;

namespace Sensemaking.Specs
{
    public partial class RemoveWhitespaceStringEnumConverterSpecs
    {
        private readonly RemoveWhitespaceStringEnumConverter the_converter = new RemoveWhitespaceStringEnumConverter();
        private JsonReader json_reader;

        private object the_value;
        private FakeEnum? the_result;
        private SerializationException the_serialization_exception;

        protected override void before_each()
        {
            base.before_each();
            json_reader = Substitute.For<JsonReader>();
            the_value = null;
            the_result = null;
            the_serialization_exception = null;
        }

        private void json_with_a_int_as_string_enum_value()
        {
            the_value = ((int) FakeEnum.WibbleWobble).ToString();
        }

        private void json_with_a_byte_enum_value()
        {
            the_value = (byte) FakeEnum.Wobble;
        }

        private void json_with_a_16_bit_integer_enum_value()
        {
            the_value = (short) FakeEnum.Wobble;
        }

        private void json_with_a_32_bit_integer_enum_value()
        {
            the_value = (int) FakeEnum.Wobble;
        }

        private void json_with_a_64_bit_integer_enum_value()
        {
            the_value = (long) FakeEnum.Wobble;
        }

        private void json_with_a_string_enum_value_containing_whitespace()
        {
            the_value = FakeEnum.WibbleWobble.ToString().Replace("e", "e ");
        }

        private void json_with_a_string_enum_value_containing_multiple_values()
        {
            the_value = (FakeEnum.WibbleWobble | FakeEnum.Wobble).ToString();
        }

        private void json_with_a_string_enum_value_in_a_different_case()
        {
            the_value = FakeEnum.WibbleWobble.ToString().ToLower();
        }

        private void json_with_a_null_enum_value()
        {
            the_value = null;
        }

        private void json_with_a_string_enum_value_that_does_not_exist()
        {
            the_value = "Chickens";
        }

        private void json_with_an_integer_enum_value_that_does_not_exist()
        {
            the_value = (int) FakeEnum.WibbleWobble + 999;
        }

        private void json_with_an_integer_as_string_enum_value_that_does_not_exist()
        {
            the_value = ((int) FakeEnum.WibbleWobble + 999).ToString();
        }

        private void json_with_a_decimal_enum_value()
        {
            the_value = ((int) FakeEnum.WibbleWobble) + 0.12m;
        }

        private void json_with_a_decimal_as_string_enum_value()
        {
            the_value = (((int) FakeEnum.WibbleWobble) + 0.12m).ToString();
        }

        private void json_with_a_boolean_enum_value()
        {
            the_value = true;
        }

        private void json_with_an_object_enum_value()
        {
            the_value = new { Property = "Boomshakala" };
        }

        private void converting()
        {
            catch_serialisation_exception(() => the_result = (FakeEnum) the_converter.ReadJson(json_reader, typeof(FakeEnum), null, null));
        }

        private void converting_to_nullable_enum()
        {
            catch_serialisation_exception(() => the_result = (FakeEnum?) the_converter.ReadJson(json_reader, typeof(FakeEnum?), null, null));
        }

        private void it_converts()
        {
            the_serialization_exception.should_be_null();
            the_result.should_be(Enum.Parse(typeof(FakeEnum), the_value.ToString().Replace(" ", ""), true));
        }

        private void it_converts_to_null()
        {
            the_serialization_exception.should_be_null();
            the_result.should_be_null();
        }

        [Flags]
        internal enum FakeEnum
        {
            Wibble = 1,
            Wobble = 2,
            WibbleWobble = 4
        }

        private void catch_serialisation_exception(Action action)
        {
            json_reader.Value.Returns(the_value);
            trying(action);
        }
    }
}