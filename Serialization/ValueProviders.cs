using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace System.Serialization;

internal class NullOrMissingToEmptyArrayProvider : IValueProvider
{
    private readonly IValueProvider defaultProvider;
    private readonly Array emptyArray;

    public NullOrMissingToEmptyArrayProvider(IValueProvider defaultProvider, Type elementType)
    {
        this.defaultProvider = defaultProvider;
        emptyArray = Array.CreateInstance(elementType, 0);
    }

    public void SetValue(object property, object? value)
    {
        defaultProvider.SetValue(property, value ?? emptyArray);
    }

    public object GetValue(object property)
    {
        return defaultProvider.GetValue(property) ?? emptyArray;
    }
}

internal class MissingToEmptyImmutableArrayProvider : IValueProvider
{
    private readonly IValueProvider defaultProvider;
    private readonly object emptyArray;

    public MissingToEmptyImmutableArrayProvider(IValueProvider provider, Type elementType)
    {
        this.defaultProvider = provider;
        emptyArray = elementType.CreateAsImmutableArray();
    }

    public void SetValue(object property, object? value)
    {
        defaultProvider.SetValue(property, !value!.IsUninitialised() ? value : emptyArray);
    }

    public object? GetValue(object property)
    {
        return !defaultProvider.GetValue(property)!.IsUninitialised() ? defaultProvider.GetValue(property) : emptyArray;
    }
}

internal class NullToEmptyImmutableListProvider : IValueProvider
{
    private readonly IValueProvider defaultProvider;
    private readonly object emptyList;

    public NullToEmptyImmutableListProvider(IValueProvider provider, Type elementType)
    {
        this.defaultProvider = provider;
        emptyList = elementType.CreateAsImmutable(typeof(ImmutableList));
    }

    public void SetValue(object property, object? value)
    {
        defaultProvider.SetValue(property, value ?? emptyList);
    }

    public object? GetValue(object property)
    {
        return defaultProvider.GetValue(property) ?? emptyList;
    }
}

internal static class TypeExtensions
{
    internal static object CreateAsImmutableArray(this Type elementType)
    {
        return typeof(ImmutableArray).GetMethods(BindingFlags.Static | BindingFlags.Public).Single(m => m.Name == "Create" && m.GetParameters().Length == 0 && m.IsGenericMethod).MakeGenericMethod(elementType).Invoke(null, null)!;
    }

    internal static object CreateAsImmutable(this Type elementType, Type targetType)
    {
        return targetType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(m => m.Name == "Create" && m.GetParameters().Length == 0 && m.IsGenericMethod).MakeGenericMethod(elementType).Invoke(null, null)!;
    }


    internal static bool IsUninitialised(this object immutableArray)
    {
        return (bool) immutableArray!.GetType().GetProperty("IsDefault", BindingFlags.Instance | BindingFlags.Public)!.GetValue(immutableArray)!;
    }
}