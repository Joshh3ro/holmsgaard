using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Infrastructure.InMemory;

public sealed class InMemoryTimeRegistrationRepository : ITimeRegistrationRepository
{
    private readonly Dictionary<Guid, TimeRegistration> registrations = [];

    public IReadOnlyCollection<TimeRegistration> GetAll()
    {
        return registrations.Values.ToArray();
    }

    public IReadOnlyCollection<TimeRegistration> GetByEmployeeId(Guid employeeId)
    {
        return registrations.Values.Where(registration => registration.EmployeeId == employeeId).ToArray();
    }

    public TimeRegistration? GetById(Guid id)
    {
        return registrations.GetValueOrDefault(id);
    }

    public void Add(TimeRegistration registration)
    {
        registrations[registration.Id] = registration;
    }
}
