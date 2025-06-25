using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

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