using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using PRFTLatam.Training.JwtAuthentication.Service.Enums;
using PRFTLatam.Training.JwtAuthentication.Service.Models;
using PRFTLatam.Training.JwtAuthentication.Service.Services.Interfaces;

namespace PRFTLatam.Training.JwtAuthentication.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private const string Issuer = "https://localhost:5001";

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("Login")]
    [ProducesResponseType(typeof(AuthenticatedResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Login([FromBody] Login user)
    {
        if (user is null)
            return BadRequest("Invalid client request");

        if (user.Email == "john@test.com" && user.Password == "123456")
        {
            var userInfo = await _userService.GetUserByEmailAsync(user.Email);
            var claims = new List<Claim>
            {   
                new Claim("Name", userInfo.Name),
                new Claim("Email", user.Email),
                new Claim("Role", userInfo.Role.ToString()),
                new Claim("IsActiveRole", userInfo.IsActiveRole.ToString()),
                new Claim(ClaimTypes.Role, userInfo.Role.ToString(), ClaimValueTypes.String, Issuer)
            };
            var tokenString = GenerateJWT(claims);

            return Ok(new AuthenticatedResponse{Token = tokenString, UserClaims = GetClaims(claims)}); // As a response, we create the AuthenticatedResponse object that contains only the Token property
        }
        else if (user.Email == "karl@test.com" && user.Password == "qwerty")
        {   
            var userInfo = await _userService.GetUserByEmailAsync(user.Email);
            var claims = new List<Claim>
            {   
                new Claim("Name", userInfo.Name),
                new Claim("Email", user.Email),
                new Claim("Role", userInfo.Role.ToString()),
                new Claim("IsActiveRole", userInfo.IsActiveRole.ToString()),
                new Claim(ClaimTypes.Role, userInfo.Role.ToString(), ClaimValueTypes.String, Issuer)
            };
            var tokenString = GenerateJWT(claims);

            return Ok(new AuthenticatedResponse{Token = tokenString, UserClaims = GetClaims(claims)}); // As a response, we create the AuthenticatedResponse object that contains only the Token property
        }
        else if (user.Email == "sammy@test.com" && user.Password == "s4mmy")
        {
            var userInfo = await _userService.GetUserByEmailAsync(user.Email);
            var claims = new List<Claim>
            {   
                new Claim("Name", userInfo.Name),
                new Claim("Email", user.Email),
                new Claim("Role", userInfo.Role.ToString()),
                new Claim("IsActiveRole", userInfo.IsActiveRole.ToString()),
                new Claim(ClaimTypes.Role, userInfo.Role.ToString(), ClaimValueTypes.String, Issuer)
            };
            var tokenString = GenerateJWT(claims);

            return Ok(new AuthenticatedResponse{Token = tokenString, UserClaims = GetClaims(claims)}); // As a response, we create the AuthenticatedResponse object that contains only the Token property
        }
        return Unauthorized("Invalid credetials"); //401
    }

    private string GenerateJWT(List<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokenOptions = new JwtSecurityToken(
                issuer: Issuer, // name of the webserver that issues the token
                audience: "https://localhost:5001", // valid recipients
                claims: claims, // list of user roles
                expires: DateTime.Now.AddMinutes(5), // date and time after which the token expires
                signingCredentials: signingCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private Dictionary<string, string> GetClaims(List<Claim> claims)
    {
        var newClaims = new Dictionary<string, string>();

        foreach (var claim in claims)
        {
            if (claim.Type == ClaimTypes.Role)
                continue;
            newClaims.Add(claim.Type, claim.Value);
        }

        return newClaims;
    }
}
