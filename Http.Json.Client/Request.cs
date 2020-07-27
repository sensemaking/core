using System;
using System.Collections.Generic;
using System.Linq;
using System.Serialization;
using Flurl.Http;
using Flurl.Http.Content;

namespace Sensemaking.Http.Json.Client
{
    public static class Request
    {
        private static readonly (string, string) AcceptHeader = ("Accept", MediaType.Json); 

        internal static CapturedJsonContent ToRequestBody(this object payload)
        {
            return new CapturedJsonContent(payload.Serialize());
        }

        internal static IFlurlRequest WithHeaders(this IFlurlRequest request, params (string Name, string Value)[] headers)
        {
            headers.ForEach(header => request.WithHeader(header.Name, header.Value));
            return request;
        }
        
        internal static IFlurlRequest WithHeaders(this string url, params (string Name, string Value)[] headers)
        {
            var request = new FlurlRequest(url);
            headers.ForEach(header => request.WithHeader(header.Name, header.Value));
            return request;
        }

        internal static (string, string)[] AddAcceptHeader(this IEnumerable<(string, string)> headers)
        {
            return new [] { AcceptHeader }.Concat(headers).ToArray();
        }
    }
}