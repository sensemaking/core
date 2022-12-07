using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace System.Serialization
{
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

            return provider;
        }

        private class NullOrMissingToEmptyArrayProvider : IValueProvider
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
    }

    public class MissingToEmptyImmutableArrayProvider : IValueProvider
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
            if(value != null)
                defaultProvider.SetValue(property, !value.IsUninitialised() ? value : emptyArray);
            else
                defaultProvider.SetValue(property, emptyArray);
        }

        public object? GetValue(object property)
        {
            if(defaultProvider.GetValue(property) != null)
               return !defaultProvider.GetValue(property)!.IsUninitialised() ? defaultProvider.GetValue(property) : emptyArray;
            else
                return emptyArray;
        }
    }

    internal static class TypeExtensions
    {
        internal static object CreateAsImmutableArray(this Type elementType)
        {
            return typeof(ImmutableArray).GetMethods(BindingFlags.Static | BindingFlags.Public).Single(m => m.Name == "Create" && m.GetParameters().Length == 0 && m.IsGenericMethod).MakeGenericMethod(elementType).Invoke(null, null)!;
        }

        internal static bool IsUninitialised(this object immutableArray)
        {
            return (bool) immutableArray!.GetType().GetProperty("IsDefault", BindingFlags.Instance | BindingFlags.Public)!.GetValue(immutableArray)!;
        }
    }
}