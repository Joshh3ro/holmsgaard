using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Application.Queries;
using Holmsgaard.ApiService.Contracts.Dto;
using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Application.Services;

public sealed class CustomerService(ICustomerRepository customerRepository)
{
    public IReadOnlyCollection<CustomerDto> GetCustomers(GetCustomersQuery query)
    {
        return customerRepository.GetAll()
            .Where(customer => query.IncludeInactive || customer.IsActive)
            .Select(customer => customer.ToDto())
            .ToArray();
    }

    public CustomerDto? GetCustomer(GetCustomerByIdQuery query)
    {
        return customerRepository.GetById(query.Id)?.ToDto();
    }

    public CustomerDto CreateCustomer(CreateCustomerCommand command)
    {
        var customer = new Customer(Guid.NewGuid(), command.Name, command.Email, command.Phone);
        customerRepository.Add(customer);
        return customer.ToDto();
    }

    public CustomerDto? UpdateCustomer(UpdateCustomerCommand command)
    {
        var customer = customerRepository.GetById(command.Id);
        if (customer is null)
        {
            return null;
        }

        customer.Update(command.Name, command.Email, command.Phone);
        return customer.ToDto();
    }

    public bool DeleteCustomer(Guid id)
    {
        if (customerRepository.GetById(id) is null)
        {
            return false;
        }

        customerRepository.Delete(id);
        return true;
    }
}
