using System;
using System.Linq;
using Sensemaking.Bdd;
using Sensemaking.Http.Json.Client;

namespace Sensemaking.Specs;

public partial class HeaderSpecs 
{
    private const string AuthorisationHeaderName = "Authorization";
    private const string username = "Captain Bob";
    private const string password = "Sj75^Wj3!3";

    private (string Name, string Value)[] headers;
    private string value;

    protected override void before_each()
    {
        base.before_each();
        headers = Array.Empty<(string, string)>();
        value = default;
    }

    private void a_header(string name, string value)
    {
        headers = new[] {(name, value)};
    }

    private void a_header()
    {
        headers = new[] {("SomeHeader", "SomeValue")};
    }

    private void getting_value_for(string name)
    {
        value = headers.ValueFor(name);
    }

    private void it_is(string value)
    {
        this.value.should_be(value);
    }

    private void adding_basic_authentication()
    {
        headers = headers.AddBasicAuthentication((username, password)).ToArray();
    }

    private void it_has_an_authorization_header()
    {
        headers.Count(h => h.Name == AuthorisationHeaderName).should_be(1);
    }

    private void it_is_base_64_encoded()
    {
        headers.ValueFor(AuthorisationHeaderName).should_be($"Basic {($"{username}:{password}").Base64Encode()}");
    }
}