using System.Collections.Generic;
using System.Net;
using System.Serialization;

namespace Sensemaking.Http
{
    public class JsonResponse
    {
        public HttpStatus Status { get; }
        public IDictionary<string, string> Headers { get; }
          
        internal JsonResponse(HttpStatusCode code, string reason, IDictionary<string, string> headers)
        {
            Status = new HttpStatus(code, reason);
            Headers = headers;
        }

        public readonly struct HttpStatus
        {
            internal HttpStatus(HttpStatusCode code, string reason)
            {
                Code = code;
                Reason = reason;
            }

            public HttpStatusCode Code { get; }
            public string Reason { get; }
        }

        public static implicit operator HttpStatus (JsonResponse response)
        {
            return response.Status;
        }
    }

    public class JsonResponse<T> : JsonResponse
    {
        public T Body { get; }
          
        internal JsonResponse(string body, HttpStatusCode code, string reason, IDictionary<string, string> headers) : base(code, reason, headers)
        {
            Body = body.Deserialize<T>();
        }

        public static implicit operator T (JsonResponse<T> response)
        {
            return response.Body;
        }
    }
}