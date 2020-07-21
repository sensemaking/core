﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Sensemaking.Http.Json.Client
{
    public static class Request
    {

        static Request() => FlurlHttp.GlobalSettings.AllowedHttpStatusRange = "*";

        public static async Task<JsonResponse<T>> GetAsync<T>(this string url, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).GetAsync();
            return await response.ToJsonResponse<T>();
        }

        public static async Task<JsonResponse<T>> GetAsync<T>(this FlurlClient client, string url, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).GetAsync();
            return await response.ToJsonResponse<T>();
        }

        public static async Task<JsonResponse> PutAsync(this string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).PutAsync(payload.ToRequestBody());
            return new JsonResponse(response);
        }

        public static async Task<JsonResponse> PutAsync(this FlurlClient client, string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).PutAsync(payload.ToRequestBody());
            return new JsonResponse(response);
        }

        public static async Task<JsonResponse> DeleteAsync(this string url, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).DeleteAsync();
            return new JsonResponse(response);
        }

        public static async Task<JsonResponse> DeleteAsync(this FlurlClient client, string url, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).DeleteAsync();
            return new JsonResponse(response);
        }

        public static async Task<JsonResponse> PostAsync(this string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).PostAsync(payload.ToRequestBody());
            return new JsonResponse(response);
        }

        public static async Task<JsonResponse> PostAsync(this FlurlClient client, string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).PostAsync(payload.ToRequestBody());
            return new JsonResponse(response);
        }

        private static async Task<JsonResponse<T>> ToJsonResponse<T>(this HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            return new JsonResponse<T>(body, response);
        }

        private static (string, string)[] AddAcceptHeader(this IEnumerable<(string, string)> headers)
        {
            return headers.Concat(new (string, string)[] { ("Accept", MediaType.Json) }).ToArray();
        }

        private static StringContent ToRequestBody(this object payload)
        {
            return new StringContent(payload.Serialize(), Encoding.UTF8, MediaType.Json);
        }
    }
}