using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Specs
{
    public partial class SerializationSpecs : Specification
    {
        [Test]
        public void deserializes_private_setters()
        {
            Given(a_json_payload);
            When(deserializing);
            Then(private_setter_properties_are_deserialized);
        }

        [Test]
        public void deserializes_null_into_empty_arrays()
        {
            Given(a_json_payload);
            When(deserializing);
            Then(null_arrays_properties_are_deserialized_into_empty_arrays);
        }

        [Test]
        public void deserializes_money()
        {
            Given(a_json_payload);
            When(deserializing);
            Then(money_is_deserialized);
        }

        [Test]
        public void deserializes_dates_and_date_times_to_local_dates()
        {
            Given(a_json_payload);
            When(deserializing);
            Then(dates_values_are_serialized_to_local_dates);
            And(date_time_values_can_be_deserialized_to_local_dates);
        }

        [Test]
        public void deserializes_dates_and_date_times_to_nullable_local_dates()
        {
            Given(a_json_payload);
            When(deserializing);
            Then(dates_values_are_deserialized_to_nullable_local_dates);
            And(date_time_values_can_be_deserialized_to_nullable_local_dates);
            And(null_is_serialized_to_nullable_local_dates);
        }

        [Test]
        public void deserializes_string_enum_values_containing_whitespace()
        {
            Given(a_json_payload);
            When(deserializing);
            Then(removes_whitespace_from_string_enums);
        }

        [Test]
        public void applies_custom_converters()
        {
            Given(a_json_payload);
            When(deserializing);
            Then(custom_converters_are_applied);
        }

        [Test]
        public void serializes_into_json_that_can_be_deserialized()
        {
            Given(an_object);
            When(serializing_and_deserialising);
            Then(the_object_is_reanimated);
        }
    }
}