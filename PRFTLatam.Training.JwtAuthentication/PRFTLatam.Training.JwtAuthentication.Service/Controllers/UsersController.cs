using System.Net;
using Microsoft.AspNetCore.Mvc;
using PRFTLatam.Training.JwtAuthentication.Service.Dtos;
using PRFTLatam.Training.JwtAuthentication.Service.Models;
using PRFTLatam.Training.JwtAuthentication.Service.Services.Interfaces;

namespace PRFTLatam.Training.JwtAuthentication.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("GetUsers")]
    [ProducesResponseType(typeof(IReadOnlyCollection<User>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUsersAsync()
    {
        var users = await _userService.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("GetUserById/{id:Guid}")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetUserAsync([FromRoute] Guid id)
    {
        var user = await _userService.GetUserAsync(id);
        return user is not null ? Ok(user) : NotFound();
    }

    [HttpPost("CreateUser")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserDto user)
    {
        var newUser = await _userService.CreateUserAsync(user);
        return Created($"/users/{newUser.Id}", newUser);
    }

    [HttpPut("UpdateUser/{id:Guid}")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateUserAsync([FromRoute] Guid id, [FromBody] UserDto user)
    {
        await _userService.UpdateUserAsync(id, user);
        return Ok(user);
    }

    [HttpPut("DeleteUser/{id:Guid}")]
    [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
    {
        await _userService.DeleteUserAsync(id);
        return Ok();
    }
}