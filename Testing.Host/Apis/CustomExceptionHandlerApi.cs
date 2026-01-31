using WebHost;

namespace Testing.Host.Apis;

public class CustomExceptionHandlerApi(WebApplicationBuilder builder, IConfiguration configuration) : WebApi(builder, configuration)
{
    private readonly IConfiguration _configuration = configuration;
    protected override string ApplicationName => "CustomExceptionHandlerApi";
    protected override string TelemetryConnectionString => _configuration["ApplicationInsights:ConnectionString"] ?? string.Empty;

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.UseExceptionHandler<TestExceptionHandler>();
        services.AddSingleton<IAmAService, BoopService>();
    }
}
