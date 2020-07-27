using System.Linq;
using System.Net;
using System.Net.Http;
using System.Serialization;
using System.Threading.Tasks;

namespace Sensemaking.Http.Json.Client
{
    internal static class Response
    {
        internal static async Task<JsonResponse<T>> ToJsonResponse<T>(this HttpResponseMessage response)
        {
            var (status, headers, content) = await response.ParseContent();
            return new JsonResponse<T>(status,headers, content);
        }

        internal static async Task<JsonResponse> ToJsonResponse(this HttpResponseMessage response)
        {
            var (status, headers, _) = await response.ParseContent();
            return new JsonResponse(status,headers);
        }

        private static async Task<(HttpStatusCode, (string, string)[], (string, (string,string)[]))> ParseContent(this HttpResponseMessage response)
        {
            var headers = response.Headers.Select(header => (header.Key, string.Join(",", header.Value))).ToArray();
            var contentHeaders = response.Content.Headers.Select(header => (header.Key, string.Join(",", header.Value))).ToArray();

            var body = await response.Content.ReadAsStringAsync();

            if (response.StatusCode.IsError())
                throw new ProblemException(response.StatusCode, headers, response.IsProblem() ? body.Deserialize<Problem>() : Problem.Empty);

            return (response.StatusCode, headers, (body, contentHeaders));
        }

    }
}