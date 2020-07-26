﻿
using System;
using System.Net;
using System.Net.Http;
using System.Serialization;
using System.Text.RegularExpressions;
using Flurl.Http;
using Flurl.Http.Testing;
using Microsoft.VisualBasic.CompilerServices;
using Sensemaking.Bdd;

namespace Sensemaking.Http.Json.Client.Specs
{
    public partial class JsonRequestSpecs
    {
        private const string url = "https://example.com/some-url";
        private static readonly (string Name, string Value)[] the_headers = { ("host", "localhost"), ("x-custom-header", "custom-value") };

        private HttpTest FakeHttp;
        private IFlurlClient client;
        private FakeBody the_body_to_respond_with;
        private FakeBody the_payload;
        private JsonResponse<FakeBody> the_get_response;
        private JsonResponse the_response;

        protected override void before_each()
        {
            base.before_each();
            FakeHttp = new HttpTest();
            client = null;
            the_body_to_respond_with = null;
            the_get_response = null;
            the_response = null;
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

        private void a_payload()
        {
            the_payload = new FakeBody("Some payload");
        }

        private void the_response_has_a_body()
        {
            the_body_to_respond_with = new FakeBody("Some response");
            FakeHttp.RespondWith(new StringContent(the_body_to_respond_with.Serialize()));
        }

        private void getting()
        {
            the_get_response = (client != null ? client.GetAsync<FakeBody>(url, the_headers): url.GetAsync<FakeBody>(the_headers)).Result;
        }

        private void putting()
        {
            the_response = (client != null ? client.PutAsync(url, the_payload, the_headers): url.PutAsync(the_payload, the_headers)).Result;
        }

        private void deleting()
        {
            the_response = (client != null ? client.DeleteAsync(url, the_headers): url.DeleteAsync(the_headers)).Result;
        }

        private void posting()
        {
            the_response = (client != null ? client.PostAsync(url, the_payload, the_headers): url.PostAsync(the_payload, the_headers)).Result;
        }

        private void it_provides_the_desrialized_response_body()
        {
            the_get_response.Body.Value.should_be(the_body_to_respond_with.Value);
        }

        private void it_calls_the_url_using(HttpMethod verb)
        {
            FakeHttp.ShouldHaveCalled(url).WithVerb(verb);
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
            the_response.Status.should_be(HttpStatusCode.OK);
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