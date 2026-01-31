using System.Net;
using BDD;
using Shouldly;
using Testing.Host.Apis;
using Testing.Utilities;
using WebHost;
using WebHostTesting;

namespace Testing;

public partial class ExceptionHandlerShould : WebApiSpecification
{
    protected override void before_each()
    {
        base.before_each();
        Environment.SetEnvironmentVariable("ApiType", nameof(TestApiTypes.CustomExceptionHandler));
        factory?.Dispose();
        client?.Dispose();
        factory = new IntegrationWebApplicationFactory<Host.Program>();
        client = factory.CreateClient();
        client.BaseAddress = new Uri(client.BaseAddress!.ToString().Replace("http", "https"));
    }

    private void an_api_with_custom_exception_handler() {}

    private void calling_the_api_that_throws_not_found_exception()
    {
        var response = client!.GetAsync("/api/customexception/notfound").Await();
        responseCode = response.StatusCode;
        responseContent = response.Content;
    }

    private void calling_the_api_that_throws_validation_exception()
    {
        var response = client!.PostAsync("/api/customexception/validation", null).Await();
        responseCode = response.StatusCode;
        responseContent = response.Content;
    }

    private void calling_the_api_that_throws_serialisation_exception()
    {
        var response = client!.DeleteAsync("/api/customexception/serialisation").Await();
        responseCode = response.StatusCode;
        responseContent = response.Content;
    }

    private void calling_the_api_that_throws_unknown_exception()
    {
        var response = client!.PutAsync("/api/customexception/unknown", null).Await();
        responseCode = response.StatusCode;
        responseContent = response.Content;
    }

    private void not_found_response_is_returned()
    {
        responseCode.ShouldBe(HttpStatusCode.NotFound);
        var content = JsonSerialization.Deserialize<ApiError>(responseContent.ReadAsStringAsync().Await());
        content.ShouldNotBeNull();
        content.Message.ShouldBe("The requested resource was not found.");
        content.Errors[0].ShouldBe("Entity with id '123' was not found.");
    }

    private void bad_request_response_is_returned_for_validation()
    {
        responseCode.ShouldBe(HttpStatusCode.BadRequest);
        var content = JsonSerialization.Deserialize<ApiError>(responseContent.ReadAsStringAsync().Await());
        content.ShouldNotBeNull();
        content.Message.ShouldBe("The request could not be correctly validated.");
    }

    private void bad_request_response_is_returned_for_serialisation()
    {
        responseCode.ShouldBe(HttpStatusCode.BadRequest);
        var content = JsonSerialization.Deserialize<ApiError>(responseContent.ReadAsStringAsync().Await());
        content.ShouldNotBeNull();
        content.Message.ShouldBe("The request could not be correctly serialized.");
    }

    private void internal_server_error_response_is_returned()
    {
        responseCode.ShouldBe(HttpStatusCode.InternalServerError);
        var content = JsonSerialization.Deserialize<ApiError>(responseContent.ReadAsStringAsync().Await());
        content.ShouldNotBeNull();
        content.Message.ShouldBe("An unexpected error occurred.");
    }
}
