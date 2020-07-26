using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Serialization;

namespace Sensemaking.Http.Json.Client
{
    public partial class JsonResponse
    {
        public HttpStatusCode Status { get; }
        public (string Name, string Value)[] Headers { get; }

        internal JsonResponse(HttpResponseMessage response)
        {
            Status = response.StatusCode;
            Headers = response.Content.Headers.Select(header => (header.Key, string.Join(",", header.Value))).ToArray();
        }

        public static implicit operator HttpStatusCode (JsonResponse response)
        {
            return response.Status;
        }
    }

    public class JsonResponse<T> : JsonResponse
    {
        public T Body { get; }
          
        internal JsonResponse(string body, HttpResponseMessage response) : base(response)
        {
            if (response.StatusCode.IsError())
                throw new ProblemException(response.StatusCode, Headers, response.StatusCode.IsProblem() ? body.Deserialize<Problem>() : Problem.Empty);

            if (body.IsNullOrEmpty())
                throw new Exception("The response to a GET request did not include a body.");

            Body = body.Deserialize<T>();
        }

        public static implicit operator T (JsonResponse<T> response)
        {
            return response.Body;
        }
    }
}