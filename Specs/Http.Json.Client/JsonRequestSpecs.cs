using System.Net.Http;
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
                Then(() => it_calls_the_url_using(HttpMethod.Get));
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
                Then(() => it_calls_the_url_using(HttpMethod.Get));
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
                Then(() => it_calls_the_url_using(HttpMethod.Put));
                And(it_passes_the_payload_as_json);
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
                Then(() => it_calls_the_url_using(HttpMethod.Put));
                And(it_passes_the_payload_as_json);
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
                Then(() => it_calls_the_url_using(HttpMethod.Delete));
                And(it_uses_the_headers);
                And(it_accepts_json);
            });

            scenario(() =>
            {
                Given(a_flurl_client);
                And(a_url);
                And(some_headers);
                When(deleting);
                Then(() => it_calls_the_url_using(HttpMethod.Delete));
                And(it_uses_the_headers);
                And(it_accepts_json);
            });
        } 

        [Test]
        public void posts_json()
        {
            scenario(() =>
            {
                Given(a_url);
                And(some_headers);
                And(a_payload);
                When(posting);
                Then(() => it_calls_the_url_using(HttpMethod.Post));
                And(it_passes_the_payload_as_json);
                And(it_uses_the_headers);
                And(it_accepts_json);
            });

            scenario(() =>
            {
                Given(a_flurl_client);
                And(a_url);
                And(some_headers);
                And(a_payload);
                When(posting);
                Then(() => it_calls_the_url_using(HttpMethod.Post));
                And(it_passes_the_payload_as_json);
                And(it_uses_the_headers);
                And(it_accepts_json);
            });
        } 
    }
}