using System.Linq;
using System.Net.Http;
using System.Serialization;

namespace Sensemaking.Http.Json.Client
{
    public partial class JsonResponse
    {
        public HttpStatus Status { get; }
        public (string Name, string Value)[] Headers { get; }
        public string this[string headerName] => Headers.SingleOrDefault(h => h.Name == headerName).Value ?? string.Empty;

        internal JsonResponse(HttpResponseMessage response)
        {
            Status = new HttpStatus(response.StatusCode, response.ReasonPhrase);
            Headers = response.Content.Headers.Select(header => (header.Key, string.Join(",", header.Value))).ToArray();
        }

        public static implicit operator HttpStatus (Json.Client.JsonResponse response)
        {
            return response.Status;
        }
    }

    public class JsonResponse<T> : Json.Client.JsonResponse
    {
        public T Body { get; }
          
        internal JsonResponse(string body, HttpResponseMessage response) : base(response)
        {
            if (response.StatusCode.IsError())
                throw new ProblemException(new HttpStatus(response.StatusCode, response.ReasonPhrase), response.StatusCode.IsProblem() ? body.Deserialize<Problem>() : Problem.Empty);

            Body = body.Deserialize<T>();
        }

        public static implicit operator T (JsonResponse<T> response)
        {
            return response.Body;
        }
    }
}