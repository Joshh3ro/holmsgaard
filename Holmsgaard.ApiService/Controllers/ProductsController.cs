using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Queries;
using Holmsgaard.ApiService.Application.Services;
using Holmsgaard.ApiService.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Holmsgaard.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController(ProductService productService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyCollection<ProductDto>> GetProducts([FromQuery] bool includeInactive = false)
    {
        return Ok(productService.GetProducts(new GetProductsQuery(includeInactive)));
    }

    [HttpGet("{id:guid}")]
    public ActionResult<ProductDto> GetProduct(Guid id)
    {
        var product = productService.GetProduct(new GetProductByIdQuery(id));
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public ActionResult<ProductDto> CreateProduct(CreateProductRequest request)
    {
        try
        {
            var product = productService.CreateProduct(new CreateProductCommand(request.Name, request.Sku, request.UnitPrice, request.StockQuantity));
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public ActionResult<ProductDto> UpdateProduct(Guid id, UpdateProductRequest request)
    {
        try
        {
            var product = productService.UpdateProduct(new UpdateProductCommand(id, request.Name, request.Sku, request.UnitPrice, request.StockQuantity));
            return product is null ? NotFound() : Ok(product);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteProduct(Guid id)
    {
        return productService.DeleteProduct(id) ? NoContent() : NotFound();
    }
}
