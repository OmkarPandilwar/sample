using CustomerManagement.Application.DTOs;
using CustomerManagement.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _tokenService;
    private readonly UserService _userService;

    public AuthController(JwtTokenService tokenService, UserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var (success, role, userId, assignedId) = _userService.ValidateUser(request.Username, request.Password);

        if (!success)
            return Unauthorized(new { message = "Invalid username or password." });

        var (token, expiresAt) = _tokenService.GenerateToken(request.Username, role, userId, assignedId);

        return Ok(new LoginResponse(token, request.Username, role, expiresAt));
    }
}