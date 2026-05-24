using Holmsgaard.ApiService.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Holmsgaard.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    [HttpPost("login")]
    public ActionResult<AuthResponse> Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Email and password are required.");
        }

        var response = new AuthResponse(
            Token: "mock-jwt-token-for-frontend-development",
            TokenType: "Bearer",
            ExpiresAt: DateTimeOffset.UtcNow.AddHours(1));

        return Ok(response);
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Full name, email and password are required.");
        }

        return StatusCode(StatusCodes.Status501NotImplemented, "Registration is prepared for the API contract, but full authentication is not implemented yet.");
    }
}
