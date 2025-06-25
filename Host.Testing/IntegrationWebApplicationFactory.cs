using Common.Environment;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebHostTesting;

public class IntegrationWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ConfigureWebHostTestingServices(builder);
        SetCommonEnvironmentToLocalTestingOrBuildPipeline(builder);
    }

    protected virtual void ConfigureWebHostTestingServices(IWebHostBuilder builder){}
    
    private static void SetCommonEnvironmentToLocalTestingOrBuildPipeline(IWebHostBuilder builder)
    {
        var environment = CommonEnvironmentExtensions.GetEnvironment();
        if (environment == CommonEnvironment.BuildPipeline)
        {
            builder.UseEnvironment(nameof(CommonEnvironment.BuildPipeline));
            return;
        }
        builder.UseEnvironment(nameof(CommonEnvironment.LocalTesting));
    }
}