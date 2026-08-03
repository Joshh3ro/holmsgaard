using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Exceptions;
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
            var product = productService.CreateProduct(new CreateProductCommand(request.Name, request.Sku, request.UnitPrice, request.StockQuantity, request.Category));
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
            if (string.IsNullOrWhiteSpace(request.RowVersion))
            {
                return BadRequest("RowVersion er påkrævet.");
            }

            var rowVersion = Convert.FromBase64String(request.RowVersion);
            var product = productService.UpdateProduct(new UpdateProductCommand(
                id,
                request.Name,
                request.Sku,
                request.UnitPrice,
                request.StockQuantity,
                request.Category,
                rowVersion));
            return product is null ? NotFound() : Ok(product);
        }
        catch (ConcurrencyConflictException exception)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Samtidig ændring registreret",
                Detail = exception.Message
            });
        }
        catch (FormatException)
        {
            return BadRequest("RowVersion har et ugyldigt format.");
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
