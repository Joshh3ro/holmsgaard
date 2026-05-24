namespace Holmsgaard.ApiService.Application.Commands;

public sealed record CreateEmployeeCommand(string FullName, string Email, decimal HourlyRate);

public sealed record UpdateEmployeeCommand(Guid Id, string FullName, string Email, decimal HourlyRate);
