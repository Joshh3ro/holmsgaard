using Holmsgaard.ApiService.Domain.Enums;

namespace Holmsgaard.ApiService.Contracts.Dto;

public sealed record TimeRegistrationDto(Guid Id, Guid EmployeeId, Guid ActivityId, DateOnly WorkDate, decimal Hours, string Note, RegistrationType Type);

public sealed record RegisterTimeRequest(Guid EmployeeId, Guid ActivityId, DateOnly WorkDate, decimal Hours, string Note, RegistrationType Type = RegistrationType.Normal);

public sealed record UpdateTimeRegistrationRequest(Guid EmployeeId, Guid ActivityId, DateOnly WorkDate, decimal Hours, string Note, RegistrationType Type = RegistrationType.Normal);

public sealed record EmployeeTimeSummaryDto(Guid EmployeeId, string EmployeeName, decimal TotalHours, decimal HourlyRate, decimal TotalCost);
