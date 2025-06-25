using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace WebHost;

internal static class Serialisation
{
    internal static void EnforceDefaultSerialisation(this IServiceCollection services, List<JsonConverter> jsonConverters)
    {
        var defaultJsonSerialisationSettings = JsonSerialization.GetJsonSerializerSettings();
        
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                foreach (var converter in defaultJsonSerialisationSettings.Converters) options.SerializerSettings.Converters.Add(converter);
                foreach (var converter in jsonConverters) options.SerializerSettings.Converters.Add(converter);
                options.SerializerSettings.Formatting = defaultJsonSerialisationSettings.Formatting;
                options.SerializerSettings.ContractResolver = defaultJsonSerialisationSettings.ContractResolver;
                options.SerializerSettings.NullValueHandling = defaultJsonSerialisationSettings.NullValueHandling;
            });
    }
}