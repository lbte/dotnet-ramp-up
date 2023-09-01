using System.Security.Claims;

namespace PRFTLatam.Training.JwtAuthentication.Service.Models;

public class AuthenticatedResponse
{
    public string Token { get; set; }
    public Dictionary<string, string> UserClaims { get; set; }
}