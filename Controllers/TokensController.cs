using Microsoft.AspNetCore.Mvc;
using RstApiExample.Abstraction;
using RstApiExample.DTO.Requests;

namespace RstApiExample.Controllers;

[Route("api/[controller]")]
public class TokensController : ControllerBase
{
    private readonly IUserService _userService;

    public TokensController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> LogIn([FromBody] UserLogInDTO userLogInDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _userService.LogInUserAsync(userLogInDto);

        if (result is null)
        {
            return NotFound("User is not found or password is incorrect.");
        }
        
        return Ok(result);
    }
}