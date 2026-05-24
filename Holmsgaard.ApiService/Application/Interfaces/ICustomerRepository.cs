using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Application.Interfaces;

public interface ICustomerRepository
{
    IReadOnlyCollection<Customer> GetAll();
    Customer? GetById(Guid id);
    void Add(Customer customer);
    void Delete(Guid id);
}
