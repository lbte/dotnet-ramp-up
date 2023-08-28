using Microsoft.AspNetCore.Mvc;

namespace PRFTLatam.Workshop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [Route("GetHealthCheck")]
    public IActionResult HealthCheck()
    {
        return Ok();
    }
}
