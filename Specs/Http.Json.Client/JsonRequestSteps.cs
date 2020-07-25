
using System;
using System.Net.Http;
using System.Serialization;
using Flurl.Http;
using Flurl.Http.Testing;
using Sensemaking.Bdd;

namespace Sensemaking.Http.Json.Client.Specs
{
    public partial class JsonRequestSpecs
    {
        private const string url = "https://example.com/some-url";
        private static readonly (string Name, string Value)[] headers = { ("host", "localhost"), ("x-custom-header", "custom-value") };

        private HttpTest FakeHttp;
        private IFlurlClient client;
        private FakeBody the_body_to_respond_with;
        private JsonResponse<FakeBody> the_get_response;

        protected override void before_each()
        {
            base.before_each();
            FakeHttp = new HttpTest();
            client = null;
            the_body_to_respond_with = null;
            the_get_response = null;
        }

        protected override void after_each()
        {
            FakeHttp.Dispose();
            base.after_each();
        }

        private void a_url() { }

        private void a_flurl_client()
        {
            client = new FlurlClient();
        }

        private void some_headers() { }

        private void a_json_response_body()
        {
            the_body_to_respond_with = new FakeBody("Some value");
            FakeHttp.RespondWith(new StringContent(the_body_to_respond_with.Serialize()));
        }

        private void getting()
        {
            the_get_response = (client != null ? client.GetAsync<FakeBody>(url, headers): url.GetAsync<FakeBody>(headers)).Result;
        }

        private void calls_the_url()
        {
            FakeHttp.ShouldHaveCalled(url);
        }

        private void it_accepts_json()
        {
            FakeHttp.ShouldHaveMadeACall().WithHeader("Accept", MediaType.Json);
        }

        private void uses_the_headers()
        {
            headers.ForEach(h => FakeHttp.ShouldHaveMadeACall().WithHeader(h.Name, h.Value));
        }

        private void provides_the_desrialized_response_body()
        {
            the_get_response.Body.Value.should_be(the_body_to_respond_with.Value);
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