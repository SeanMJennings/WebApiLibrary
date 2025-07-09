using System.Net;
using Common.Environment;
using Alerting;
using BDD;
using Serilog;
using Shouldly;
using Testing.Host;
using Testing.Host.Apis;
using Testing.Utilities;
using WebHost;
using WebHostTesting;

namespace Testing;

public partial class WebApiShould : WebApiSpecification
{
    private void an_api_not_in_production()
    {
        factory?.Dispose();
        client?.Dispose();
        Environment.SetEnvironmentVariable("ApiType", TestApiTypes.Default.ToString());
        Environment.SetEnvironmentVariable("ENVIRONMENT", CommonEnvironment.Development.ToString());
        factory = new IntegrationWebApplicationFactory<Program>();
        client = factory.CreateClient();
    }    
    
    private void an_api_in_production()
    {
        factory?.Dispose();
        client?.Dispose();
        Environment.SetEnvironmentVariable("ApiType", TestApiTypes.Default.ToString());
        Environment.SetEnvironmentVariable("ENVIRONMENT", CommonEnvironment.Production.ToString());
        factory = new IntegrationWebApplicationFactory<Program>();
        client = factory.CreateClient();
    }

    private void calling_the_api_test_controller()
    {
        var response = client!.GetAsync("/api/test").Await();
        responseCode = response.StatusCode;
        reponseContent = response.Content;
    }
    
    private void calling_the_api_custom_serialisation_controller()
    {
        var response = client!.GetAsync("/api/customserialisation").Await();
        responseCode = response.StatusCode;
        reponseContent = response.Content;
    }
    
    private void calling_the_api_that_should_error_unexpectedly()
    {
        var response = client!.GetAsync("/api/exception").Await();
        responseCode = response.StatusCode;
        reponseContent = response.Content;
    }
    
    private void calling_the_swagger_endpoint()
    {
        var response = client!.GetAsync("/swagger").Await();
        responseCode = response.StatusCode;
        reponseContent = response.Content;
    }
    
    private void calling_the_api_that_should_error_in_validation()
    {
        var response = client!.PostAsync("/api/exception", null).Await();
        responseCode = response.StatusCode;
        reponseContent = response.Content;
    }    
    
    private void calling_the_api_that_should_error_in_serialisation()
    {
        var response = client!.DeleteAsync("/api/exception").Await();
        responseCode = response.StatusCode;
        reponseContent = response.Content;
    }

    private void the_controller_was_mapped()
    {
        responseCode.ShouldBe(HttpStatusCode.OK);
    }

    private void calling_the_api_test_controller_that_relies_on_a_service()
    {
        calling_the_api_test_controller();
    }

    private void the_service_is_available_for_injection()
    {
        responseCode.ShouldBe(HttpStatusCode.OK);
    }
    
    
    private void default_serialisation_is_evident_in_the_response()
    {
        var content = reponseContent.ReadAsStringAsync().Await();
        content.ShouldNotBeNullOrEmpty();
        content.Contains("EnumProperty", StringComparison.InvariantCulture).ShouldBeTrue();
        content.Contains("NullableNumberProperty", StringComparison.InvariantCulture).ShouldBeFalse();
        content.Contains("Value1", StringComparison.InvariantCultureIgnoreCase).ShouldBeTrue();
    }

    private void custom_serialisation_is_evident_in_the_response()
    {
        var content = reponseContent.ReadAsStringAsync().Await();
        content.ShouldContain("customserialization");
    }

    private void the_error_is_logged()
    {
        capturedMessage.ShouldBe("UnexpectedException");
        capturedAlertCode.ShouldBe(AlertCode.Alert);
        capturedException.ShouldBeOfType<ApplicationException>();
        capturedException.Message.ShouldBe("Oh no!");
    }
    
    private void default_exception_handling_is_evident_in_the_response()
    {
        responseCode.ShouldBe(HttpStatusCode.InternalServerError);
        var content = JsonSerialization.Deserialize<ApiError>(reponseContent.ReadAsStringAsync().Await());
        content.Message.ShouldBe("An unexpected error occurred.");
        content.Errors.ShouldBeEmpty();
    }
    
    private void default_error_handling_for_validation_exceptions_is_evident_in_the_response()
    {
        responseCode.ShouldBe(HttpStatusCode.BadRequest);
        var content = JsonSerialization.Deserialize<ApiError>(reponseContent.ReadAsStringAsync().Await());
        content.ShouldNotBeNull();
        content.Message.ShouldBe("The request could not be correctly validated.");
        content.Errors[0].ShouldBe("bong");
    }    
    
    private void default_error_handling_for_serialisation_exceptions_is_evident_in_the_response()
    {
        responseCode.ShouldBe(HttpStatusCode.BadRequest);
        var content = JsonSerialization.Deserialize<ApiError>(reponseContent.ReadAsStringAsync().Await());
        content.ShouldNotBeNull();
        content.Message.ShouldBe("The request could not be correctly serialized.");
        content.Errors[0].ShouldBe("bing");
    }

    private void swagger_is_available()
    {
        responseCode.ShouldBe(HttpStatusCode.OK);
    }
    
    private void swagger_is_not_available()
    {
        responseCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private void logging_should_be_configured()
    {
        services.GetService(typeof(ILogger)).ShouldBeAssignableTo<Serilog.Core.Logger>();
    }
}