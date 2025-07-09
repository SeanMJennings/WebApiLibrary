using System.Text.Json.Serialization;
using WebHost;

namespace Testing.Host.Apis;

public class DefaultApi(WebApplicationBuilder builder, IConfiguration configuration) : WebApi(builder, configuration)
{
    private readonly IConfiguration _configuration = configuration;
    protected override string ApplicationName => "DefaultApi";
    protected override string ApplicationInsightsConnectionString => _configuration["ApplicationInsights:ConnectionString"] ?? string.Empty;
    protected override List<JsonConverter> JsonConverters { get; } = [new AnotherDomainModelConverter()];
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IAmAService, BoopService>();
    }
}