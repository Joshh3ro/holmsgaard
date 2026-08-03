using Holmsgaard.ApiService.Contracts.Dto;
using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Application.Services;

internal static class MappingExtensions
{
    public static CustomerDto ToDto(this Customer customer)
    {
        return new CustomerDto(customer.Id, customer.Name, customer.Email, customer.Phone, customer.IsActive);
    }

    public static EmployeeDto ToDto(this Employee employee)
    {
        return new EmployeeDto(employee.Id, employee.FullName, employee.Email, employee.HourlyRate, employee.IsActive);
    }

    public static ActivityDto ToDto(this Activity activity)
    {
        return new ActivityDto(activity.Id, activity.CustomerId, activity.Title, activity.Description, activity.IsActive);
    }

    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.Sku,
            product.UnitPrice,
            product.StockQuantity,
            product.IsActive,
            product.Category,
            Convert.ToBase64String(product.RowVersion));
    }

    public static TimeRegistrationDto ToDto(this TimeRegistration registration)
    {
        return new TimeRegistrationDto(registration.Id, registration.EmployeeId, registration.ActivityId, registration.WorkDate, registration.Hours, registration.Note, registration.Type);
    }
}
