using System.Collections.Generic;
using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace System.Serialization;

public sealed class JsonSerializer : Newtonsoft.Json.JsonSerializer
{
    public JsonSerializer(params JsonConverter[] converters)
    {
        var settings = Serialization.GetSettings(converters);
        CheckAdditionalContent = settings.CheckAdditionalContent;
        ConstructorHandling = settings.ConstructorHandling;
        ContractResolver = settings.ContractResolver!;
        Culture = settings.Culture;
        DateFormatHandling = settings.DateFormatHandling;
        DateFormatString = settings.DateFormatString;
        DateParseHandling = settings.DateParseHandling;
        DateTimeZoneHandling = settings.DateTimeZoneHandling;
        DefaultValueHandling = settings.DefaultValueHandling;
        EqualityComparer = settings.EqualityComparer;
        FloatFormatHandling = settings.FloatFormatHandling;
        FloatParseHandling = settings.FloatParseHandling;
        Formatting = settings.Formatting;
        MaxDepth = settings.MaxDepth;
        MetadataPropertyHandling = settings.MetadataPropertyHandling;
        MissingMemberHandling = settings.MissingMemberHandling;
        NullValueHandling = settings.NullValueHandling;
        ObjectCreationHandling = settings.ObjectCreationHandling;
        PreserveReferencesHandling = settings.PreserveReferencesHandling;
        ReferenceLoopHandling = settings.ReferenceLoopHandling;
        StringEscapeHandling = settings.StringEscapeHandling;
        TypeNameAssemblyFormatHandling = settings.TypeNameAssemblyFormatHandling;
        TypeNameHandling = settings.TypeNameHandling;
        settings.Converters.ForEach(Converters.Add);
    }
}

public static class Serialization
{
    private static readonly List<JsonConverter> additionalConverters = new List<JsonConverter>();

    public static void Configure(params JsonConverter[] converters)
    {
        converters.ForEach(c =>
        {
            if(additionalConverters.None(ac => ac.GetType().AssemblyQualifiedName == c.GetType().AssemblyQualifiedName))
                additionalConverters.Add(c);
        });
    }

    public static T Deserialize<T>(this string json, params JsonConverter[] converters)
    {
        return JsonConvert.DeserializeObject<T>(json, GetSettings(converters))!;
    }

    public static object Deserialize(this string json, Type type, params JsonConverter[] converters)
    {
        return JsonConvert.DeserializeObject(json, type, GetSettings(converters))!;
    }

    public static string Serialize(this object entity, params JsonConverter[] converters)
    {
        return JsonConvert.SerializeObject(entity, Formatting.None, GetSettings(converters));
    }

    public static JsonSerializerSettings GetSettings(params JsonConverter[] converters)
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new ContractResolver(),
            DefaultValueHandling = DefaultValueHandling.Populate,
            Formatting = Formatting.None,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };
        settings.Converters.Add(new RemoveWhitespaceStringEnumConverter());
        settings.Converters.Add(new MoneyConverter());
        settings.Converters.Add(new NullableMoneyConverter());
        settings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        settings.Converters.CustomiseNodaTimeConvertion();
        additionalConverters.ForEach(settings.Converters.Add);
        converters.ForEach(settings.Converters.Add);
        return settings;
    }

    internal static void CustomiseNodaTimeConvertion(this IList<JsonConverter> converters)
    {
        converters.Remove(NodaConverters.LocalDateConverter);
        converters.Add(new DateAndDateTimeToLocalDateConverter());
        converters.Add(new DateAndDateTimeToNullableLocalDateConverter());
    }
}
