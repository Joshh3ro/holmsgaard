using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Application.Services;
using Holmsgaard.ApiService.Infrastructure.InMemory;

namespace Holmsgaard.ApiService;

public static class DependencyInjection
{
    public static IServiceCollection AddHolmsgaardBackend(this IServiceCollection services)
    {
        services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();
        services.AddSingleton<IEmployeeRepository, InMemoryEmployeeRepository>();
        services.AddSingleton<IActivityRepository, InMemoryActivityRepository>();
        services.AddSingleton<IProductRepository, InMemoryProductRepository>();
        services.AddSingleton<ITimeRegistrationRepository, InMemoryTimeRegistrationRepository>();

        services.AddScoped<CustomerService>();
        services.AddScoped<EmployeeService>();
        services.AddScoped<ActivityService>();
        services.AddScoped<ProductService>();
        services.AddScoped<TimeCalculationService>();

        return services;
    }
}
