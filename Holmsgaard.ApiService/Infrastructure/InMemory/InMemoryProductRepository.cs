using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Infrastructure.InMemory;

public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly Dictionary<Guid, Product> products = [];

    public InMemoryProductRepository()
    {
        // Materiale
        var m1 = new Product(Guid.Parse("44444444-4444-4444-4444-444444444441"), "Grundplanker (fyr)", "MAT-FYR-001", 49.95m, 100, ProductCategory.Materiale);
        var m2 = new Product(Guid.Parse("44444444-4444-4444-4444-444444444442"), "PVC trykrør 32mm", "MAT-PVC-032", 89.00m, 50, ProductCategory.Materiale);
        var m3 = new Product(Guid.Parse("44444444-4444-4444-4444-444444444443"), "Gulvfliser 60x60cm", "MAT-FLI-6060", 129.00m, 30, ProductCategory.Materiale);
        var m4 = new Product(Guid.Parse("44444444-4444-4444-4444-444444444444"), "Standard jordblanding", "MAT-JORD-001", 89.95m, 25, ProductCategory.Materiale);
        var m5 = new Product(Guid.Parse("44444444-4444-4444-4444-444444444445"), "Cement 25kg", "MAT-CEM-025", 59.00m, 40, ProductCategory.Materiale);

        // Værktøj
        var v1 = new Product(Guid.Parse("44444444-4444-4444-4444-444444444451"), "Hammer 500g", "VAERK-HAM-500", 149.00m, 20, ProductCategory.Værktøj);
        var v2 = new Product(Guid.Parse("44444444-4444-4444-4444-444444444452"), "Boremaskine 18V", "VAERK-BOR-18V", 899.00m, 10, ProductCategory.Værktøj);
        var v3 = new Product(Guid.Parse("44444444-4444-4444-4444-444444444453"), "Skruetrækker-sæt 12 dele", "VAERK-SKR-12", 249.00m, 15, ProductCategory.Værktøj);
        var v4 = new Product(Guid.Parse("44444444-4444-4444-4444-444444444454"), "Vaterpass 60cm", "VAERK-VAT-60", 199.00m, 12, ProductCategory.Værktøj);
        var v5 = new Product(Guid.Parse("44444444-4444-4444-4444-444444444455"), "Stige 3-trins alu", "VAERK-STI-3", 699.00m, 8, ProductCategory.Værktøj);

        products[m1.Id] = m1;
        products[m2.Id] = m2;
        products[m3.Id] = m3;
        products[m4.Id] = m4;
        products[m5.Id] = m5;
        products[v1.Id] = v1;
        products[v2.Id] = v2;
        products[v3.Id] = v3;
        products[v4.Id] = v4;
        products[v5.Id] = v5;
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
        if (products.TryGetValue(id, out var product))
        {
            product.Deactivate();
        }
    }

    public void SaveChanges()
    {
    }
}
