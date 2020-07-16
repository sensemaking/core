using System;
using System.Reflection;

namespace Sensemaking.Bdd
{
    public static class ReflectionExtensions
    {
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
