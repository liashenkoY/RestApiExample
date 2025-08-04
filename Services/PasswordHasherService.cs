using Microsoft.AspNetCore.Identity;
using RstApiExample.Abstraction;

namespace RstApiExample.Services;

public class PasswordHasherService: IPasswordHasher
{
    private readonly PasswordHasher<object> _passwordHasher;
    
    public PasswordHasherService()
    {
        _passwordHasher = new PasswordHasher<object>();
    }
    
    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null, password);
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
        return result == PasswordVerificationResult.Success;
    }
}