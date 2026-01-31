using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using WebHost;

namespace Testing.Host;

[ApiController]
[Route("api/test")]
public class Controller(IAmAService service) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        service.Boop();
        return Ok(new DomainModel());
    }
}

[ApiController]
[Route("api/customserialisation")]
public class SerialisationController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new AnotherDomainModel());
    }
}

[ApiController]
[Route("api/exception")]
public class ExceptionController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        throw new ApplicationException("Oh no!");
    }

    [HttpPost]
    public IActionResult Post()
    {
        throw new ValidationException("bong");
    }

    [HttpDelete]
    public IActionResult Delete()
    {
        throw new SerializationException("bing");
    }
}

[ApiController]
[Route("api/customexception")]
public class CustomExceptionController : ControllerBase
{
    [HttpGet("notfound")]
    public IActionResult GetNotFound()
    {
        throw new NotFoundException("Entity", "123");
    }

    [HttpPost("validation")]
    public IActionResult PostValidation()
    {
        throw new ValidationException("validation error");
    }

    [HttpDelete("serialisation")]
    public IActionResult DeleteSerialisation()
    {
        throw new SerializationException("serialisation error");
    }

    [HttpPut("unknown")]
    public IActionResult PutUnknown()
    {
        throw new InvalidOperationException("unknown error");
    }
}