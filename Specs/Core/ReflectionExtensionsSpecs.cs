using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Specs;

[TestFixture]
public partial class ReflectionExtensionsSpecs : Specification
{
    [Test]
    public void set_private_value()
    {
        Given(an_object_with_a_privet_property);
        When(setting_the_reflected_value);
        Then(the_value_is_set);
    }

    [Test]
    public void set_inherited_private_value()
    {
        Given(an_inherited_object_with_a_private_property);
        When(setting_the_inherited_reflected_value);
        Then(the_inherited_value_is_set);
    }
}