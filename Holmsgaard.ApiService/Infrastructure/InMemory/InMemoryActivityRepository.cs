using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Infrastructure.InMemory;

public sealed class InMemoryActivityRepository : IActivityRepository
{
    private readonly Dictionary<Guid, Activity> activities = [];

    public InMemoryActivityRepository()
    {
        var activity = new Activity(
            Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "Havearbejde",
            "Demo aktivitet til frontend og test.");
        activities[activity.Id] = activity;
    }

    public IReadOnlyCollection<Activity> GetAll()
    {
        return activities.Values.ToArray();
    }

    public Activity? GetById(Guid id)
    {
        return activities.GetValueOrDefault(id);
    }

    public void Add(Activity activity)
    {
        activities[activity.Id] = activity;
    }

    public void Delete(Guid id)
    {
        activities.Remove(id);
    }
}
