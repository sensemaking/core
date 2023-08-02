using System.Serialization;
using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Specs
{
    public partial class RemoveWhitespaceStringEnumConverterSpecs : Specification
    {
        [Test]
        public void converts_to_an_enum_for_integer_values()
        {
            scenario(() =>
            {
                Given(json_with_a_int_as_string_enum_value);
                When(converting);
                Then(it_converts);
            });

            scenario(() =>
            {
                Given(json_with_a_byte_enum_value);
                When(converting);
                Then(it_converts);
            });

            scenario(() =>
            {
                Given(json_with_a_16_bit_integer_enum_value);
                When(converting);
                Then(it_converts);
            });

            scenario(() =>
            {
                Given(json_with_a_32_bit_integer_enum_value);
                When(converting);
                Then(it_converts);
            });

            scenario(() =>
            {
                Given(json_with_a_64_bit_integer_enum_value);
                When(converting);
                Then(it_converts);
            });
        }

        [Test]
        public void converts_to_an_enum_if_a_string_value_contains_whitespace()
        {
            Given(json_with_a_string_enum_value_containing_whitespace);
            When(converting);
            Then(it_converts);
        }

        [Test]
        public void converts_to_an_enum_if_a_string_value_is_different_case()
        {
            Given(json_with_a_string_enum_value_in_a_different_case);
            When(converting);
            Then(it_converts);
        }

        [Test]
        public void converts_to_an_enum_if_a_string_value_contains_multiple_values()
        {
            Given(json_with_a_string_enum_value_containing_multiple_values);
            When(converting);
            Then(it_converts);
        }

        [Test]
        public void converts_to_nullable_enums()
        {
            scenario(() =>
            {
                Given(json_with_a_string_enum_value_containing_whitespace);
                When(converting_to_nullable_enum);
                Then(it_converts);
            });

            scenario(() =>
            {
                Given(json_with_a_null_enum_value);
                When(converting_to_nullable_enum);
                Then(it_converts_to_null);
            });
        }

        [Test]
        public void fails_to_convert_strings_that_are_not_included_in_the_enum_informing_of_allowable_values()
        {
            Given(json_with_a_string_enum_value_that_does_not_exist);
            When(converting);
            Then(informs<SerializationException>($"{the_value} is not a valid {nameof(FakeEnum)}. Allowable values are: Wibble; Wobble; WibbleWobble."));
        }

        [Test]
        public void fails_to_convert_integers_that_are_not_included_in_the_enum_informing_of_allowable_values()
        {
            Given(json_with_an_integer_enum_value_that_does_not_exist);
            When(converting);
            Then(informs<SerializationException>($"{the_value} is not a valid {nameof(FakeEnum)}. Allowable values are: 1 (Wibble); 2 (Wobble); 4 (WibbleWobble)."));
        }

        [Test]
        public void fails_to_convert_integers_as_strings_that_are_not_included_in_the_enum_informing_of_allowable_values()
        {
            Given(json_with_an_integer_as_string_enum_value_that_does_not_exist);
            When(converting);
            Then(informs<SerializationException>($"{the_value} is not a valid {nameof(FakeEnum)}. Allowable values are: 1 (Wibble); 2 (Wobble); 4 (WibbleWobble)."));
        }

        [Test]
        public void fails_to_convert_other_json_types_informing_of_allowable_values()
        {
            scenario(() =>
            {
                Given(json_with_a_decimal_enum_value);
                When(converting);
                Then(informs<SerializationException>($"{the_value} is not a valid {nameof(FakeEnum)}. Allowable values are: Wibble; Wobble; WibbleWobble."));
            });

            scenario(() =>
            {
                Given(json_with_a_decimal_as_string_enum_value);
                When(converting);
                Then(informs<SerializationException>($"{the_value} is not a valid {nameof(FakeEnum)}. Allowable values are: Wibble; Wobble; WibbleWobble."));
            });

            scenario(() =>
            {
                Given(json_with_a_boolean_enum_value);
                When(converting);
                Then(informs<SerializationException>($"{the_value} is not a valid {nameof(FakeEnum)}. Allowable values are: Wibble; Wobble; WibbleWobble."));
            });

            scenario(() =>
            {
                Given(json_with_an_object_enum_value);
                When(converting);
                Then(informs<SerializationException>($"{the_value} is not a valid {nameof(FakeEnum)}. Allowable values are: Wibble; Wobble; WibbleWobble."));
            });
        }
    }
}