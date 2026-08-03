using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Application.Queries;
using Holmsgaard.ApiService.Contracts.Dto;
using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Application.Services;

public sealed class ProductService(IProductRepository productRepository)
{
    public IReadOnlyCollection<ProductDto> GetProducts(GetProductsQuery query)
    {
        return productRepository.GetAll()
            .Where(product => query.IncludeInactive || product.IsActive)
            .Select(product => product.ToDto())
            .ToArray();
    }

    public ProductDto? GetProduct(GetProductByIdQuery query)
    {
        return productRepository.GetById(query.Id)?.ToDto();
    }

    public ProductDto CreateProduct(CreateProductCommand command)
    {
        var product = new Product(Guid.NewGuid(), command.Name, command.Sku, command.UnitPrice, command.StockQuantity, command.Category);
        productRepository.Add(product);
        return product.ToDto();
    }

    public ProductDto? UpdateProduct(UpdateProductCommand command)
    {
        var product = productRepository.GetById(command.Id);
        if (product is null)
        {
            return null;
        }

        product.Update(command.Name, command.Sku, command.UnitPrice, command.StockQuantity, command.Category);
        productRepository.SaveChanges(product, command.RowVersion);
        return product.ToDto();
    }

    public bool DeleteProduct(Guid id)
    {
        if (productRepository.GetById(id) is null)
        {
            return false;
        }

        productRepository.Delete(id);
        return true;
    }
}
