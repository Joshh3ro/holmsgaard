using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Contracts.Dto;
using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Application.Services;

public sealed class WarehouseService(IWarehouseRepository warehouseRepository)
{
    public InventorySummaryDto GetInventorySummary()
    {
        var products = warehouseRepository.GetAllProducts().Where(product => product.IsActive).ToList();
        var purchases = warehouseRepository.GetAll();

        var materialCount = products.Count(p => p.Category == ProductCategory.Materiale);
        var vaerktojCount = products.Count(p => p.Category == ProductCategory.Værktøj);
        var totalStockQuantity = products.Sum(p => p.StockQuantity);
        var totalStockValue = products.Sum(p => p.UnitPrice * p.StockQuantity);
        var totalPurchasedValue = purchases.Sum(p => p.TotalValue);

        return new InventorySummaryDto(
            products.Count,
            materialCount,
            vaerktojCount,
            totalStockQuantity,
            totalStockValue,
            purchases.Count,
            totalPurchasedValue);
    }

    public IReadOnlyCollection<ProductWithPurchasesDto> GetProductsWithPurchases()
    {
        var products = warehouseRepository.GetAllProducts().Where(product => product.IsActive);
        var purchases = warehouseRepository.GetAll();

        return products
            .Select(p => new ProductWithPurchasesDto(
                p.Id,
                p.Name,
                p.Sku,
                p.Category,
                p.StockQuantity,
                p.UnitPrice,
                p.IsActive,
                purchases
                    .Where(wp => wp.ProductId == p.Id)
                    .Select(wp => new PurchaseHistoryDto(
                        wp.Id,
                        wp.Quantity,
                        wp.PurchaseDate,
                        wp.UnitPriceAtPurchase,
                        wp.Supplier,
                        wp.Note,
                        wp.TotalValue))
                    .OrderByDescending(wp => wp.PurchaseDate)
                    .ToList()))
            .ToList();
    }

    public PurchaseHistoryDto RecordPurchase(RecordPurchaseCommand command)
    {
        var product = warehouseRepository.GetProductById(command.ProductId);
        if (product is null)
        {
            throw new ArgumentException("Product not found.", nameof(command.ProductId));
        }

        var purchase = new WarehousePurchase(
            Guid.NewGuid(),
            command.ProductId,
            command.Quantity,
            command.PurchaseDate,
            command.UnitPriceAtPurchase,
            command.Supplier,
            command.Note);

        warehouseRepository.Add(purchase);

        // Opdater lagerbeholdning
        var newStock = product.StockQuantity + command.Quantity;
        warehouseRepository.UpdateProductStock(command.ProductId, newStock);
        warehouseRepository.SaveChanges();

        return new PurchaseHistoryDto(
            purchase.Id,
            purchase.Quantity,
            purchase.PurchaseDate,
            purchase.UnitPriceAtPurchase,
            purchase.Supplier,
            purchase.Note,
            purchase.TotalValue);
    }

    public ProductWithPurchasesDto UpdateStock(UpdateStockCommand command)
    {
        var product = warehouseRepository.GetProductById(command.ProductId);
        if (product is null)
        {
            throw new ArgumentException("Product not found.", nameof(command.ProductId));
        }

        warehouseRepository.UpdateProductStock(command.ProductId, command.NewStockQuantity);
        warehouseRepository.SaveChanges();

        // Reload product
        var updated = warehouseRepository.GetProductById(command.ProductId)!;
        var purchases = warehouseRepository.GetByProductId(command.ProductId);

        return new ProductWithPurchasesDto(
            updated.Id,
            updated.Name,
            updated.Sku,
            updated.Category,
            updated.StockQuantity,
            updated.UnitPrice,
            updated.IsActive,
            purchases
                .Select(wp => new PurchaseHistoryDto(
                    wp.Id,
                    wp.Quantity,
                    wp.PurchaseDate,
                    wp.UnitPriceAtPurchase,
                    wp.Supplier,
                    wp.Note,
                    wp.TotalValue))
                .ToList());
    }
}
