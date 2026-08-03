using System.Security.Claims;
using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Contracts.Dto;
using Holmsgaard.ApiService.Domain.Entities;
using Holmsgaard.ApiService.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Holmsgaard.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(
    IEmployeeRepository employeeRepository,
    IPasswordHasher<Employee> passwordHasher,
    JwtTokenService tokenService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<AuthResponse> Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Email and password are required.");
        }

        var employee = employeeRepository.GetAll()
            .FirstOrDefault(e => e.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));

        if (employee is null || !employee.IsActive)
        {
            return Unauthorized("Invalid email or password.");
        }

        var verification = passwordHasher.VerifyHashedPassword(
            employee,
            employee.PasswordHash,
            request.Password);

        if (verification == PasswordVerificationResult.Failed)
        {
            return Unauthorized("Invalid email or password.");
        }

        if (verification == PasswordVerificationResult.SuccessRehashNeeded)
        {
            employee.SetPassword(passwordHasher.HashPassword(employee, request.Password));
            employeeRepository.Update(employee);
            employeeRepository.SaveChanges();
        }

        return Ok(tokenService.CreateToken(employee));
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public ActionResult<AuthResponse> Register(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Full name, email and password are required.");
        }

        if (request.Password.Length < 8)
        {
            return BadRequest("Password must contain at least 8 characters.");
        }

        var existing = employeeRepository.GetAll()
            .FirstOrDefault(e => e.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));

        if (existing is not null)
        {
            return BadRequest("An employee with this email already exists.");
        }

        var employee = new Domain.Entities.Employee(
            Guid.NewGuid(), request.FullName, request.Email, 0m);
        employee.SetPassword(passwordHasher.HashPassword(employee, request.Password));

        employeeRepository.Add(employee);

        return Ok(tokenService.CreateToken(employee));
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult<CurrentUserResponse> GetCurrentUser()
    {
        var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(idValue, out var employeeId))
        {
            return Unauthorized();
        }

        return Ok(new CurrentUserResponse(
            employeeId,
            User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
            User.FindFirstValue(ClaimTypes.Email) ?? string.Empty));
    }
}
