using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Serialization;

namespace Sensemaking.Http.Json.Client
{
    public class JsonResponse
    {
        public HttpStatusCode Status { get; }
        public (string Name, string Value)[] Headers { get; }

        internal JsonResponse(HttpStatusCode status, (string, string)[] headers)
        {
            Status = status;
            Headers = headers;
        }

        public static implicit operator HttpStatusCode (JsonResponse response)
        {
            return response.Status;
        }
    }

    public class JsonResponse<T> : JsonResponse
    {
        public (T Body, (string, string)[] Headers) Content { get; }

        internal JsonResponse(HttpStatusCode Status, (string, string)[] Headers, (string Body, (string,string)[] Headers) Content) : base(Status, Headers)
        {
            if (Content.Body.IsNullOrEmpty())
                throw new Exception("The response to a GET request did not include a body.");

            this.Content = (Content.Body.Deserialize<T>(), Content.Headers);
        }

        public static implicit operator T (JsonResponse<T> response)
        {
            return response.Content.Body;
        }
    }
}