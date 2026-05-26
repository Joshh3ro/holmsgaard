using System.Security.Cryptography;
using System.Text;
using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Holmsgaard.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IEmployeeRepository employeeRepository) : ControllerBase
{
    [HttpPost("login")]
    public ActionResult<AuthResponse> Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Email and password are required.");
        }

        var employee = employeeRepository.GetAll()
            .FirstOrDefault(e => e.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));

        if (employee is null)
        {
            return Unauthorized("Invalid email or password.");
        }

        var hash = HashPassword(request.Password);
        if (employee.PasswordHash != hash)
        {
            return Unauthorized("Invalid email or password.");
        }

        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        return Ok(new AuthResponse(
            Token: token,
            TokenType: "Bearer",
            ExpiresAt: DateTimeOffset.UtcNow.AddHours(8)));
    }

    [HttpPost("register")]
    public ActionResult<AuthResponse> Register(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Full name, email and password are required.");
        }

        var existing = employeeRepository.GetAll()
            .FirstOrDefault(e => e.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));

        if (existing is not null)
        {
            return BadRequest("An employee with this email already exists.");
        }

        var hash = HashPassword(request.Password);
        // HourlyRate default 0 for registered users
        var employee = new Domain.Entities.Employee(
            Guid.NewGuid(), request.FullName, request.Email, 0m, hash);

        employeeRepository.Add(employee);

        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        return Ok(new AuthResponse(
            Token: token,
            TokenType: "Bearer",
            ExpiresAt: DateTimeOffset.UtcNow.AddHours(8)));
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password + "HolmsgaardSalt2026"));
        return Convert.ToBase64String(bytes);
    }
}
