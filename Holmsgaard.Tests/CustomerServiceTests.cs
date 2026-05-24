using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Queries;
using Holmsgaard.ApiService.Application.Services;
using Holmsgaard.ApiService.Infrastructure.InMemory;

namespace Holmsgaard.Tests;

public class CustomerServiceTests
{
    [Fact]
    public void CreateCustomerAddsCustomer()
    {
        var repository = new InMemoryCustomerRepository();
        var service = new CustomerService(repository);

        var created = service.CreateCustomer(new CreateCustomerCommand("Ny Kunde", "ny@example.com", "12345678"));

        Assert.Equal("Ny Kunde", created.Name);
        Assert.NotNull(service.GetCustomer(new GetCustomerByIdQuery(created.Id)));
    }

    [Fact]
    public void UpdateCustomerReturnsNullWhenCustomerDoesNotExist()
    {
        var service = new CustomerService(new InMemoryCustomerRepository());

        var updated = service.UpdateCustomer(new UpdateCustomerCommand(Guid.NewGuid(), "Ukendt", "ukendt@example.com", "12345678"));

        Assert.Null(updated);
    }

    [Fact]
    public void DeleteCustomerRemovesCustomer()
    {
        var repository = new InMemoryCustomerRepository();
        var service = new CustomerService(repository);
        var created = service.CreateCustomer(new CreateCustomerCommand("Slet Mig", "delete@example.com", "12345678"));

        var deleted = service.DeleteCustomer(created.Id);

        Assert.True(deleted);
        Assert.Null(service.GetCustomer(new GetCustomerByIdQuery(created.Id)));
    }
}
