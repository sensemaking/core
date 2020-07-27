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
        public (string Name, string Value)[] ContentHeaders { get; }
        public T Body { get; }

        internal JsonResponse(HttpStatusCode status, (string, string)[] headers, string body, (string,string)[] contentHeaders) : base(status, headers)
        {
            if (body.IsNullOrEmpty())
                throw new Exception("The response to a GET request did not include a body.");

            ContentHeaders = contentHeaders;
            Body = body.Deserialize<T>();
        }

        public static implicit operator T (JsonResponse<T> response)
        {
            return response.Body;
        }
    }
}