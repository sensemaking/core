using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Serialization;

namespace Sensemaking.Http.Json.Client
{
    public class JsonResponse
    {
        public HttpStatusCode Status { get; }
        public IReadOnlyList<(string Name, string Value)> Headers { get; }

        internal JsonResponse(HttpStatusCode status, IEnumerable<(string, string)> headers)
        {
            Status = status;
            Headers = new ReadOnlyCollection<(string Name, string Value)>(headers.ToList());
        }

        public static implicit operator HttpStatusCode (JsonResponse response)
        {
            return response.Status;
        }
    }

    public class JsonResponse<T> : JsonResponse
    {
        public (T Body, IReadOnlyList<(string, string)> Headers) Content { get; }

        internal JsonResponse(HttpStatusCode status, IEnumerable<(string, string)> headers, (string Body, IEnumerable<(string,string)> Headers) content) : base(status, headers)
        {
            if (content.Body.IsNullOrEmpty())
                throw new Exception("The response to a GET request did not include a body.");

            this.Content = (content.Body.Deserialize<T>(), new ReadOnlyCollection<(string, string)>( content.Headers.ToList() ));
        }

        public static implicit operator T (JsonResponse<T> response)
        {
            return response.Content.Body;
        }
    }
}