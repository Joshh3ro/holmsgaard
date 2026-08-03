using Holmsgaard.ApiService;
using Holmsgaard.ApiService.Application.Exceptions;
using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Contracts.Dto;
using Holmsgaard.ApiService.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Json;

namespace Holmsgaard.Tests;

public sealed class ProductConcurrencyIntegrationTests
{
    [Fact]
    public async Task StaleProductVersionReturnsConflict()
    {
        await using var factory = new WebApplicationFactory<ApiAssemblyMarker>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseSetting("Jwt:Key", AuthIntegrationTests.TestSigningKey);
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<IProductRepository>();
                    services.AddSingleton<IProductRepository, ConflictProductRepository>();
                });
            });

        using var client = factory.CreateClient();
        var productId = ConflictProductRepository.ProductId;
        var request = new UpdateProductRequest(
            "Opdateret hammer",
            "HAM-001",
            199m,
            4,
            ProductCategory.Værktøj,
            Convert.ToBase64String([1, 2, 3, 4, 5, 6, 7, 8]));

        var response = await client.PutAsJsonAsync($"/api/products/{productId}", request);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("Samtidig ændring registreret", problem?.Title);
    }

    private sealed class ConflictProductRepository : IProductRepository
    {
        public static readonly Guid ProductId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
        private readonly Product product = new(ProductId, "Hammer", "HAM-001", 149m, 5, ProductCategory.Værktøj);

        public IReadOnlyCollection<Product> GetAll() => [product];

        public Product? GetById(Guid id) => id == ProductId ? product : null;

        public void Add(Product newProduct)
        {
        }

        public void Delete(Guid id)
        {
        }

        public void SaveChanges(Product changedProduct, byte[] expectedRowVersion)
        {
            throw new ConcurrencyConflictException(
                "Produktet blev ændret af en anden bruger. Genindlæs siden og prøv igen.");
        }
    }
}
