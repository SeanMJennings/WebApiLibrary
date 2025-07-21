using WebHost;

namespace Testing.Host.Apis;

public class AllowAllCorsApi(WebApplicationBuilder builder, IConfiguration configuration) : WebApi(builder, configuration)
{
    private readonly IConfiguration _configuration = configuration;
    protected override string ApplicationName => "AllowAllCorsApi";
    protected override string TelemetryConnectionString => _configuration["ApplicationInsights:ConnectionString"] ?? string.Empty;

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IAmAService, BoopService>();
        services.AddCorsAllowAll();
    }
    
    protected override void ConfigureApplication(WebApplication theApp)
    {
        theApp.UseCorsAllowAll();
    }
}