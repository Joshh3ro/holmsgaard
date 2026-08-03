using Holmsgaard.ApiService.Domain.Entities;
using Holmsgaard.ApiService.Infrastructure.InMemory;

namespace Holmsgaard.Tests;

public class InMemoryRepositoryTests
{
    [Fact]
    public void CustomerRepositoryStoresAddedCustomer()
    {
        var repository = new InMemoryCustomerRepository();
        var customer = new Customer(Guid.NewGuid(), "Repository Kunde", "repo@example.com", "12345678");

        repository.Add(customer);

        Assert.Same(customer, repository.GetById(customer.Id));
    }

    [Fact]
    public void TimeRegistrationRepositoryReturnsRegistrationsByEmployee()
    {
        var repository = new InMemoryTimeRegistrationRepository();
        var employeeId = Guid.NewGuid();
        var activityId = Guid.NewGuid();
        repository.Add(new TimeRegistration(Guid.NewGuid(), employeeId, activityId, new DateOnly(2026, 5, 23), 4m, "Test"));

        var registrations = repository.GetByEmployeeId(employeeId);

        Assert.Single(registrations);
    }
}
