using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        if (request.Username == "sarah" && request.Password == "123")
        {
            var token = _tokenService.GenerateToken(request.Username);

            return Ok(new { token });
        }

        return Unauthorized();
    }
}