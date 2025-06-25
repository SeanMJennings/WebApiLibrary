using System.Net;
using System.Net.Http.Headers;
using Alerting;
using BDD;
using Logging.TestingUtilities;
using Microsoft.Extensions.DependencyInjection;
using Testing.Host.Apis;
using WebHostTesting;
using Program = Testing.Host.Program;

namespace Testing.Utilities;

public abstract class WebApiSpecification : Specification
{
    protected IntegrationWebApplicationFactory<Program>? factory;
    protected IServiceProvider services;
    protected HttpClient? client;
    protected HttpStatusCode responseCode;
    protected HttpContent reponseContent = null!;
    protected HttpHeaders responseHeaders = null!;
    protected string capturedMessage = null!;
    protected Exception capturedException = null!;
    protected AlertCode capturedAlertCode;
    
    protected override void before_each()
    {
        Environment.SetEnvironmentVariable("ApiType", TestApiTypes.Default.ToString());
        base.before_each();
        factory?.Dispose();
        client?.Dispose();
        factory = new IntegrationWebApplicationFactory<Program>();
        services = factory.Services;
        client = factory.CreateClient();
        responseCode = default;
        client.DefaultRequestHeaders.Authorization = null;
        client.BaseAddress = new Uri(client.BaseAddress!.ToString().Replace("http", "https"));
        reponseContent = null!;
        responseHeaders = null!;
        capturedMessage = null!;
        capturedException = null!;
        capturedAlertCode = default;
        var logErrorAction = new Action<string, Exception, AlertCode, object?>((message, theException, alertCode, _) =>
        {
            capturedMessage = message;
            capturedException = theException;
            capturedAlertCode = alertCode;
        });
        LoggingMocker.SetupLoggingErrorMock(logErrorAction);
    }
    
    protected override void after_each()
    {
        base.after_each();
        Environment.SetEnvironmentVariable("ApiType", string.Empty);
        factory?.Dispose();
        client?.Dispose();
        services = null!;
    }
    
    protected override void after_all()
    {
        base.after_all();
        factory?.Dispose();
        client?.Dispose();
        services = null!;
    }
    
    protected virtual void an_api(){}
}