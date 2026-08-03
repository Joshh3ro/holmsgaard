using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Infrastructure.InMemory;

public sealed class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly Dictionary<Guid, Customer> customers = [];

    public InMemoryCustomerRepository()
    {
        var customer = new Customer(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Holmsgaard Demo Kunde", "kunde@holmsgaard.local", "+45 12 34 56 78");
        customers[customer.Id] = customer;
    }

    public IReadOnlyCollection<Customer> GetAll()
    {
        return customers.Values.ToArray();
    }

    public Customer? GetById(Guid id)
    {
        return customers.GetValueOrDefault(id);
    }

    public void Add(Customer customer)
    {
        customers[customer.Id] = customer;
    }

    public void Delete(Guid id)
    {
        customers.Remove(id);
    }
}
