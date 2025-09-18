using System;
using NUnit.Framework;
using Sensemaking.Bdd;

namespace Fdb.Rx.Testing.Core.LanguageExtensions;

public partial class EnumerableEquivalenceSpecs : Specification
{
    [Test]
    public void enumerables_are_equivalent_when_null_empty_or_they_have_the_same_contents()
    {
        scenario(() =>
        {
            Given(() => an_array(null));
            And(() => another(null));
            Then(() => has_same_contents(true));
        });

        scenario(() =>
        {
            Given(() => an_array(Array.Empty<object>()));
            And(() => another(Array.Empty<object>()));
            Then(() => has_same_contents(true));
        });

        scenario(() =>
        {
            Given(() => an_array(null));
            And(() => another(Array.Empty<object>()));
            Then(() => has_same_contents(true));
        });

        scenario(() =>
        {
            Given(() => an_array(Array.Empty<object>()));
            And(() => another(null));
            Then(() => has_same_contents(true));
        });

        scenario(() =>
        {
            Given(() => an_array(new[] { new { Name = "Wibble" } }));
            And(() => another(new[] { new { Name = "Wibble" } }));
            Then(() => has_same_contents(true));
        });

        scenario(() =>
        {
            Given(() => an_array(null));
            And(() => another(new object[] { new { Name = "Wibble" } }));
            Then(() => has_same_contents(false));
        });

        scenario(() =>
        {
            Given(() => an_array(Array.Empty<object>()));
            And(() => another(new object[] { new { Name = "Wibble" } }));
            Then(() => has_same_contents(false));
        });

        scenario(() =>
        {
            Given(() => an_array(new object[] { new { Name = "Wibble" } }));
            And(() => another(new object[] { new { Name = "Wobble" } }));
            Then(() => has_same_contents(false));
        });

        scenario(() =>
        {
            Given(() => an_array(new[] { new { Name = "Wibble" } }));
            And(() => another(new[] { "Wobble" }));
            Then(() => has_same_contents(false));
        });

        scenario(() =>
        {
            Given(() => an_array(new[] { new { Name = "Wibble1" }, new { Name = "Wibble2" } }));
            And(() => another(new[] { new { Name = "Wibble1" }, new { Name = "Wibble2" } }));
            Then(() => has_same_contents(true));
            And(() => has_same_contents_in_the_same_order(true));
        });

        scenario(() =>
        {
            Given(() => an_array(new[] { new { Name = "Wibble1" }, new { Name = "Wibble2" } }));
            And(() => another(new[] { new { Name = "Wibble2" }, new { Name = "Wibble1" } }));
            Then(() => has_same_contents(true));
            And(() => has_same_contents_in_the_same_order(false));
        });

        scenario(() => 
        {
            Given(() => an_array([1, 2, 2]));
            And(() => another([2, 1, 1]));
            Then(() => has_same_contents(false));
            And(() => has_same_contents_in_the_same_order(false));
        });
    }
}