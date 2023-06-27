using System.Net;
using System;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;

namespace Sensemaking.Http.Json.Client
{
    public static class JsonRequest
    {
       static JsonRequest() => FlurlHttp.GlobalSettings.AllowedHttpStatusRange = "*";
        
        private static async Task<JsonResponse> MakeRequest(this IFlurlClient client, string url, Func<IFlurlRequest, Task<IFlurlResponse>> request, params (string Name, string Value)[] headers)
        {
            var response = await request(client.Request(url).WithHeaders(headers.AddAcceptHeader()));
            var cookies = new CookieContainer().GetCookies(new Uri(url, UriKind.Absolute)).ToArray();
            return await response.ToJsonResponse(cookies);
        }
        
        private static async Task<JsonResponse<T>> MakeRequest<T>(this IFlurlClient client, string url, Func<IFlurlRequest, Task<IFlurlResponse>> request, params (string Name, string Value)[] headers)
        {
            var cookies = new CookieContainer();
            var response = await request(client.Request(url).WithHeaders(headers.AddAcceptHeader()));
            var responseCookies = cookies.GetCookies(new Uri(url, UriKind.Absolute)).ToArray();
            return await response.ToJsonResponse<T>(responseCookies);
        }
        
        private static async Task<JsonResponse> MakeRequest(this string url, Func<IFlurlRequest, Task<IFlurlResponse>> request, params (string Name, string Value)[] headers)
        {
            var cookies = new CookieContainer();
            var response = await request(url.WithHeaders(headers.AddAcceptHeader()));
            var responseCookies = cookies.GetCookies(new Uri(url, UriKind.Absolute)).ToArray();
            return await response.ToJsonResponse(responseCookies);
        }
        
        private static async Task<JsonResponse<T>> MakeRequest<T>(this string url, Func<IFlurlRequest, Task<IFlurlResponse>> request, params (string Name, string Value)[] headers)
        {
            var cookies = new CookieContainer();
            var response = await request(url.WithHeaders(headers.AddAcceptHeader()));
            var responseCookies = cookies.GetCookies(new Uri(url, UriKind.Absolute)).ToArray();
            return await response.ToJsonResponse<T>(responseCookies);
        }
        
        public static async Task<JsonResponse<T>> Get<T>(this string url, params (string Name, string Value)[] headers) => await url.MakeRequest<T>(request => request.GetAsync(), headers);
        public static async Task<JsonResponse<T>> Get<T>(this string url, (string Username, string Password) basicAuth, params (string Name, string Value)[] headers) => await url.MakeRequest<T>(request => request.GetAsync(), headers);
        public static async Task<JsonResponse<T>> Get<T>(this IFlurlClient client, string url, params (string Name, string Value)[] headers) => await client.MakeRequest<T>(url, request => request.GetAsync(), headers);
        public static async Task<JsonResponse> Put(this string url, object payload, params (string Name, string Value)[] headers) => await url.MakeRequest(request => request.PutAsync(payload.ToRequestBody()), headers);
        public static async Task<JsonResponse> Put(this IFlurlClient client, string url, object payload, params (string Name, string Value)[] headers) => await client.MakeRequest(url, request => request.PutAsync(payload.ToRequestBody()), headers);
        public static async Task<JsonResponse> Patch(this string url, object payload, params (string Name, string Value)[] headers) => await url.MakeRequest(request => request.PatchAsync(payload.ToRequestBody()), headers);
        public static async Task<JsonResponse> Patch(this IFlurlClient client, string url, object payload, params (string Name, string Value)[] headers) => await client.MakeRequest(url, request => request.PatchAsync(payload.ToRequestBody()), headers);
        public static async Task<JsonResponse> Delete(this string url, params (string Name, string Value)[] headers) => await url.MakeRequest(request => request.DeleteAsync(), headers);
        public static async Task<JsonResponse> Delete(this IFlurlClient client, string url, params (string Name, string Value)[] headers) => await client.MakeRequest(url, request => request.DeleteAsync(), headers);
        public static async Task<JsonResponse> Post(this string url, object payload, params (string Name, string Value)[] headers) => await url.MakeRequest(request => request.PostAsync(payload.ToRequestBody()), headers);
        public static async Task<JsonResponse> Post(this IFlurlClient client, string url, object payload, params (string Name, string Value)[] headers) => await client.MakeRequest(url, request => request.PostAsync(payload.ToRequestBody()), headers);
        public static async Task<JsonResponse<T>> Post<T>(this string url, object payload, params (string Name, string Value)[] headers) => await url.MakeRequest<T>(request => request.PostAsync(payload.ToRequestBody()), headers);
        public static async Task<JsonResponse<T>> Post<T>(this IFlurlClient client, string url, object payload, params (string Name, string Value)[] headers) => await client.MakeRequest<T>(url, request => request.PostAsync(payload.ToRequestBody()), headers);
    }
}