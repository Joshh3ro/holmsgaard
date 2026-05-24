using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Infrastructure.InMemory;

public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly Dictionary<Guid, Product> products = [];

    public InMemoryProductRepository()
    {
        var product = new Product(Guid.Parse("44444444-4444-4444-4444-444444444444"), "Standard jordblanding", "JORD-001", 89.95m, 25);
        products[product.Id] = product;
    }

    public IReadOnlyCollection<Product> GetAll()
    {
        return products.Values.ToArray();
    }

    public Product? GetById(Guid id)
    {
        return products.GetValueOrDefault(id);
    }

    public void Add(Product product)
    {
        products[product.Id] = product;
    }

    public void Delete(Guid id)
    {
        products.Remove(id);
    }
}
