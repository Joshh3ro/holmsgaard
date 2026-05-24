using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Application.Interfaces;

public interface IActivityRepository
{
    IReadOnlyCollection<Activity> GetAll();
    Activity? GetById(Guid id);
    void Add(Activity activity);
    void Delete(Guid id);
}
