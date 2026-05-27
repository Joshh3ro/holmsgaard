using Holmsgaard.ApiService.Application.Services;
using Holmsgaard.ApiService.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Holmsgaard.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class WarehouseController(WarehouseService warehouseService) : ControllerBase
{
    [HttpGet("summary")]
    public ActionResult<InventorySummaryDto> GetSummary()
    {
        return Ok(warehouseService.GetInventorySummary());
    }

    [HttpGet("products")]
    public ActionResult<IReadOnlyCollection<ProductWithPurchasesDto>> GetProducts()
    {
        return Ok(warehouseService.GetProductsWithPurchases());
    }

    [HttpPost("purchases")]
    public ActionResult<PurchaseHistoryDto> RecordPurchase([FromBody] RecordPurchaseRequest request)
    {
        try
        {
            var command = new RecordPurchaseCommand(
                request.ProductId,
                request.Quantity,
                request.PurchaseDate,
                request.UnitPriceAtPurchase,
                request.Supplier,
                request.Note);

            var result = warehouseService.RecordPurchase(command);
            return CreatedAtAction(nameof(GetProducts), result);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("stock")]
    public ActionResult<ProductWithPurchasesDto> UpdateStock([FromBody] UpdateStockRequest request)
    {
        try
        {
            var command = new UpdateStockCommand(request.ProductId, request.NewStockQuantity);
            var result = warehouseService.UpdateStock(command);
            return Ok(result);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}

public sealed record RecordPurchaseRequest(
    Guid ProductId,
    int Quantity,
    DateOnly PurchaseDate,
    decimal UnitPriceAtPurchase,
    string? Supplier,
    string? Note);

public sealed record UpdateStockRequest(
    Guid ProductId,
    int NewStockQuantity);
