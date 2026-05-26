using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Data;
using Holmsgaard.ApiService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Holmsgaard.ApiService.Infrastructure.EfCore;

public sealed class EfCoreActivityRepository : IActivityRepository
{
    private readonly ApplicationDbContext _context;

    public EfCoreActivityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyCollection<Activity> GetAll()
    {
        return [.. _context.Activities];
    }

    public Activity? GetById(Guid id)
    {
        return _context.Activities.Find(id);
    }

    public void Add(Activity activity)
    {
        _context.Activities.Add(activity);
    }

    public void Delete(Guid id)
    {
        var activity = _context.Activities.Find(id);
        if (activity is not null)
        {
            _context.Activities.Remove(activity);
        }
    }
}