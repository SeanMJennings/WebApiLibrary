using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebHost;

public static class ServiceCorsExtensions
{
    private const string CorsAllowAllPolicyName = "AllowAll";
    public static void AddCorsAllowAll(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsAllowAllPolicyName, policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });
    }

    public static void UseCorsAllowAll(this WebApplication app)
    {
        app.UseCors(CorsAllowAllPolicyName);
    }
}