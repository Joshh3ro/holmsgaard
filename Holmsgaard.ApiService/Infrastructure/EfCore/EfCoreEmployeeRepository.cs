using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Data;
using Holmsgaard.ApiService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Holmsgaard.ApiService.Infrastructure.EfCore;

public sealed class EfCoreEmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EfCoreEmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<Employee> GetAll()
    {
        // Enumerate the DbSet to execute the query immediately and return a concrete collection.
        return _context.Employees.ToArray();
    }

    public Employee? GetById(Guid id)
    {
        return _context.Employees.Find(id);
    }

    public void Add(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
    }

    public void Update(Employee employee)
    {
        _context.Employees.Update(employee);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var employee = _context.Employees.Find(id);
        if (employee is not null)
        {
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }
    }
}