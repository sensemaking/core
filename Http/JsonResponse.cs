using System.Linq;
using System.Net;
using System.Net.Http;
using System.Serialization;

namespace Sensemaking.Http
{
    public class JsonResponse
    {
        public HttpStatus Status { get; }
        public (string Name, string Value)[] Headers { get; }
          
        internal JsonResponse(HttpResponseMessage response)
        {
            Status = new HttpStatus(response.StatusCode, response.ReasonPhrase);
            Headers = response.Headers.Select(header => (header.Key, string.Join(",", header.Value))).ToArray();
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
          
        internal JsonResponse(string body, HttpResponseMessage response) : base(response)
        {
            Body = body.Deserialize<T>();
        }

        public static implicit operator T (JsonResponse<T> response)
        {
            return response.Body;
        }
    }
}