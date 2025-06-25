using WebHost;

namespace Testing.Host.Apis;

public static class TestApiFactory
{
    public static WebApi Create(TestApiTypes type, WebApplicationBuilder builder, IConfiguration configuration)
    {
        return type switch
        {
            TestApiTypes.Default => new DefaultApi(builder, configuration),
            TestApiTypes.AllowAllCors => new AllowAllCorsApi(builder, configuration),
            TestApiTypes.AllowNoCors => new AllowNoCorsApi(builder, configuration),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}