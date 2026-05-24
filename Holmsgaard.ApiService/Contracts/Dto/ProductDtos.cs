namespace Holmsgaard.ApiService.Contracts.Dto;

public sealed record ProductDto(Guid Id, string Name, string Sku, decimal UnitPrice, int StockQuantity, bool IsActive);

public sealed record CreateProductRequest(string Name, string Sku, decimal UnitPrice, int StockQuantity);

public sealed record UpdateProductRequest(string Name, string Sku, decimal UnitPrice, int StockQuantity);
