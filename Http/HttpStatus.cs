using System.Net;

namespace Sensemaking.Http
{
    public readonly struct HttpStatus
    {
        public HttpStatus(HttpStatusCode code, string reason)
        {
            Code = code;
            Reason = reason;
        }

        public HttpStatusCode Code { get; }
        public string Reason { get; }
    }
}