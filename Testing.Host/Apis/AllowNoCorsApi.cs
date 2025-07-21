using WebHost;

namespace Testing.Host.Apis;

public class AllowNoCorsApi(WebApplicationBuilder builder, IConfiguration configuration) : WebApi(builder, configuration)
{
    private readonly IConfiguration _configuration = configuration;
    protected override string ApplicationName => "AllowANoCorsApi";
    protected override string TelemetryConnectionString => _configuration["ApplicationInsights:ConnectionString"] ?? string.Empty;
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IAmAService, BoopService>();
    }
    
    protected override void ConfigureApplication(WebApplication theApp)
    {
        theApp.UseCors();
    }
}