using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Specs;

[TestFixture]
public partial class StringExtensionsSpecs : Specification
{
    [Test]
    public void is_a_string_digital()
    {
        scenario(() =>
        {
            Given(a_string_of("10000"));
            Then(the_value_is_digital);
        });
        
        scenario(() =>
        {
            Given(a_string_of("qwerty"));
            Then(the_value_is_not_digital);
        });
        
        scenario(() =>
        {
            Given(a_string_of(null));
            Then(the_value_is_not_digital);
        });
        
        scenario(() =>
        {
            Given(a_string_of(""));
            Then(the_value_is_not_digital);
        });
    }
}