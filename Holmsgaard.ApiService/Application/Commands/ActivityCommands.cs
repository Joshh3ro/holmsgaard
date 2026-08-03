namespace Holmsgaard.ApiService.Application.Commands;

public sealed record CreateActivityCommand(Guid CustomerId, string Title, string Description);

public sealed record UpdateActivityCommand(Guid Id, Guid CustomerId, string Title, string Description);
