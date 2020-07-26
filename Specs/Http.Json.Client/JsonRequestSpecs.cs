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
                And(the_response_has_a_body);
                When(getting);
                Then(it_gets_from_the_url);
                And(it_uses_the_headers);
                And(it_accepts_json);
                And(it_provides_the_desrialized_response_body);
            });

            scenario(() =>
            {
                Given(a_flurl_client);
                And(a_url);
                And(some_headers);
                And(the_response_has_a_body);
                When(getting);
                Then(it_gets_from_the_url);
                And(it_uses_the_headers);
                And(it_accepts_json);
                And(it_provides_the_desrialized_response_body);
            });
        } 

        [Test]
        public void puts_json()
        {
            scenario(() =>
            {
                Given(a_url);
                And(some_headers);
                And(a_payload);
                When(putting);
                Then(it_puts_the_payload_to_the_url);
                And(it_is_json_content);
                And(it_uses_the_headers);
                And(it_accepts_json);
            });

            scenario(() =>
            {
                Given(a_flurl_client);
                And(a_url);
                And(some_headers);
                And(a_payload);
                When(putting);
                Then(it_puts_the_payload_to_the_url);
                And(it_is_json_content);
                And(it_uses_the_headers);
                And(it_accepts_json);
            });
        } 

        [Test]
        public void deletes()
        {
            scenario(() =>
            {
                Given(a_url);
                And(some_headers);
                When(deleting);
                Then(it_deletes_from_the_url);
                And(it_uses_the_headers);
                And(it_accepts_json);
            });

            scenario(() =>
            {
                Given(a_flurl_client);
                And(a_url);
                And(some_headers);
                When(deleting);
                Then(it_deletes_from_the_url);
                And(it_uses_the_headers);
                And(it_accepts_json);
            });
        } 
    }
}