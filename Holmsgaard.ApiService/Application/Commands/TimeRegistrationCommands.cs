namespace Holmsgaard.ApiService.Application.Commands;

public sealed record RegisterEmployeeTimeCommand(Guid EmployeeId, Guid ActivityId, DateOnly WorkDate, decimal Hours, string Note);

public sealed record UpdateTimeRegistrationCommand(Guid Id, Guid EmployeeId, Guid ActivityId, DateOnly WorkDate, decimal Hours, string Note);
