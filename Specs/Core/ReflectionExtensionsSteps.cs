using System;
using Sensemaking.Bdd;

namespace Sensemaking.Specs;

public partial class ReflectionExtensionsSpecs
{
    private TestReflection testReflection;
    private TestInheritedReflection testInheritedReflection;
    private const string test_value = "some val bla bla...";
    private const string test_field_value = "some field val bla bla...";

    private void an_object_with_a_privet_property()
    {
        testReflection = new TestReflection();
    }

    private void an_inherited_object_with_a_private_property()
    {
        testInheritedReflection = new TestInheritedReflection();
    }

    private void setting_the_reflected_value()
    {
        testReflection.SetReflectedValue("private_property", test_value);
        testReflection.SetReflectedValue("public_property_private_setter", test_value);
        testReflection.SetReflectedValue("field_to_set", test_field_value);
    }

    private void setting_the_inherited_reflected_value()
    {
        testInheritedReflection.SetReflectedValue("private_property", test_value);
        testInheritedReflection.SetReflectedValue("public_property_private_setter", test_value);
        testInheritedReflection.SetReflectedValue("field_to_set", test_field_value);
    }

    private void the_value_is_set()
    {
        testReflection.GetReflectedValue<string>("private_property").should_be(test_value);
        testReflection.public_property_private_setter.should_be(test_value);
        testReflection.GetFieldValue.should_be(test_field_value);
    }

    private void the_inherited_value_is_set()
    {
        testInheritedReflection.GetReflectedValue<string>("private_property").should_be(test_value);
        testInheritedReflection.public_property_private_setter.should_be(test_value);
        testInheritedReflection.GetFieldValue.should_be(test_field_value);
    }

    private class TestReflection
    {
        private string private_property { get; set; } = "";
        public string public_property_private_setter { get; private set; } = "";
        private string field_to_set = "";
        public string GetFieldValue => field_to_set;
    }

    private class TestInheritedReflection : TestReflection { }
}