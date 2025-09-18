using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Specs;

[TestFixture]
public partial class FlaggedEnumSpecs : Specification
{
    [Test]
    public void valid_flagged_enum()
    {
        Given(a_valid_flagged_enum_value);
        Then(it_is_recognised_as_valid);
    }

    [Test]
    public void zero_value()
    {
        Given(a_zero_value);
        Then(it_is_recognised_as_valid);
    }

    [Test]
    public void invalid_flagged_enum()
    {
        Given(an_invalid_flagged_enum_value);
        Then(it_is_recognised_as_invalid);
    }
}