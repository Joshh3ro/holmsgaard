namespace Holmsgaard.ApiService.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; }
    public string Name { get; private set; } = string.Empty;
    public string Sku { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public int StockQuantity { get; private set; }
    public bool IsActive { get; private set; }
    public ProductCategory Category { get; private set; }

    public Product(Guid id, string name, string sku, decimal unitPrice, int stockQuantity, ProductCategory category = ProductCategory.Materiale)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        IsActive = true;
        Update(name, sku, unitPrice, stockQuantity, category);
    }

    public void Update(string name, string sku, decimal unitPrice, int stockQuantity, ProductCategory? category = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("Product sku is required.", nameof(sku));
        }

        if (unitPrice < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");
        }

        if (stockQuantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(stockQuantity), "Stock quantity cannot be negative.");
        }

        Name = name.Trim();
        Sku = sku.Trim();
        UnitPrice = unitPrice;
        StockQuantity = stockQuantity;
        if (category.HasValue)
        {
            Category = category.Value;
        }
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
