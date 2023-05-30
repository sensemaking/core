using System.Threading.Tasks;
using Flurl.Http;

namespace Sensemaking.Http.Json.Client
{
    public static class JsonRequest
    {
        static JsonRequest() => FlurlHttp.GlobalSettings.AllowedHttpStatusRange = "*";

        public static async Task<JsonResponse<T>> Get<T>(this string url, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).GetAsync();
            return await response.ToJsonResponse<T>();
        }

        public static async Task<JsonResponse<T>> Get<T>(this IFlurlClient client, string url, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).GetAsync();
            return await response.ToJsonResponse<T>();
        }

        public static async Task<JsonResponse> Put(this string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).PutAsync(payload.ToRequestBody());
            return await response.ToJsonResponse();
        }

        public static async Task<JsonResponse<T>> Put<T>(this string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).PutAsync(payload.ToRequestBody());
            return await response.ToJsonResponse<T>();
        }

        public static async Task<JsonResponse> Put(this IFlurlClient client, string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).PutAsync(payload.ToRequestBody());
            return await response.ToJsonResponse();
        }

        public static async Task<JsonResponse<T>> Put<T>(this IFlurlClient client, string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).PutAsync(payload.ToRequestBody());
            return await response.ToJsonResponse<T>();
        }

        public static async Task<JsonResponse> Delete(this string url, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).DeleteAsync();
            return await response.ToJsonResponse();
        }

        public static async Task<JsonResponse> Delete(this IFlurlClient client, string url, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).DeleteAsync();
            return await response.ToJsonResponse();
        }

        public static async Task<JsonResponse> Post(this string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).PostAsync(payload.ToRequestBody());
            return await response.ToJsonResponse();
        }

        public static async Task<JsonResponse<T>> Post<T>(this string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).PostAsync(payload.ToRequestBody());
            return await response.ToJsonResponse<T>();
        }
        
        public static async Task<JsonResponse> Post(this IFlurlClient client, string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).PostAsync(payload.ToRequestBody());
            return await response.ToJsonResponse();
        }
        
        public static async Task<JsonResponse<T>> Post<T>(this IFlurlClient client, string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).PostAsync(payload.ToRequestBody());
            return await response.ToJsonResponse<T>();
        }
    }
}