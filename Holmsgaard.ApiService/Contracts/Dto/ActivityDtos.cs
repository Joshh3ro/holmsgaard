namespace Holmsgaard.ApiService.Contracts.Dto;

public sealed record ActivityDto(Guid Id, Guid CustomerId, string Title, string Description, bool IsActive);

public sealed record CreateActivityRequest(Guid CustomerId, string Title, string Description);

public sealed record UpdateActivityRequest(Guid CustomerId, string Title, string Description);
