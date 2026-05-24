using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Queries;
using Holmsgaard.ApiService.Application.Services;
using Holmsgaard.ApiService.Infrastructure.InMemory;

namespace Holmsgaard.Tests;

public class ProductServiceTests
{
    [Fact]
    public void CreateProductAddsProduct()
    {
        var service = new ProductService(new InMemoryProductRepository());

        var product = service.CreateProduct(new CreateProductCommand("Spade", "SPA-001", 199m, 10));

        Assert.Equal("Spade", product.Name);
        Assert.Equal(199m, product.UnitPrice);
    }

    [Fact]
    public void UpdateProductChangesValues()
    {
        var service = new ProductService(new InMemoryProductRepository());
        var product = service.CreateProduct(new CreateProductCommand("Spade", "SPA-001", 199m, 10));

        var updated = service.UpdateProduct(new UpdateProductCommand(product.Id, "Rive", "RIV-001", 149m, 5));

        Assert.NotNull(updated);
        Assert.Equal("Rive", updated.Name);
        Assert.Equal(5, updated.StockQuantity);
    }

    [Fact]
    public void DeleteProductReturnsFalseWhenProductDoesNotExist()
    {
        var service = new ProductService(new InMemoryProductRepository());

        var deleted = service.DeleteProduct(Guid.NewGuid());

        Assert.False(deleted);
    }
}
