using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Data;
using Holmsgaard.ApiService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Holmsgaard.ApiService.Infrastructure.EfCore;

public sealed class EfCoreWarehouseRepository : IWarehouseRepository
{
    private readonly ApplicationDbContext _context;

    public EfCoreWarehouseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<WarehousePurchase> GetAll()
    {
        return [.. _context.WarehousePurchases];
    }

    public IReadOnlyCollection<WarehousePurchase> GetByProductId(Guid productId)
    {
        return [.. _context.WarehousePurchases.Where(wp => wp.ProductId == productId)];
    }

    public void Add(WarehousePurchase purchase)
    {
        _context.WarehousePurchases.Add(purchase);
    }

    public IReadOnlyCollection<Product> GetAllProducts()
    {
        return [.. _context.Products];
    }

    public Product? GetProductById(Guid id)
    {
        return _context.Products.Find(id);
    }

    public void UpdateProductStock(Guid productId, int newStockQuantity)
    {
        var product = _context.Products.Find(productId);
        if (product is not null)
        {
            product.Update(product.Name, product.Sku, product.UnitPrice, newStockQuantity, product.Category);
        }
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
