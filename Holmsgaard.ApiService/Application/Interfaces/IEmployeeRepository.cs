using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Application.Interfaces;

public interface IEmployeeRepository
{
    IReadOnlyCollection<Employee> GetAll();
    Employee? GetById(Guid id);
    void Add(Employee employee);
    void Delete(Guid id);
}
