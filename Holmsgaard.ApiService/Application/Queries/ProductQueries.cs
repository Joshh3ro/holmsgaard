namespace Holmsgaard.ApiService.Application.Queries;

public sealed record GetProductsQuery(bool IncludeInactive = false);

public sealed record GetProductByIdQuery(Guid Id);
