namespace Holmsgaard.ApiService.Application.Queries;

public sealed record GetCustomersQuery(bool IncludeInactive = false);

public sealed record GetCustomerByIdQuery(Guid Id);
