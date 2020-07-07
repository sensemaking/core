using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Serialization;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;

namespace Sensemaking.Http
{
    public static class Json
    {
        static Json()
        {
            FlurlHttp.GlobalSettings.AllowedHttpStatusRange = "*";
        }

        public static async Task<JsonResponse<T>> GetAsync<T>(this string url, IDictionary<string, string>? headers = null)
        {
            headers = AddAcceptHeader(headers);
            var response = await url.WithHeaders(headers).GetAsync();
            var body = await response.Content.ReadAsStringAsync();
            var responseHeaders = response.Headers.ToDictionary(header => header.Key, header => string.Join(",", header.Value));
            return new JsonResponse<T>(body, response.StatusCode, response.ReasonPhrase, responseHeaders);
        }

        public static async Task<JsonResponse> PutAsync(this string url, object payload, IDictionary<string, string>? headers = null)
        {
            headers = AddAcceptHeader(headers);
            var response = await url.WithHeaders(headers).PutAsync(payload.ToRequestBody());
            var responseHeaders = response.Headers.ToDictionary(header => header.Key, header => string.Join(",", header.Value));
            return new JsonResponse(response.StatusCode, response.ReasonPhrase, responseHeaders);
        }

        public static async Task<JsonResponse> DeleteAsync(this string url, IDictionary<string, string>? headers = null)
        {
            headers = AddAcceptHeader(headers);
            var response = await url.WithHeaders(headers).DeleteAsync();
            var responseHeaders = response.Headers.ToDictionary(header => header.Key, header => string.Join(",", header.Value));
            return new JsonResponse(response.StatusCode, response.ReasonPhrase, responseHeaders);
        }

        public static async Task<JsonResponse> PostAsync(this string url, object payload, IDictionary<string, string>? headers = null)
        {
            headers = AddAcceptHeader(headers);
            var response = await url.WithHeaders(headers).PostAsync(payload.ToRequestBody());
            var responseHeaders = response.Headers.ToDictionary(header => header.Key, header => string.Join(",", header.Value));
            return new JsonResponse(response.StatusCode, response.ReasonPhrase, responseHeaders);
        }

        private static IDictionary<string, string> AddAcceptHeader(IDictionary<string, string>? headers)
        {
            headers ??= new Dictionary<string, string>();
            headers.Add("Accept", MediaType.Json);
            return headers;
        }

        private static StringContent ToRequestBody(this object payload)
        {
            return new StringContent(payload.Serialize(), Encoding.UTF8, MediaType.Json);
        }
    }
}