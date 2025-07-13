using System.Net;
using BDD;
using Shouldly;
using Testing.Host;
using Testing.Host.Apis;
using Testing.Utilities;
using WebHostTesting;

namespace Testing;

public partial class WebApiCorsShould : WebApiSpecification
{
    private void an_api_with_restricted_cors_policy()
    {
        factory?.Dispose();
        client?.Dispose();
        Environment.SetEnvironmentVariable("ApiType", TestApiTypes.AllowNoCors.ToString());
        factory = new IntegrationWebApplicationFactory<Program>();
        client = factory.CreateClient();
    }
    
    private void an_api_with_unrestricted_cors_policy()
    {
        factory?.Dispose();
        client?.Dispose();
        Environment.SetEnvironmentVariable("ApiType", TestApiTypes.AllowAllCors.ToString());
        factory = new IntegrationWebApplicationFactory<Program>();
        client = factory.CreateClient();
    }

    private void calling_the_api_from_a_different_origin()
    {
        var request = new HttpRequestMessage(HttpMethod.Options, "/api/test");
        request.Headers.Add("Origin", "https://example.com");
        request.Headers.Add("Access-Control-Request-Method", "GET");

        var response = client!.SendAsync(request).Await();
        responseCode = response.StatusCode;
        responseContent = response.Content;
        responseHeaders = response.Headers;
    }

    private void restrictive_cors_policy_is_applied()
    {
        responseCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
        responseHeaders.ShouldNotBeNull();
        responseHeaders.Contains("Access-Control-Allow-Origin").ShouldBeFalse();
        responseHeaders.Contains("Access-Control-Allow-Methods").ShouldBeFalse();
    }    
    
    private void unrestricted_cors_policy_is_applied()
    {
        responseCode.ShouldBe(HttpStatusCode.NoContent);
        responseHeaders.ShouldNotBeNull();
        responseHeaders.Contains("Access-Control-Allow-Origin").ShouldBeTrue();
        responseHeaders.Contains("Access-Control-Allow-Methods").ShouldBeTrue();
    }
}