using RstApiExample.DTO.Responses;
using RstApiExample.Entities;

namespace RstApiExample.Extensions;

public static class UserExtensions
{
    public static UserResponseDTO ToResponseDTO(this User user)
    {
        return new UserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
    
    public static IEnumerable<UserResponseDTO> ToResponseDTOs(this IEnumerable<User> users)
    {
        return users.Select(user => user.ToResponseDTO());
    }
}