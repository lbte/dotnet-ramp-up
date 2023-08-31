using PRFTLatam.Training.JwtAuthentication.Service.Dtos;
using PRFTLatam.Training.JwtAuthentication.Service.Models;

namespace PRFTLatam.Training.JwtAuthentication.Service.Services.Interfaces;

public interface IUserService
{
    Task<IReadOnlyCollection<User>> GetUsersAsync();
    Task<User> GetUserAsync(Guid id);
    Task<User> CreateUserAsync(UserDto user);
    Task DeleteUserAsync(Guid id);
    Task UpdateUserAsync(Guid id, UserDto user);
}