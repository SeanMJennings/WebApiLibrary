using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WebHost;

internal static class ExceptionHandling
{
    internal static void EnforceDefaultExceptionHandling(this IServiceCollection services)
    {
        services.Replace(ServiceDescriptor.Singleton(new ExceptionHandler()));
    }
    
    internal static void MapExceptionsToApiError(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            AllowStatusCode404Response = true,
            ExceptionHandler = context =>
            {
                var feature = context.Features.Get<IExceptionHandlerFeature>()!;
                var exceptionHandler = app.ApplicationServices.GetRequiredService<ExceptionHandler>();
                var (statusCode, apiError) = exceptionHandler.HandleException(feature.Error);
                context.Response.StatusCode = (int)statusCode;

                if (statusCode == HttpStatusCode.InternalServerError) Logger.LogErrorThatWillTriggerAnAlert("UnexpectedException", feature.Error);

                return context.Response.WriteAsync(JsonSerialization.Serialize(apiError));
            }
        });
    }
}

public class ExceptionHandler
{
    public (HttpStatusCode, ApiError) HandleException(Exception exception)
    {
        return exception switch
        {
            ValidationException ex => Handle(ex),
            SerializationException ex => Handle(ex),
            _ => Handle(exception),
        };
    }

    protected virtual (HttpStatusCode, ApiError) Handle(ValidationException ex)
    {
        if (ex.ValidationResult.ErrorMessage is null)
        {
            return (HttpStatusCode.BadRequest, new ApiError("The request could not be correctly validated."));
        }
        return (HttpStatusCode.BadRequest, new ApiError("The request could not be correctly validated.", ex.ValidationResult.ErrorMessage));
    }

    protected virtual (HttpStatusCode, ApiError) Handle(SerializationException ex)
    {
        return (HttpStatusCode.BadRequest, new ApiError("The request could not be correctly serialized.", ex.Message));
    }

    protected virtual (HttpStatusCode, ApiError) Handle(Exception _)
    {
        return (HttpStatusCode.InternalServerError, ApiError.Empty);
    }
}