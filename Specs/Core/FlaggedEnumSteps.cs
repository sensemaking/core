using System;
using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Specs;

public partial class FlaggedEnumSpecs
{
    [Flags]
    public enum AnEnum
    {
        None = 0,
        One = 1,
        Two = 2,
        Four = 4
    }

    public AnEnum theEnum;

    private void a_valid_flagged_enum_value()
    {
        theEnum = AnEnum.One | AnEnum.Four;
    }

    private void a_zero_value()
    {
        theEnum = AnEnum.None;
    }

    private void an_invalid_flagged_enum_value()
    {
        theEnum = (AnEnum) 21;
    }
       
    private void it_is_recognised_as_valid()
    {
        Assert.That(theEnum.IsFlagDefined());
    }

    private void it_is_recognised_as_invalid()
    {
        theEnum.IsFlagDefined().should_be_false();
    }
}