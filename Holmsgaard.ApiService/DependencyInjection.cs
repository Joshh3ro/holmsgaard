using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Application.Services;
using Holmsgaard.ApiService.Infrastructure.EfCore;

namespace Holmsgaard.ApiService;

public static class DependencyInjection
{
    public static IServiceCollection AddHolmsgaardBackend(this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, EfCoreCustomerRepository>();
        services.AddScoped<IEmployeeRepository, EfCoreEmployeeRepository>();
        services.AddScoped<IActivityRepository, EfCoreActivityRepository>();
        services.AddScoped<IProductRepository, EfCoreProductRepository>();
        services.AddScoped<ITimeRegistrationRepository, EfCoreTimeRegistrationRepository>();

        services.AddScoped<CustomerService>();
        services.AddScoped<EmployeeService>();
        services.AddScoped<ActivityService>();
        services.AddScoped<ProductService>();
        services.AddScoped<TimeCalculationService>();

        return services;
    }
}
