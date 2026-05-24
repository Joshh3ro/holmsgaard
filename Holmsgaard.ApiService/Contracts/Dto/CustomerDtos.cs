namespace Holmsgaard.ApiService.Contracts.Dto;

public sealed record CustomerDto(Guid Id, string Name, string Email, string Phone, bool IsActive);

public sealed record CreateCustomerRequest(string Name, string Email, string Phone);

public sealed record UpdateCustomerRequest(string Name, string Email, string Phone);
