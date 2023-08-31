using PRFTLatam.Training.JwtAuthentication.Service.Enums;

namespace PRFTLatam.Training.JwtAuthentication.Service.Dtos;

public class UserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
}