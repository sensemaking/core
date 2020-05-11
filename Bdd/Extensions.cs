using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Sensemaking.Bdd
{
    public static class Extensions
    {
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var property in value.GetType().GetProperties())
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }

        public static T GetReflectedValue<T>(this object source, string propertyName) 
        {
            return (T) source.GetType().GetInfo(propertyName)?.GetValue(source, null);  
        }

        public static void SetReflectedValue(this object source, string propertyName, object value)
        {
            source.GetType().GetInfo(propertyName)?.SetValue(source, value);
        }

        private static PropertyInfo GetInfo(this Type type, string propertyName)
        {
            var info = type?.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            while (info == null && type != null)
                info = type.BaseType.GetInfo(propertyName);

            return info;
        }
    }
}
