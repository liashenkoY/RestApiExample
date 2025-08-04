using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RstApiExample.Abstraction;
using RstApiExample.DTO.Requests;
using RstApiExample.Extensions;

namespace RstApiExample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _userService.GetAllUsersAsync();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(result.ToResponseDTOs());
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid id.");
        }

        var result = await _userService.GetUserByIdAsync(id);
        if (result is null)
        {
            return NotFound("User is not found.");
        }

        return Ok(result.ToResponseDTO());
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateOrUpdateDTO userCreateOrUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.CreateUserAsync(userCreateOrUpdateDto);
        if (result is null)
        {
            return BadRequest("User already exists.");
        }

        return Ok("Success");
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserCreateOrUpdateDTO userUpdateDto)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid id.");
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _userService.UpdateUserAsync(id, userUpdateDto);
        if (!result)
        {
            return NotFound("User is not found or email is already used.");
        }
        
        return Ok("Success");
    }

    [Authorize]
    [HttpPatch("{id}/email")]
    public async Task<IActionResult> ChangeEmail(int id, [FromBody] ChangeEmailDTO changeEmailDto)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid id.");
        }
        
        var result = await _userService.ChangeEmailAsync(id, changeEmailDto.Email);
        if (!result)
        {
            return NotFound("User is not found or email is already used.");
        }
        
        return Ok("Success");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid id.");
        }
        
        var result = await _userService.DeleteUserAsync(id);
        if (!result)
        {
            return NotFound("User is not found.");
        }
        
        return Ok("Success");
    }
}