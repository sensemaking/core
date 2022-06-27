using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl.Http;

namespace Sensemaking.Http.Json.Client
{
    internal static class Response
    {
        internal static async Task<JsonResponse<T>> ToJsonResponse<T>(this IFlurlResponse response)
        {
            var (status, headers, body) = await response.ResponseMessage.ParseContent();
            return new JsonResponse<T>(status, headers, body);
        }

        internal static async Task<JsonResponse> ToJsonResponse(this IFlurlResponse response)
        {
            var (status, headers, _) = await response.ResponseMessage.ParseContent();
            return new JsonResponse(status, headers);
        }

        private static async Task<(HttpStatusCode, IEnumerable<(string, string)>, string)> ParseContent(this HttpResponseMessage response)
        {
            var headers = response.Headers.Select(header => (header.Key, string.Join(",", header.Value)));
            var contentType = response.Content.Headers.ContentType;
            var body = await response.Content.ReadAsStringAsync();

            if(body.IsNullOrEmpty() && contentType != null)
                throw new Exception("The response has a Content-Type but no body.");

            if(!body.IsNullOrEmpty() && (contentType?.MediaType == null || !Regex.IsMatch(contentType.MediaType, @"application\/([\S]+\+)*json")))
                throw new Exception("The response does not have a Json content type.");

            if (response.IsError())
                throw new ProblemException(response.StatusCode, headers, response.IsProblem() 
                    ? body.Deserialize<Problem>() 
                    : new Problem("Endpoint did not provide a json problem. Error is any serialised response body that may have been provided.",
                        !body.IsNullOrEmpty() ? new [] { body } : Array.Empty<string>()));

            return (response.StatusCode, headers, body);
        }

    }
}