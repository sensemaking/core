
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

        [Test]
        public void informs_if_a_get_has_response_with_no_body()
        {
            scenario(() =>
            {
                Given(a_url);
                And(some_headers);
                When(() => trying(getting));
                Then(() => informs("The response to a GET request did not include a body."));
            });
        }

        [Test]
        public void response_includes_the_status_code()
        {
            scenario(() =>
            {
                Given(a_url);
                And(the_response_has_a_body);
                When(getting);
                Then(the_response_has_the_status_code);
            });

            scenario(() =>
            {
                Given(a_url);
                When(putting);
                Then(the_response_has_the_status_code);
            });

            scenario(() =>
            {
                Given(a_url);
                When(deleting);
                Then(the_response_has_the_status_code);
            });

            scenario(() =>
            {
                Given(a_url);
                When(posting);
                Then(the_response_has_the_status_code);
            });
        }

        [Test]
        public void responses_include_any_headers()
        {
           scenario(() =>
           {
               Given(a_url);
               And(the_response_has_a_body_and_headers);
               When(() => trying(getting));
               Then(it_should_have_the_headers);
           });
            
           scenario(() =>
           {
               Given(a_url);
               And(the_response_has_headers);
               When(() => trying(putting));
               Then(it_should_have_the_headers);
           });
            
           scenario(() =>
           {
               Given(a_url);
               And(the_response_has_headers);
               When(() => trying(deleting));
               Then(it_should_have_the_headers);
           });
            
           scenario(() =>
           {
               Given(a_url);
               And(the_response_has_headers);
               When(() => trying(posting));
               Then(it_should_have_the_headers);
           });
        }

        [Test]
        public void errors_cause_problem_exception()
        {
            scenario(() =>
            {
                Given(a_url);
                And(the_response_errors);
                When(() => trying(getting));
                Then(it_causes_a_problem_exception);
                And(the_exception_has_the_status_code);
                And(the_exception_has_any_response_headers);
            });

            scenario(() =>
            {
                Given(a_url);
                And(the_response_errors);
                When(() => trying(putting));
                Then(it_causes_a_problem_exception);
                And(the_exception_has_the_status_code);
                And(the_exception_has_any_response_headers);
            });

            scenario(() =>
            {
                Given(a_url);
                And(the_response_errors);
                When(() => trying(deleting));
                Then(it_causes_a_problem_exception);
                And(the_exception_has_the_status_code);
                And(the_exception_has_any_response_headers);
            });

            scenario(() =>
            {
                Given(a_url);
                And(the_response_errors);
                When(() => trying(posting));
                Then(it_causes_a_problem_exception);
                And(the_exception_has_the_status_code);
                And(the_exception_has_any_response_headers);
            });
        }

        [Test]
        public void errors_with_problem_content_cause_problem_exception_with_problem_details()
        {
           scenario(() =>
           {
               Given(a_url);
               And(the_response_errors_with_a_problem_and_headers);
               When(() => trying(getting));
               Then(the_exception_should_have_the_problem);
           });

           scenario(() =>
           {
               Given(a_url);
               And(the_response_errors_with_a_problem_and_headers);
               When(() => trying(putting));
               Then(the_exception_should_have_the_problem);
           });

           scenario(() =>
           {
               Given(a_url);
               And(the_response_errors_with_a_problem_and_headers);
               When(() => trying(deleting));
               Then(the_exception_should_have_the_problem);
           });

           scenario(() =>
           {
               Given(a_url);
               And(the_response_errors_with_a_problem_and_headers);
               When(() => trying(posting));
               Then(the_exception_should_have_the_problem);
           });
        }
    }
}