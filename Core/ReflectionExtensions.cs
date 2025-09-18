using System;
using System.Reflection;

namespace System;

public static class ReflectionExtensions
{
    public static T GetReflectedValue<T>(this object source, string memberName)
    {
        var member = CheckMember(source, memberName);
        return (T) member.GetValue(source);
    }

    public static void SetReflectedValue(this object source, string memberName, object value) 
    {
        if (source is ValueType) throw new ArgumentException("Use generic version when modifying value types");
        var member = CheckMember(source, memberName, true);
        member.SetValue(source, value);
    }

    public static T SetReflectedValue<T>(this ValueType source, string memberName, object value) where T : struct
    {
        var member = CheckMember(source, memberName, true);
        member.SetValue(source, value);
        return (T)source;
    }

    private static dynamic CheckMember(object source, string memberName, bool findSetter = false)
    {
        var member = source.GetType().GetInfo(memberName, findSetter);
        if (member == null) throw new ArgumentException($"{memberName} is neither a property or a field of {source.GetType()}");
        return member;
    }

    private static dynamic? GetInfo(this Type type, string propertyName, bool findSetter)
    {
        return type.GetFieldInfo(propertyName) ?? type.GetPropertyInfo(propertyName, findSetter);
    }

    private static MemberInfo? GetPropertyInfo(this Type type, string propertyName, bool findSetter)
    {
        MemberInfo? info = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField);
            
        var lookingForSetter = findSetter && info.HasNoSetter();
        if ((info == null || lookingForSetter) && type?.BaseType != null)
            info = type.BaseType.GetPropertyInfo(propertyName, findSetter);

        return info;
    }

    private static MemberInfo? GetFieldInfo(this Type type, string propertyName)
    {
        MemberInfo? info = type.GetField(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

        if (info == null  && type?.BaseType != null)
            info = type.BaseType.GetFieldInfo(propertyName);

        return info;
    }

    private static bool HasNoSetter(this MemberInfo? info)
    {
        return info != null ? GetReflectedValue<object>(info, "SetMethod") == null : false;
    }
}