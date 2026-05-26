using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Data;
using Holmsgaard.ApiService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Holmsgaard.ApiService.Infrastructure.EfCore;

public sealed class EfCoreCustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public EfCoreCustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<Customer> GetAll()
    {
        return [.. _context.Customers];
    }

    public Customer? GetById(Guid id)
    {
        return _context.Customers.Find(id);
    }

    public void Add(Customer customer)
    {
        _context.Customers.Add(customer);
    }

    public void Delete(Guid id)
    {
        var customer = _context.Customers.Find(id);
        if (customer is not null)
        {
            _context.Customers.Remove(customer);
        }
    }
}