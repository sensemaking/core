using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Serialization;

namespace Sensemaking.Http
{
    public class HttpResponse<T>
    {
        [AllowNull]
        public T Body { get; }
        public HttpStatus Status { get; }
        public IDictionary<string, string> Headers { get; }
          
        internal HttpResponse(HttpStatusCode code, string reason, IDictionary<string, string> headers, string body = default!)
        {
            Body = body != null ? body.Deserialize<T>() : default;
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

            private HttpStatusCode Code { get; }
            private string Reason { get; }
        }

        public static implicit operator T (HttpResponse<T> response)
        {
            return response.Body;
        }
    }
}