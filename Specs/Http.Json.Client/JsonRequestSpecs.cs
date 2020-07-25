using NUnit.Framework;
using Sensemaking.Bdd;

namespace Sensemaking.Http.Json.Client.Specs
{
    public partial class JsonRequestSpecs : Specification
    {
        [Test]
        public void gets_json()
        {
            scenario(() =>
            {
                Given(a_url);
                And(some_headers);
                And(a_json_response_body);
                When(getting);
                Then(calls_the_url);
                And(it_accepts_json);
                And(uses_the_headers);
                And(provides_the_desrialized_response_body);
            });
        } 
    }
}