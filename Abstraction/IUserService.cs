using RstApiExample.DTO.Requests;
using RstApiExample.Entities;

namespace RstApiExample.Abstraction;

public interface IUserService
{
    Task<User?> CreateUserAsync(UserCreateOrUpdateDTO userOrUpdateDto);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<bool> UpdateUserAsync(int id, UserCreateOrUpdateDTO user);
    Task<bool> ChangeEmailAsync(int id, string newEmail);
    Task<bool> DeleteUserAsync(int id);
    Task<string?> LogInUserAsync(UserLogInDTO user);
}