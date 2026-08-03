using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Application.Interfaces;

public interface IWarehouseRepository
{
    IReadOnlyCollection<WarehousePurchase> GetAll();
    IReadOnlyCollection<WarehousePurchase> GetByProductId(Guid productId);
    void Add(WarehousePurchase purchase);
    IReadOnlyCollection<Product> GetAllProducts();
    Product? GetProductById(Guid id);
    void UpdateProductStock(Guid productId, int newStockQuantity);
    void SaveChanges();
}
