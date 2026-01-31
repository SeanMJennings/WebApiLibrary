using System.Net;
using WebHost;

namespace Testing.Host.Apis;

public class TestExceptionHandler : ExceptionHandler
{
    public override (HttpStatusCode, ApiError) HandleException(Exception exception)
    {
        return exception switch
        {
            NotFoundException ex => Handle(ex),
            _ => base.HandleException(exception)
        };
    }

    protected virtual (HttpStatusCode, ApiError) Handle(NotFoundException ex)
    {
        return (HttpStatusCode.NotFound, new ApiError("The requested resource was not found.", ex.Message));
    }
}
