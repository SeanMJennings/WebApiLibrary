using System.Text.Json.Serialization;
using Common.Environment;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Observability.OpenTelemetry.AspNet;
using OpenTelemetry;

namespace WebHost;

public abstract class WebApi
{
    private readonly WebApplicationBuilder builder;
    private WebApplication app = null!;
    private readonly IConfiguration configuration;
    protected abstract string ApplicationName { get; }
    protected abstract string TelemetryConnectionString { get; }
    protected virtual List<JsonConverter> JsonConverters { get; } = [];
    
    protected WebApi(WebApplicationBuilder webApplicationBuilder, IConfiguration configuration)
    {
        this.configuration = configuration;
        builder = webApplicationBuilder;
        Setup();
    }
    
    public Task RunAsync()
    {
        return app.RunAsync();
    }

    protected virtual void ConfigureServices(IServiceCollection services){}
    
    protected virtual OpenTelemetryBuilder ConfigureTelemetry(WebApplicationBuilder builder)
    {
        return builder.ConfigureOpenTelemetry(ApplicationName);
    }
    
    protected virtual void ConfigureApplication(WebApplication theApp){}
    
    private void Setup()
    {
        builder.Configuration.AddConfiguration(configuration);
        builder.Services.EnforceDefaultSerialisation(JsonConverters);
        builder.Services.EnforceDefaultExceptionHandling();
        builder.Services.AddEndpointsApiExplorer();
        
        if (IsNotProduction())
        {
            builder.Services.AddSwaggerGen();
        }
        
        ConfigureServices(builder.Services);
        
        if (IsNotLocalTestingOrBuildPipeline() && !string.IsNullOrWhiteSpace(TelemetryConnectionString))
        {
            ConfigureTelemetry(builder);
        }
        
        app = builder.Build();
        
        if (IsNotLocalTestingOrBuildPipeline() && !string.IsNullOrWhiteSpace(TelemetryConnectionString))
        {
            app.ConfigureGlobalLogger();
        }
        
        app.MapExceptionsToApiError();
        
        if (IsNotProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseAuthentication();
        app.UseAuthorization();
        ConfigureApplication(app);
        app.MapControllers();
    }

    protected bool IsProduction()
    {
        return CommonEnvironmentExtensions.GetEnvironment().IsAProductionEnvironment();
    }

    protected bool IsNotProduction()
    {
        return !IsProduction();
    }
    
    protected bool IsNotLocalTestingOrBuildPipeline()
    {
        var environment = CommonEnvironmentExtensions.GetEnvironment();
        return environment != CommonEnvironment.LocalTesting && environment != CommonEnvironment.BuildPipeline;
    }
}