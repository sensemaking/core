using System;
using Sensemaking.Bdd;

namespace Fdb.Rx.Testing.Core.LanguageExtensions;

public partial class EnumerableEquivalenceSpecs
{
    private object[] the_array;
    private object[] the_other_array;

    protected override void before_each()
    {
        base.before_each();
        the_array = null;
        the_other_array = null;
    }

    private void an_array(object[] array)
    {
        the_array = array;
    }

    private void another(object[] array)
    {
        the_other_array = array;
    }

    private void has_same_contents(bool equivalence)
    {
        the_array.HasSameContents(the_other_array).should_be(equivalence);
    }

    private void has_same_contents_in_the_same_order(bool equivalence)
    {
        the_array.HasSameContentsInSameOrder(the_other_array).should_be(equivalence);
    }
}