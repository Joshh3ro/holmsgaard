using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Data;
using Holmsgaard.ApiService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Holmsgaard.ApiService.Infrastructure.EfCore;

public sealed class EfCoreTimeRegistrationRepository : ITimeRegistrationRepository
{
    private readonly ApplicationDbContext _context;

    public EfCoreTimeRegistrationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<TimeRegistration> GetAll()
    {
        return [.. _context.TimeRegistrations];
    }

    public IReadOnlyCollection<TimeRegistration> GetByEmployeeId(Guid employeeId)
    {
        return [.. _context.TimeRegistrations.Where(t => t.EmployeeId == employeeId)];
    }

    public TimeRegistration? GetById(Guid id)
    {
        return _context.TimeRegistrations.Find(id);
    }

    public void Add(TimeRegistration registration)
    {
        _context.TimeRegistrations.Add(registration);
        _context.SaveChanges();
    }

    public void Update(TimeRegistration registration)
    {
        _context.TimeRegistrations.Update(registration);
    }

    public void Delete(Guid id)
    {
        var registration = _context.TimeRegistrations.Find(id);
        if (registration is not null)
        {
            _context.TimeRegistrations.Remove(registration);
            _context.SaveChanges();
        }
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
