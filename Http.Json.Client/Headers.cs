using System;
using System.Collections.Generic;
using System.Linq;

namespace Sensemaking.Http.Json.Client;

public static class Headers
{
    private static readonly (string, string) AcceptHeader = ("Accept", MediaType.Json);
    private static (string, string) AuthorizationHeader((string Username, string Password) auth) => ("Authorization", "Basic " + $"{auth.Username}:{auth.Password}".Base64Encode());

    public static string ValueFor(this (string Name, string Value)[] headers, string headerName)
    {
        return headers.SingleOrDefault(h => h.Name == headerName).Value ?? string.Empty;
    }

    public static IEnumerable<(string, string)> AddBasicAuthentication(this IEnumerable<(string, string)> headers, (string Username, string Password) auth)
    {
        return new[] { AuthorizationHeader(auth) }.Concat(headers);
    }

    internal static IEnumerable<(string, string)> AddAcceptHeader(this IEnumerable<(string, string)> headers)
    {
        return new[] { AcceptHeader }.Concat(headers);
    }
}