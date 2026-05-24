namespace Holmsgaard.ApiService.Application.Commands;

public sealed record CreateCustomerCommand(string Name, string Email, string Phone);

public sealed record UpdateCustomerCommand(Guid Id, string Name, string Email, string Phone);
