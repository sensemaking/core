using System;
using Sensemaking.Bdd;

namespace Sensemaking.Specs;

public partial class StringExtensionsSpecs
{
    private string source = null;

    protected override void before_each()
    {
        base.before_each();
        source = null;
    }

    private Action a_string_of(string s)
    {
        return () => source = s;
    }

    private void the_value_is_digital()
    {
        source.IsDigital().should_be_true();
    }

    private void the_value_is_not_digital()
    {
        source.IsDigital().should_be_false();
    }
}