namespace Holmsgaard.ApiService.Contracts.Dto;

public sealed record LoginRequest(string Email, string Password);

public sealed record RegisterRequest(string FullName, string Email, string Password);

public sealed record AuthResponse(string Token, string TokenType, DateTimeOffset ExpiresAt);
