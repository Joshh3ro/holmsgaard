namespace Holmsgaard.ApiService.Application.Queries;

public sealed record GetEmployeesQuery(bool IncludeInactive = false);

public sealed record GetEmployeeByIdQuery(Guid Id);

public sealed record GetEmployeeTimeSummaryQuery(Guid EmployeeId);
