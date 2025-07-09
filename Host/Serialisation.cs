using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace WebHost;

internal static class Serialisation
{
    internal static void EnforceDefaultSerialisation(this IServiceCollection services, List<JsonConverter> jsonConverters)
    {
        var defaultJsonSerialisationSettings = JsonSerialization.GetJsonSerializerOptions();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                foreach (var converter in defaultJsonSerialisationSettings.Converters) options.JsonSerializerOptions.Converters.Add(converter);
                foreach (var converter in jsonConverters) options.JsonSerializerOptions.Converters.Add(converter);
                options.JsonSerializerOptions.PropertyNamingPolicy = defaultJsonSerialisationSettings.PropertyNamingPolicy;
                options.JsonSerializerOptions.DefaultIgnoreCondition = defaultJsonSerialisationSettings.DefaultIgnoreCondition;
            });
    }
}