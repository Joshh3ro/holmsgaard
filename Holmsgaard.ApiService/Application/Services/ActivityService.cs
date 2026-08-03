using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Interfaces;
using Holmsgaard.ApiService.Application.Queries;
using Holmsgaard.ApiService.Contracts.Dto;
using Holmsgaard.ApiService.Domain.Entities;

namespace Holmsgaard.ApiService.Application.Services;

public sealed class ActivityService(IActivityRepository activityRepository, ICustomerRepository customerRepository)
{
    public IReadOnlyCollection<ActivityDto> GetActivities(GetActivitiesQuery query)
    {
        return activityRepository.GetAll()
            .Where(activity => query.IncludeInactive || activity.IsActive)
            .Select(activity => activity.ToDto())
            .ToArray();
    }

    public ActivityDto? GetActivity(GetActivityByIdQuery query)
    {
        return activityRepository.GetById(query.Id)?.ToDto();
    }

    public ActivityDto CreateActivity(CreateActivityCommand command)
    {
        EnsureCustomerExists(command.CustomerId);
        var activity = new Activity(Guid.NewGuid(), command.CustomerId, command.Title, command.Description);
        activityRepository.Add(activity);
        return activity.ToDto();
    }

    public ActivityDto? UpdateActivity(UpdateActivityCommand command)
    {
        EnsureCustomerExists(command.CustomerId);
        var activity = activityRepository.GetById(command.Id);
        if (activity is null)
        {
            return null;
        }

        activity.Update(command.CustomerId, command.Title, command.Description);
        return activity.ToDto();
    }

    public bool DeleteActivity(Guid id)
    {
        if (activityRepository.GetById(id) is null)
        {
            return false;
        }

        activityRepository.Delete(id);
        return true;
    }

    private void EnsureCustomerExists(Guid customerId)
    {
        if (customerRepository.GetById(customerId) is null)
        {
            throw new InvalidOperationException("Customer does not exist.");
        }
    }
}
