using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace System.Serialization;

public class ContractResolver : CamelCasePropertyNamesContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var jProperty = base.CreateProperty(member, memberSerialization);
        if(jProperty.Writable)
            return jProperty;

        jProperty.Writable = member is PropertyInfo && ((PropertyInfo) member).GetSetMethod(true) != null;
        return jProperty;
    }

    protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
    {
        var provider = base.CreateMemberValueProvider(member);
        var propertyType = (member as PropertyInfo)?.PropertyType;

        if(propertyType == null)
            return provider;

        if(propertyType.IsArray)
            return new NullOrMissingToEmptyArrayProvider(provider, propertyType.GetElementType()!);

        if(propertyType!.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(ImmutableArray<>))
            return new MissingToEmptyImmutableArrayProvider(provider, propertyType.GenericTypeArguments.Single());

        if(propertyType!.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(ImmutableList<>))
            return new NullToEmptyImmutableListProvider(provider, propertyType.GenericTypeArguments.Single());

        return provider;
    }
}