using System.Linq;

namespace Sensemaking.Http.Json.Client
{
    public static class Headers
    {
        public static string ValueFor(this (string Name, string Value)[] headers, string headerName)
        {
            return headers.SingleOrDefault(h => h.Name == headerName).Value ?? string.Empty;
        }
    }
}