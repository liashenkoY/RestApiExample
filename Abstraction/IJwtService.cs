using RstApiExample.Entities;

namespace RstApiExample.Abstraction;

public interface IJwtService
{
    string GenerateJwtToken(User user);
}