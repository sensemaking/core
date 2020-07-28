using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sensemaking.Http.Json.Client
{
    internal static class Response
    {
        internal static async Task<JsonResponse<T>> ToJsonResponse<T>(this HttpResponseMessage response)
        {
            var (status, headers, body) = await response.ParseContent();
            return new JsonResponse<T>(status, headers, body);
        }

        internal static async Task<JsonResponse> ToJsonResponse(this HttpResponseMessage response)
        {
            var (status, headers, _) = await response.ParseContent();
            return new JsonResponse(status, headers);
        }

        private static async Task<(HttpStatusCode, IEnumerable<(string, string)>, string)> ParseContent(this HttpResponseMessage response)
        {
            var headers = response.Headers.Select(header => (header.Key, string.Join(",", header.Value)));
            var contentType = response.Content.Headers.ContentType;
            var body = await response.Content.ReadAsStringAsync();

            if(body.IsNullOrEmpty() && contentType != null)
                throw new Exception("The response has a Content-Type but no body.");

            if(!body.IsNullOrEmpty() && (contentType == null || !Regex.IsMatch(contentType.MediaType, @"application\/([\S]+\+)*json")))
                throw new Exception("The response has a body but it is not Json.");

            if (response.IsError())
                throw new ProblemException(response.StatusCode, headers, response.IsProblem() ? body.Deserialize<Problem>() : Problem.Empty);

            return (response.StatusCode, headers, body);
        }

    }
}