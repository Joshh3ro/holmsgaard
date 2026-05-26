using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Application.Queries;
using Holmsgaard.ApiService.Contracts.Dto;
using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Application.Services;

public sealed class EmployeeService(IEmployeeRepository employeeRepository)
{
    public IReadOnlyCollection<EmployeeDto> GetEmployees(GetEmployeesQuery query)
    {
        return employeeRepository.GetAll()
            .Where(employee => query.IncludeInactive || employee.IsActive)
            .Select(employee => employee.ToDto())
            .ToArray();
    }

    public EmployeeDto? GetEmployee(GetEmployeeByIdQuery query)
    {
        return employeeRepository.GetById(query.Id)?.ToDto();
    }

    public EmployeeDto CreateEmployee(CreateEmployeeCommand command)
    {
        var employee = new Employee(Guid.NewGuid(), command.FullName, command.Email, command.HourlyRate);
        employeeRepository.Add(employee);
        return employee.ToDto();
    }

    public EmployeeDto? UpdateEmployee(UpdateEmployeeCommand command)
    {
        var employee = employeeRepository.GetById(command.Id);
        if (employee is null)
        {
            return null;
        }

        employee.Update(command.FullName, command.Email, command.HourlyRate);
        employeeRepository.Update(employee);
        employeeRepository.SaveChanges();
        return employee.ToDto();
    }

    public bool DeleteEmployee(Guid id)
    {
        if (employeeRepository.GetById(id) is null)
        {
            return false;
        }

        employeeRepository.Delete(id);
        return true;
    }
}
