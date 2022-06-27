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
            if (jProperty.Writable)
                return jProperty;

            jProperty.Writable = member is PropertyInfo && ((PropertyInfo)member).GetSetMethod(true) != null;
            return jProperty;
        }

        protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
        {
            var provider = base.CreateMemberValueProvider(member);
            var propertyType = (member as PropertyInfo)?.PropertyType;

            return propertyType != null && propertyType.IsArray && propertyType.GetElementType() != null ? new NullToEmptyArrayProvider(provider, propertyType.GetElementType()!) : provider;
        }

        private class NullToEmptyArrayProvider : IValueProvider
        {
            private readonly IValueProvider defaultArrayValueProvider;
            private readonly Array emptyArray;

            public NullToEmptyArrayProvider(IValueProvider defaultArrayValueProvider, Type arrayType)
            {
                this.defaultArrayValueProvider = defaultArrayValueProvider;
                emptyArray = Array.CreateInstance(arrayType, 0);
            }

            public void SetValue(object property, object? value)
            {
                defaultArrayValueProvider.SetValue(property, value ?? emptyArray);
            }

            public object GetValue(object property)
            {
                return defaultArrayValueProvider.GetValue(property) ?? emptyArray;
            }
        }
    }
}