using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Serialization;

namespace Sensemaking.Http.Json.Client
{
    public class JsonResponse
    {
        public HttpStatusCode Status { get; }
        public (string Name, string Value)[] Headers { get; }

        public string BodyAsString { get; }
        
        public Cookie[] Cookies { get; }

        internal JsonResponse(HttpStatusCode status, IEnumerable<(string, string)> headers, string body, Cookie[] cookies)
        {
            Status = status;
            Headers = headers.ToArray();
            BodyAsString = body;
            Cookies = cookies;
        }

        public static implicit operator HttpStatusCode (JsonResponse response)
        {
            return response.Status;
        }
    }

    public class JsonResponse<T> : JsonResponse
    {
        public T Body { get; }

        internal JsonResponse(HttpStatusCode status, IEnumerable<(string, string)> headers, string body, Cookie[] cookies) : base(status, headers, body, cookies)
        {
            if (body.IsNullOrEmpty())
                throw new Exception("The response to a GET request did not include a body.");

            this.Body = body.Deserialize<T>();
        }

        public static implicit operator T (JsonResponse<T> response)
        {
            return response.Body;
        }
    }
}