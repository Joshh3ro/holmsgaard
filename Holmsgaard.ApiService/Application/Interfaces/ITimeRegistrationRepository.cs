using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Application.Interfaces;

public interface ITimeRegistrationRepository
{
    IReadOnlyCollection<TimeRegistration> GetAll();
    IReadOnlyCollection<TimeRegistration> GetByEmployeeId(Guid employeeId);
    TimeRegistration? GetById(Guid id);
    void Add(TimeRegistration registration);
}
