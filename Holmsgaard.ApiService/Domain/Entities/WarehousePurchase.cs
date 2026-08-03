namespace Holmsgaard.ApiService.Domain.Entities;

public sealed class WarehousePurchase
{
    public Guid Id { get; }
    public Guid ProductId { get; }
    public int Quantity { get; private set; }
    public DateOnly PurchaseDate { get; private set; }
    public decimal UnitPriceAtPurchase { get; private set; }
    public string? Supplier { get; private set; }
    public string? Note { get; private set; }
    public decimal TotalValue => Quantity * UnitPriceAtPurchase;

    public WarehousePurchase(Guid id, Guid productId, int quantity, DateOnly purchaseDate, decimal unitPriceAtPurchase, string? supplier, string? note)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("ProductId is required.", nameof(productId));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        if (unitPriceAtPurchase < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unitPriceAtPurchase), "Unit price cannot be negative.");
        }

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        ProductId = productId;
        Quantity = quantity;
        PurchaseDate = purchaseDate;
        UnitPriceAtPurchase = unitPriceAtPurchase;
        Supplier = supplier?.Trim();
        Note = note?.Trim();
    }
}
