namespace Holmsgaard.ApiService.Contracts.Dto;

public sealed record EmployeeDto(Guid Id, string FullName, string Email, decimal HourlyRate, bool IsActive);

public sealed record CreateEmployeeRequest(string FullName, string Email, decimal HourlyRate);

public sealed record UpdateEmployeeRequest(string FullName, string Email, decimal HourlyRate);
