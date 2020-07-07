using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Serialization;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;

namespace Sensemaking.Http
{
    public static class FlurlExtensions
    {
        public static async Task<T> GetAsync<T>(this string url, IDictionary<string, string>? headers = null)
        {
            var response = await url.WithHeaders(headers).GetAsync();
            var body = await response.Content.ReadAsStringAsync();
            return body.Deserialize<T>();
        }

        public static async Task<HttpResponseMessage> PutAsync(this string url, object payload, IDictionary<string, string>? headers = null)
        {
            return await url.WithHeaders(headers).PutAsync(new StringContent(payload.Serialize(), Encoding.UTF8, MediaType.Json));
        }
        
        public static async Task<HttpResponseMessage> DeleteAsync(this string url, IDictionary<string, string>? headers = null)
        {
            return await url.WithHeaders(headers).DeleteAsync();
        }
        
        public static async Task<HttpResponseMessage> PostAsync(this string url, object payload, IDictionary<string, string>? headers = null)
        {
            return await url.WithHeaders(headers).PostAsync(new StringContent(payload.Serialize(), Encoding.UTF8, MediaType.Json));
        }
    }
}