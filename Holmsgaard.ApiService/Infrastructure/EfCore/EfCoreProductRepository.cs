using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Data;
using Holmsgaard.ApiService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Holmsgaard.ApiService.Infrastructure.EfCore;

public sealed class EfCoreProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public EfCoreProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<Product> GetAll()
    {
        return [.. _context.Products];
    }

    public Product? GetById(Guid id)
    {
        return _context.Products.Find(id);
    }

    public void Add(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var product = _context.Products.Find(id);
        if (product is not null)
        {
            product.Deactivate();
            _context.SaveChanges();
        }
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
