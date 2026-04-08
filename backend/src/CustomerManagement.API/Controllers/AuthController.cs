using CustomerManagement.Application.DTOs;
using CustomerManagement.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _tokenService;
    private readonly UserService _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(JwtTokenService tokenService, UserService userService, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login attempt for User: '{Username}' with Password: '{Password}'", 
            request.Username, request.Password);
        var (success, role, userId, assignedId) = _userService.ValidateUser(request.Username, request.Password);

        if (!success)
            return Unauthorized(new { message = "Invalid username or password." });

        var (token, expiresAt) = _tokenService.GenerateToken(request.Username, role, userId, assignedId);

        return Ok(new LoginResponse(token, request.Username, role, expiresAt));
    }
}