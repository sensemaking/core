using System.Collections.Generic;
using System.Net;
using System.Serialization;

namespace Sensemaking.Http
{
    public class HttpResponse
    {
        public HttpStatus Status { get; }
        public IDictionary<string, string> Headers { get; }
          
        internal HttpResponse(HttpStatusCode code, string reason, IDictionary<string, string> headers)
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

        public static implicit operator HttpStatus (HttpResponse response)
        {
            return response.Status;
        }
    }

    public class HttpResponse<T> : HttpResponse
    {
        public T Body { get; }
          
        internal HttpResponse(string body, HttpStatusCode code, string reason, IDictionary<string, string> headers) : base(code, reason, headers)
        {
            Body = body.Deserialize<T>();
        }

        public static implicit operator T (HttpResponse<T> response)
        {
            return response.Body;
        }
    }
}