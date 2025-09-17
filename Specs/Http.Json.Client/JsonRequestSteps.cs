using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Serialization;
using Flurl.Http;
using Flurl.Http.Content;
using Flurl.Http.Testing;
using Sensemaking.Bdd;
using Sensemaking.Http;
using Sensemaking.Http.Json.Client;

namespace Sensemaking.Specs
{
    public partial class JsonRequestSpecs
    {
        private const string url = "https://example.com/some-url";
        private static readonly (string Name, string Value)[] the_headers = {("h1", "h1value"), ("h2", "h2value")};
        private static readonly HttpStatusCode error_code = HttpStatusCode.InternalServerError;
        private static readonly Problem the_problem = new Problem("a problem", "an error");

        private HttpTest FakeHttp;
        private IFlurlClient client;
        private FakeBody the_body_to_respond_with;
        private HttpStatusCode the_status_to_respond_with;
        private FakeBody the_payload;
        private JsonResponse the_response;

        protected override void before_each()
        {
            base.before_each();
            client = null;
            FakeHttp = new HttpTest();
            the_body_to_respond_with = null;
            the_status_to_respond_with = HttpStatusCode.OK;
            the_response = null;
        }

        protected override void after_each()
        {
            FakeHttp.Dispose();
            base.after_each();
        }

        private void a_url()
        {
        }

        private void a_flurl_client()
        {
            client = new FlurlClient();
        }

        private void some_headers() { }

        private void a_payload()
        {
            the_payload = new FakeBody("Some payload");
        }

        private void the_response_has_a_body()
        {
            the_body_to_respond_with = new FakeBody("Some response");
            FakeHttp.RespondWith(() => new CapturedJsonContent(the_body_to_respond_with.Serialize()));
        }

        private void the_response_has_a_body_using_a_json_sub_type()
        {
            the_body_to_respond_with = new FakeBody("Some response");
            var content = new CapturedJsonContent(the_body_to_respond_with.Serialize());
            content.Headers.ContentType = new MediaTypeHeaderValue(MediaType.Siren);
            FakeHttp.RespondWith(() => content).RespondWith();
        }

        private void the_response_has_no_body()
        {
            the_body_to_respond_with = new FakeBody("Some response");
            the_status_to_respond_with = HttpStatusCode.NoContent;
            FakeHttp.RespondWith(() => new ByteArrayContent(Array.Empty<byte>()), (int) the_status_to_respond_with);
        }

        private void the_response_has_headers_and_a_body()
        {
            FakeHttp.RespondWith(() => new CapturedJsonContent(new FakeBody("Some response").Serialize()), headers: new {h1 = the_headers[0].Value, h2 = the_headers[1].Value});
        }

        private void the_response_has_headers_and_no_body()
        {
            the_status_to_respond_with = HttpStatusCode.NoContent;
            FakeHttp.RespondWith(() => new ByteArrayContent(Array.Empty<byte>()), (int) the_status_to_respond_with, new {h1 = the_headers[0].Value, h2 = the_headers[1].Value});
        }

        private void the_response_has_a_non_json_body()
        {
            FakeHttp.RespondWith("Some response");
        }

        private void the_response_has_no_body_but_has_a_content_type()
        {
            var content = new ByteArrayContent(Array.Empty<byte>());
            content.Headers.ContentType = new MediaTypeHeaderValue(MediaType.JsonProblem);
            FakeHttp.RespondWith(() => content, (int) error_code, new {h1 = the_headers[0].Value, h2 = the_headers[1].Value});
        }

        private void the_response_errors()
        {
            FakeHttp.RespondWith(() => new ByteArrayContent(Array.Empty<byte>()), (int) error_code, new {h1 = the_headers[0].Value, h2 = the_headers[1].Value});
        }

        private void the_response_errors_with_a_problem_and_headers()
        {
            var content = new CapturedJsonContent(the_problem.Serialize());
            content.Headers.ContentType = new MediaTypeHeaderValue(MediaType.JsonProblem);
            FakeHttp.RespondWith(() => content, (int) error_code, new {h1 = the_headers[0].Value, h2 = the_headers[1].Value});
        }

        private void getting()
        {
            the_response = (client != null ? client.Get<FakeBody>(url, the_headers) : url.Get<FakeBody>(the_headers)).Result;
        }

        private void putting()
        {
            the_response = (client != null ? client.Put(url, the_payload, the_headers) : url.Put(the_payload, the_headers)).Result;
        }

        private void patching()
        {
            the_response = (client != null ? client.Patch(url, the_payload, the_headers) : url.Patch(the_payload, the_headers)).Result;
        }

        private void deleting()
        {
            the_response = (client != null ? client.Delete(url, the_headers) : url.Delete(the_headers)).Result;
        }

        private void posting()
        {
            the_response = (client != null ? client.Post(url, the_payload, the_headers) : url.Post(the_payload, the_headers)).Result;
        }

        private void posting_expecting_a_response_body()
        {
            the_response = (client != null ? client.Post<FakeBody>(url, the_payload, the_headers) : url.Post<FakeBody>(the_payload, the_headers)).Result;
        }

        private void it_provides_the_desrialized_response_body()
        {
            (the_response as JsonResponse<FakeBody>).Body.Value.should_be(the_body_to_respond_with.Value);
        }

        private Action it_calls_the_url_using(HttpMethod verb)
        {
            return () => FakeHttp.ShouldHaveCalled(url).WithVerb(verb);
        }

        private void it_passes_the_payload_as_json()
        {
            FakeHttp.ShouldHaveMadeACall().WithContentType(MediaType.Json).WithRequestBody(the_payload.Serialize());
        }

        private void it_uses_the_headers()
        {
            the_headers.ForEach(h => FakeHttp.ShouldHaveMadeACall().WithHeader(h.Name, h.Value));
        }

        private void it_accepts_json()
        {
            FakeHttp.ShouldHaveMadeACall().WithHeader("Accept", MediaType.Json);
        }

        private void the_response_has_the_status_code()
        {
            the_response.Status.should_be(the_status_to_respond_with);
        }

        private void it_should_have_the_headers()
        {
            the_response.Headers.should_be(the_headers);
        }

        private void it_causes_a_problem_exception()
        {
            the_exception.should_be_instance_of<ProblemException>();
            var problem = (the_exception as ProblemException);
            informs<Exception>($"A problem has occured while making an http request:{Environment.NewLine}" +
                $"\tStatus: {problem.Status}{Environment.NewLine}" +
                $"\tProblem: {problem.Problem.Title}{Environment.NewLine}" +
                string.Join($"{Environment.NewLine}\t", problem.Problem.Errors) + Environment.NewLine);
        }

        private void the_exception_has_the_status_code()
        {
            (the_exception as ProblemException).Status.should_be(error_code);
        }

        private void the_exception_has_any_response_headers()
        {
            (the_exception as ProblemException).Headers.should_be(the_headers);
        }

        private void the_exception_should_have_the_problem()
        {
            (the_exception as ProblemException).Problem.should_be(the_problem);
        }
    }

    internal class FakeBody
    {
        public FakeBody(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}