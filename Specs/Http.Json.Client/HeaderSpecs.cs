using System;
using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Specs;

public partial class HeaderSpecs : Specification
{
        [Test]
        public void can_get_header_values()
        {
            Given(() => a_header("MyHeader", "MyValue"));
            When(() => getting_value_for("MyHeader"));
            Then(() => it_is("MyValue"));
        }

        [Test]
        public void generates_basic_authentication_header()
        {
            Given(a_header);
            When(adding_basic_authentication);
            Then(it_has_an_authorization_header);
            And(it_is_base_64_encoded);
        }
}