using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public static async Task<JsonResponse<T>> GetAsync<T>(this string url, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).GetAsync();
            var body = await response.Content.ReadAsStringAsync();
            return new JsonResponse<T>(body, response);
        }

        public static async Task<JsonResponse> PutAsync(this string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).PutAsync(payload.ToRequestBody());
            return new JsonResponse(response);
        }

        public static async Task<JsonResponse> DeleteAsync(this string url, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).DeleteAsync();
            return new JsonResponse(response);
        }

        public static async Task<JsonResponse> PostAsync(this string url, object payload, params (string Name, string Value)[] headers)
        {
            headers.AddAcceptHeader();
            var response = await url.WithHeaders(headers.AddAcceptHeader()).PostAsync(payload.ToRequestBody());
            return new JsonResponse(response);
        }

        private static(string, string)[] AddAcceptHeader(this IEnumerable<(string, string)> headers)
        {
            return headers.Concat(new (string, string)[] { ("Accept", MediaType.Json)}).ToArray();
        }

        private static StringContent ToRequestBody(this object payload)
        {
            return new StringContent(payload.Serialize(), Encoding.UTF8, MediaType.Json);
        }
    }
}