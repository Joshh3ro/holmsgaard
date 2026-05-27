using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Application.Commands;

public sealed record CreateProductCommand(string Name, string Sku, decimal UnitPrice, int StockQuantity, ProductCategory Category);

public sealed record UpdateProductCommand(Guid Id, string Name, string Sku, decimal UnitPrice, int StockQuantity, ProductCategory? Category);
