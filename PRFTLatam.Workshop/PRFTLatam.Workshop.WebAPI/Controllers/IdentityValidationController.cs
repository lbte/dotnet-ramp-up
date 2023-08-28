using Microsoft.AspNetCore.Mvc;
using PRFTLatam.Workshop.Services;


namespace PRFTLatam.Workshop.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityValidationController : ControllerBase
{
    // dependency injection
    private readonly IIdentityService _identityService;

    public IdentityValidationController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpGet]
    [Route("GetIds")]
    public IActionResult GetIds()
    {
        var ids = _identityService.GetIds();
        return ids.Any() ? Ok(ids) : StatusCode(StatusCodes.Status422UnprocessableEntity);
    }

    [HttpGet]
    [Route("GetAllIdsValidation")]
    public IActionResult GetAllIdsValidation()
    {
        var errors = _identityService.IdentityValidation(_identityService.GetIds());
        return errors.Any() ? StatusCode(StatusCodes.Status422UnprocessableEntity, errors) : Ok();
    }

    [HttpGet]
    [Route("GetSomeIdsValidation")]
    public IActionResult GetSomeIdsValidation(int amount)
    {
        var errors = _identityService.AmountOfIdsIdentityValidation(amount);
        return errors.Any() ? StatusCode(StatusCodes.Status422UnprocessableEntity, errors) : Ok();
    }

}