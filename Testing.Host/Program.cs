using Testing.Host.Apis;

namespace Testing.Host;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();
        var builder = WebApplication.CreateBuilder(args);
        var apiType = Enum.Parse<TestApiTypes>(Environment.GetEnvironmentVariable("ApiType")!, true);
        var testApi = TestApiFactory.Create(apiType, builder, configuration);
        await testApi.RunAsync();
    }
}