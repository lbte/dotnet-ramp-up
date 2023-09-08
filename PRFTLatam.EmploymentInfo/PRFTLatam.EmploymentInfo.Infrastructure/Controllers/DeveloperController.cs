using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PRFTLatam.EmploymentInfo.Application.Services;
using PRFTLatam.EmploymentInfo.Domain.Models;

namespace PRFTLatam.EmploymentInfo.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DeveloperController : ControllerBase
{
    private readonly IDeveloperService _developerService;

    public DeveloperController(IDeveloperService developerService)
    {
        _developerService = developerService;
    }

    
    /// <summary>
    /// Displays all developers in the Database
    /// </summary>
    /// <returns>A List with all the developers</returns>
    /// <response code="200">Returns the list of all developers</response>
    /// <response code="404">If there are no developers found</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllDevelopers()
    {
        var developers = await _developerService.GetDevelopersAsync();
        return developers.Any() ? Ok(developers) : StatusCode(StatusCodes.Status404NotFound, "There were no developers found to show");
    }

    /// <summary>
    /// Displays all developers in the Database that have the corresponding first name
    /// </summary>
    /// <param name="firstName">Developer's First name</param>
    /// <returns>A List with all the developers with that first name</returns>
    /// <response code="200">Returns the list of developers with that first name</response>
    /// <response code="404">If there are no developers found with that first name</response>
    [HttpGet]
    [Route("{firstName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDevelopersByFirstName(string firstName)
    {
        var developers = await _developerService.GetDevelopersByFirstName(firstName);
        return developers.Any() ? Ok(developers) : StatusCode(StatusCodes.Status404NotFound, $"There were no developers found to show with the first name: {firstName}");
    }

    /// <summary>
    /// Displays all developers in the Database that have the corresponding last name
    /// </summary>
    /// <param name="lastName">Developer's Last name</param>
    /// <returns>A List with all the developers with that last name</returns>
    /// <response code="200">Returns the list of developers with that last name</response>
    /// <response code="404">If there are no developers found with that last name</response>
    [HttpGet]
    [Route("{lastName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDevelopersByLastName(string lastName)
    {
        var developers = await _developerService.GetDevelopersByLastName(lastName);
        return developers.Any() ? Ok(developers) : StatusCode(StatusCodes.Status404NotFound, $"There were no developers found to show with the last name: {lastName}");
    }
    
    /// <summary>
    /// Displays all developers in the Database that have the corresponding age
    /// </summary>
    /// <param name="age">Developer's age</param>
    /// <returns>A List with all the developers with that age</returns>
    /// <response code="200">Returns the list of developers with that age</response>
    /// <response code="404">If there are no developers found with that age</response>
    [HttpGet]
    [Route("{age}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDevelopersByAge(int age)
    {
        var developers = await _developerService.GetDevelopersByAge(age);
        return developers.Any() ? Ok(developers) : StatusCode(StatusCodes.Status404NotFound, $"There were no developers found to show which are {age} years old");
    }

    /// <summary>
    /// Displays all developers in the Database that have the corresponding number of worked hours
    /// </summary>
    /// <param name="workedHours">Developer's number of worked hours</param>
    /// <returns>A List with all the developers with that number of worked hours</returns>
    /// <response code="200">Returns the list of developers with that number of worked hours</response>
    /// <response code="404">If there are no developers found with that number of worked hours</response>
    [HttpGet]
    [Route("{workedHours}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDevelopersByWorkedHours(int workedHours)
    {
        var developers = await _developerService.GetDevelopersByWorkedHours(workedHours);
        return developers.Any() ? Ok(developers) : StatusCode(StatusCodes.Status404NotFound, $"There were no developers found to show with {workedHours} worked hours");
    }
    
    /// <summary>
    /// Displays the developer in the Database that has the corresponding email address
    /// </summary>
    /// <param name="email">Developer's email address</param>
    /// <returns>A developer with that email address</returns>
    /// <response code="200">Returns the developer with that email address</response>
    /// <response code="404">If there are no developers found with that email address</response>
    [HttpGet]
    [Route("{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDeveloperByEmail(string email)
    {
        var developer = await _developerService.GetDeveloperByEmail(email);
        return developer != null ? Ok(developer) : StatusCode(StatusCodes.Status404NotFound, $"There was not a developer with the email: {email}");
    }

    /// <summary>
    /// Creates a Developer
    /// </summary>
    /// <param name="developer">The Developer Object</param>
    /// <returns>A newly created Developer</returns>
    /// <response code="201">Returns the newly created developer</response>
    /// <response code="400">If the developer is null</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDeveloper([FromBody] Developer developer) 
    {
        var newDeveloper = await _developerService.CreateDeveloper(developer);
        return newDeveloper != null ? CreatedAtAction(nameof(CreateDeveloper), newDeveloper) : StatusCode(StatusCodes.Status400BadRequest, $"The developer could not be created");
    }

    /// <summary>
    /// Updates a Developer
    /// </summary>
    /// <param name="developer">The Developer Object</param>
    /// <returns>The updated Developer</returns>
    /// <response code="201">Returns the updated developer</response>
    /// <response code="400">If the developer is null</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDeveloper([FromBody] Developer developer) 
    {
        var updatedDeveloper = await _developerService.UpdateDeveloper(developer);
        return updatedDeveloper != null ? Ok(updatedDeveloper) : StatusCode(StatusCodes.Status400BadRequest, $"The developer could not be created");
    }

    /// <summary>
    /// Deletes a specific Developer
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    /// <response code="204">If the delete operation was successful</response>
    /// <response code="404">If the developer was not found</response>
    [HttpDelete("{email}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDeveloper(string email)
    {
        var deletedDeveloperEmail = await _developerService.DeleteDeveloper(email);
        return deletedDeveloperEmail != "" ? NoContent() : StatusCode(StatusCodes.Status404NotFound, $"The developer could not be deleted"); 
    }
}