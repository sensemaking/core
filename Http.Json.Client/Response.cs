﻿using System.Linq;
using System.Net;
using System.Net.Http;
using System.Serialization;
using System.Threading.Tasks;

namespace Sensemaking.Http.Json.Client
{
    internal static class Response
    {
        internal static async Task<JsonResponse<T>> ToJsonResponse<T>(this HttpResponseMessage response)
        {
            var body = await response.Content.ReadAsStringAsync();
            return new JsonResponse<T>(body, response);
        }

        internal static async Task<(HttpStatusCode Status, (string, string)[] headers, string body)> ParseContent(this HttpResponseMessage response)
        {
            var headers = response.Headers.Select(header => (header.Key, string.Join(",", header.Value))).ToArray();
            var body = await response.Content.ReadAsStringAsync();
        
            if (response.StatusCode.IsError())
                throw new ProblemException(response.StatusCode, headers, response.StatusCode.IsProblem() ? body.Deserialize<Problem>() : Problem.Empty);

            return (response.StatusCode, headers, body);
        }

    }
}