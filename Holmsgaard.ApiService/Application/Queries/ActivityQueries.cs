namespace Holmsgaard.ApiService.Application.Queries;

public sealed record GetActivitiesQuery(bool IncludeInactive = false);

public sealed record GetActivityByIdQuery(Guid Id);
