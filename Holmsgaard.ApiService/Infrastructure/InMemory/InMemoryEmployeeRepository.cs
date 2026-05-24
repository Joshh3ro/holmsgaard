using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Infrastructure.InMemory;

public sealed class InMemoryEmployeeRepository : IEmployeeRepository
{
    private readonly Dictionary<Guid, Employee> employees = [];

    public InMemoryEmployeeRepository()
    {
        var employee = new Employee(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Demo Medarbejder", "medarbejder@holmsgaard.local", 350m);
        employees[employee.Id] = employee;
    }

    public IReadOnlyCollection<Employee> GetAll()
    {
        return employees.Values.ToArray();
    }

    public Employee? GetById(Guid id)
    {
        return employees.GetValueOrDefault(id);
    }

    public void Add(Employee employee)
    {
        employees[employee.Id] = employee;
    }

    public void Delete(Guid id)
    {
        employees.Remove(id);
    }
}
