using Holmsgaard.ApiService.Application.Commands;
using Holmsgaard.ApiService.Application.Queries;
using Holmsgaard.ApiService.Application.Services;
using Holmsgaard.ApiService.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Holmsgaard.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ActivitiesController(ActivityService activityService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyCollection<ActivityDto>> GetActivities([FromQuery] bool includeInactive = false)
    {
        return Ok(activityService.GetActivities(new GetActivitiesQuery(includeInactive)));
    }

    [HttpGet("{id:guid}")]
    public ActionResult<ActivityDto> GetActivity(Guid id)
    {
        var activity = activityService.GetActivity(new GetActivityByIdQuery(id));
        return activity is null ? NotFound() : Ok(activity);
    }

    [HttpPost]
    public ActionResult<ActivityDto> CreateActivity(CreateActivityRequest request)
    {
        try
        {
            var activity = activityService.CreateActivity(new CreateActivityCommand(request.CustomerId, request.Title, request.Description));
            return CreatedAtAction(nameof(GetActivity), new { id = activity.Id }, activity);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public ActionResult<ActivityDto> UpdateActivity(Guid id, UpdateActivityRequest request)
    {
        try
        {
            var activity = activityService.UpdateActivity(new UpdateActivityCommand(id, request.CustomerId, request.Title, request.Description));
            return activity is null ? NotFound() : Ok(activity);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteActivity(Guid id)
    {
        return activityService.DeleteActivity(id) ? NoContent() : NotFound();
    }
}
