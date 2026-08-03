using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Contracts.Dto;

public sealed record InventorySummaryDto(
    int TotalProducts,
    int MaterialCount,
    int VaerktojCount,
    int TotalStockQuantity,
    decimal TotalStockValue,
    int TotalPurchaseCount,
    decimal TotalPurchasedValue);

public sealed record ProductWithPurchasesDto(
    Guid Id,
    string Name,
    string Sku,
    ProductCategory Category,
    int StockQuantity,
    decimal UnitPrice,
    bool IsActive,
    string RowVersion,
    List<PurchaseHistoryDto> Purchases);

public sealed record PurchaseHistoryDto(
    Guid Id,
    int Quantity,
    DateOnly PurchaseDate,
    decimal UnitPriceAtPurchase,
    string? Supplier,
    string? Note,
    decimal TotalValue);

public sealed record RecordPurchaseCommand(
    Guid ProductId,
    int Quantity,
    DateOnly PurchaseDate,
    decimal UnitPriceAtPurchase,
    string? Supplier,
    string? Note);

public sealed record UpdateStockCommand(
    Guid ProductId,
    int NewStockQuantity);
