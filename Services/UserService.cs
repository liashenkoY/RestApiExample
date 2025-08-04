using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RstApiExample.Abstraction;
using RstApiExample.Data;
using RstApiExample.DTO.Requests;
using RstApiExample.Entities;

namespace RstApiExample.Services;

public class UserService: IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public UserService(
        ApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtService jwtService,
        IMemoryCache cache)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _cache = cache;
        _cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
    }
    
    public async Task<User?> CreateUserAsync(UserCreateOrUpdateDTO userOrUpdateDto)
    {
        var existingUser = await GetUserByEmailAsync(userOrUpdateDto.Email);
        if (existingUser is not null)
        {
            return null;
        }
        
        var passwordHash = _passwordHasher.HashPassword(userOrUpdateDto.Password);
        
        var user = new User
        {
            Name = userOrUpdateDto.Name,
            Email = userOrUpdateDto.Email,
            PasswordHash = passwordHash
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var cacheKey = $"user_{id}";
        if (!_cache.TryGetValue(cacheKey, out User? user))
        {
            user = await _context.Users.FindAsync(id);
            _cache.Set(cacheKey, user, _cacheOptions);
        }
        
        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var cacheKey = $"user_{email}";
        if (!_cache.TryGetValue(cacheKey, out User? user))
        {
            user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            _cache.Set(cacheKey, user, _cacheOptions);
        }

        return user;
    }
    
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        const string cacheKey = "users_all";
        if (!_cache.TryGetValue(cacheKey, out IEnumerable<User>? users))
        {
            users = await _context.Users.ToListAsync();
            _cache.Set(cacheKey, users, _cacheOptions);
        }
        
        return users;
    }

    public async Task<bool> UpdateUserAsync(int id, UserCreateOrUpdateDTO user)
    {
        var idCacheKey = $"user_{id}";
        if (!_cache.TryGetValue(idCacheKey, out User? existingUser))
        {
            existingUser = await _context.Users.FindAsync(id);
        }
        
        var emailCacheKey = $"user_{user.Email}";
        if (!_cache.TryGetValue(emailCacheKey, out User? existingUserWithEmail))
        {
            existingUserWithEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        }
        
        if (existingUser == null || existingUserWithEmail != null)
        {
            return false;
        }
        
        _context.Users.Attach(existingUser);
        
        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.PasswordHash = _passwordHasher.HashPassword(user.Password);
        
        _cache.Set(idCacheKey, existingUser, _cacheOptions);
        _cache.Set(emailCacheKey, existingUser, _cacheOptions);
        _cache.Remove($"users_all");
        
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> ChangeEmailAsync(int id, string newEmail)
    {
        var idCacheKey = $"user_{id}";
        if (!_cache.TryGetValue(idCacheKey, out User? user))
        {
            user = await _context.Users.FindAsync(id);
        }
        
        if (user is null)
        {
            return false;
        }
        
        var emailCacheKey = $"user_{newEmail}";
        if (!_cache.TryGetValue(emailCacheKey, out User? existingUserWithEmail))
        {
            existingUserWithEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == newEmail);
        }

        if (existingUserWithEmail != null)
        {
            return false;
        }
        
        _context.Users.Attach(user);
        
        _cache.Remove($"user_{user.Email}");
        
        user.Email = newEmail;
        
        _cache.Set(idCacheKey, user, _cacheOptions);
        _cache.Set(emailCacheKey, user, _cacheOptions);
        _cache.Remove($"users_all");
        
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var idCacheKey = $"user_{id}";
        if (!_cache.TryGetValue(idCacheKey, out User? user))
        {
            user = await _context.Users.FindAsync(id);
        }
        
        if (user is null)
        {
            return false;
        }

        _context.Users.Remove(user);
        
        _cache.Remove($"user_{user.Id}");
        _cache.Remove($"users_all");
        
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<string?> LogInUserAsync(UserLogInDTO user)
    {
        var userFromDb = await GetUserByEmailAsync(user.Email);
        if (userFromDb is null)
        {
            return null;
        }
        
        if (!_passwordHasher.VerifyPassword(userFromDb.PasswordHash, user.Password))
        {
            return null;
        }
        
        var token = _jwtService.GenerateJwtToken(userFromDb);
        
        return token;
    }
}