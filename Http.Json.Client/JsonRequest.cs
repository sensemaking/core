using System.Threading.Tasks;
using Flurl.Http;

namespace Sensemaking.Http.Json.Client
{
    public static class JsonRequest
    {
        static JsonRequest() => FlurlHttp.GlobalSettings.AllowedHttpStatusRange = "*";

        public static async Task<JsonResponse<T>> GetAsync<T>(this string url, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).GetAsync();
            return await response.ToJsonResponse<T>();
        }

        public static async Task<JsonResponse<T>> GetAsync<T>(this IFlurlClient client, string url, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).GetAsync();
            return await response.ToJsonResponse<T>();
        }

        public static async Task<JsonResponse> PutAsync(this string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).PutAsync(payload.ToRequestBody());
            return await response.ToJsonResponse();
        }

        public static async Task<JsonResponse<T>> PutAsync<T>(this string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).PutAsync(payload.ToRequestBody());
            return await response.ToJsonResponse<T>();
        }

        public static async Task<JsonResponse> PutAsync(this IFlurlClient client, string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).PutAsync(payload.ToRequestBody());
            return await response.ToJsonResponse();
        }

        public static async Task<JsonResponse<T>> PutAsync<T>(this IFlurlClient client, string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).PutAsync(payload.ToRequestBody());
            return await response.ToJsonResponse<T>();
        }

        public static async Task<JsonResponse> DeleteAsync(this string url, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).DeleteAsync();
            return await response.ToJsonResponse();
        }

        public static async Task<JsonResponse> DeleteAsync(this IFlurlClient client, string url, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).DeleteAsync();
            return await response.ToJsonResponse();
        }

        public static async Task<JsonResponse> PostAsync(this string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).PostAsync(payload.ToRequestBody());
            return await response.ToJsonResponse();
        }

        public static async Task<JsonResponse<T>> PostAsync<T>(this string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await url.WithHeaders(headers.AddAcceptHeader()).PostAsync(payload.ToRequestBody());
            return await response.ToJsonResponse<T>();
        }
        
        public static async Task<JsonResponse> PostAsync(this IFlurlClient client, string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).PostAsync(payload.ToRequestBody());
            return await response.ToJsonResponse();
        }
        
        public static async Task<JsonResponse<T>> PostAsync<T>(this IFlurlClient client, string url, object payload, params (string Name, string Value)[] headers)
        {
            var response = await client.Request(url).WithHeaders(headers.AddAcceptHeader()).PostAsync(payload.ToRequestBody());
            return await response.ToJsonResponse<T>();
        }
    }
}